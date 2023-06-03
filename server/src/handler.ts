import { RawData, WebSocket } from "ws";

import GameEventHandler, { GameEvent } from "@/event";
import Server from "@/server";

/**
 * A class that handles messages for the WebSocket messages
 */
export default class MessageHandler {
  /** All of the loaded game event handlers */
  handlers: Record<string, GameEventHandler> = {};

  /** The WebSocket server, required because each handler is initialized with it */
  private wss: Server;

  /**
   * Create a new MessageHandler instance
   * @param {Server} wss The WebSocket server
   * @param {boolean} registerAll Whether to load all game event handlers at initialization
   * @throws Error
   */
  constructor(wss: Server, registerAll: boolean = true) {
    this.wss = wss;

    // Register all game event handlers
    if (registerAll) this.registerAllHandlers();
  }

  /**
   * Handle a message
   * @param {WebSocket} ws The WebSocket connection
   * @param {RawData} data The message data
   * @throws Error
   */
  async handle(ws: WebSocket, data: RawData): Promise<void> {
    // Parse the message as JSON
    let message: GameEvent;
    try {
      message = JSON.parse(data.toString());

      // Check if the message type is valid
      if (typeof message.type !== "string" || !Object.keys(this.handlers).includes(message.type))
        throw new Error("Invalid message type");
    } catch (err) {
      // If an error occurs, send an error message to the client
      return MessageHandler.sendError(ws, err as any);
    }

    // Call the handler for the message type
    try {
      await this.handlers[message.type].handle(ws, message);
    } catch (err) {
      // If an error occurs, send an error message to the client
      MessageHandler.sendError(ws, err as any);
    }
  }

  /**
   * Send an error message to the client
   * @param {WebSocket} ws The WebSocket connection
   * @param {string | Error} err The error object or message
   */
  static sendError(ws: WebSocket, err: string | Error): void {
    // If the error is an Error object, get the message
    if (err instanceof Error) err = err.message;

    // Send the error message
    ws.send(JSON.stringify({ type: "error", message: err }));

    // Log the error
    console.error(new Date(), err);
  }

  /**
   * Register a new game event handler
   * @param {GameEventHandler} Handler The handler to register (must extend GameEventHandler)
   * @throws Error
   */
  register<T extends typeof GameEventHandler>(Handler: T): void {
    // Check if HandlerClass is a class (typeof returns "function" for classes)
    if (typeof Handler !== "function") throw new Error("Handler isn't a class!");

    // Create a new instance of the handler
    // @ts-ignore (I honestly dunno anymore, this ain't abstract... ts thinks it is tho)
    const handler: InstanceType<T> = new Handler(this.wss);

    // Check if the handler is valid (i.e. extends GameEventHandler)
    if (!(handler instanceof GameEventHandler))
      throw new Error(`Handler ${Handler.name} doesn't extend GameEventHandler!`);

    // Register the handler (rewrite if already exists)
    this.handlers[Handler.type] = handler;
  }

  /**
   * Register all game event handlers
   * (these are defined here cuz I don't want to have them in each of the ws connections)
   * @throws Error
   */
  async registerAllHandlers(): Promise<void> {
    // Import all handlers from the events folder
    const handlers = require.context("./events/", true, /\.ts$/);

    for (const handler of handlers.keys()) {
      // We can assume that the default export is the a class that extends GameEventHandler
      const module = handlers(handler); // Import the module (the handler class)
      const Handler = module.default as typeof GameEventHandler;

      // Pass to the register function
      this.register(Handler);
    }
  }
}
