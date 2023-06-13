import { WebSocket } from "@/server";

import * as Messages from "@/messages";

import Game from "@/game";
import Handler from "@/handlers";
import EventHandler, { Event } from "@/handlers/event";

/**
 * A class that handles messages for the WebSocket messages
 */
export default class MessageHandler extends Handler {
  /** All of the loaded game event handlers */
  handlers: Record<string, EventHandler> = {};

  /** Text decoder for decoding ArrayBuffer messages */
  decoder: TextDecoder;

  constructor(game: Game, decoder: TextDecoder = new TextDecoder()) {
    super(game);

    // Set the decoder (passed by the server. doesn't have to be ig)
    this.decoder = decoder;

    // Register all game event handlers
    this.registerAllHandlers();
  }

  /**
   * Handle a message
   * @param {WebSocket} ws The WebSocket connection
   * @param {ArrayBuffer} data The message data
   */
  async handle(ws: WebSocket, data: ArrayBuffer): Promise<void> {
    // Parse the message as JSON
    let message: Event;
    try {
      message = JSON.parse(this.decoder.decode(data));

      // Check if the message type is valid
      if (typeof message.type !== "string" || !Object.keys(this.handlers).includes(message.type))
        throw new Error("Invalid message type");
    } catch (err) {
      // If an error occurs, send an error message to the client
      ws.send(JSON.stringify(Messages.Error(err as any)));

      return;
    }

    // Call the handler for the message type
    try {
      await this.handlers[message.type].handle(ws, message);
    } catch (err) {
      // If an error occurs, send an error message to the client
      // yes, as any. sue me
      ws.send(JSON.stringify(Messages.Error(err as any)));
    }
  }

  /**
   * Register a new game event handler
   * @param {EventHandler} Handler The handler to register (must extend GameEventHandler)
   * @throws Error
   */
  private register<T extends typeof EventHandler>(Handler: T): void {
    // Check if HandlerClass is a class (typeof returns "function" for classes)
    if (typeof Handler !== "function") throw new Error("Handler isn't a class!");

    // Create a new instance of the handler
    // @ts-ignore (I honestly dunno anymore, this ain't abstract... ts thinks it is tho)
    const handler: InstanceType<T> = new Handler(this.game);

    // Check if the handler is valid (i.e. extends GameEventHandler)
    if (!(handler instanceof EventHandler))
      throw new Error(`Handler ${Handler.name} doesn't extend GameEventHandler!`);

    // Register the handler (rewrite if already exists)
    this.handlers[Handler.type] = handler;
  }

  /**
   * Register all game event handlers
   * (these are defined here cuz I don't want to have them in each of the ws connections)
   * @throws Error
   */
  private async registerAllHandlers(): Promise<void> {
    // Import all handlers from the events folder
    const handlers = require.context("../events/", true, /\.ts$/);

    for (const handler of handlers.keys()) {
      // We can assume that the default export is the a class that extends GameEventHandler
      const module = handlers(handler); // Import the module (the handler class)
      const Handler = module.default as typeof EventHandler;

      // Pass to the register function
      this.register(Handler);
    }
  }
}
