import { IncomingMessage } from "http";

import { ServerOptions, WebSocket, WebSocketServer } from "ws";

import Game from "@/game";
import MessageHandler from "@/handler";

/**
 * A WebSocket server for the game. This class is responsible for handling all connections
 * @extends WebSocketServer
 */
export default class Server extends WebSocketServer {
  /** An instance of the Game class. This is what contains the info about the players, sessions, ... */
  readonly game: Game = new Game(this);

  /** A handler for the messages. It's a separate class because it loads all of the GameEventHandlers */
  readonly messageHandler: MessageHandler = new MessageHandler(this);

  /** Whether the server is running in development mode */
  dev = process.env.NODE_ENV === "development";

  /**
   * Create a new WSServer
   * @param {ServerOptions} options The options to pass to the WebSocket server
   * @param {() => void} callback An optional callback to run when the server is ready
   */
  constructor(options: ServerOptions, callback?: () => void) {
    // Create a new WebSocketServer
    super(options, callback);

    // Bind the connection event handler to the class
    this.on("connection", this.onConnection);
  }

  /**
   * An event handler for when a new WebSocket connection is made
   * @param {WebSocket} ws A WebSocket connection
   */
  private onConnection(ws: WebSocket, req: IncomingMessage): void {
    // If the server is running in devmode, log the connection
    if (this.dev) console.log(new Date(), "Connect", req.socket.remoteAddress);

    // Attach the event handlers to the websocket
    ws.on("message", data => this.messageHandler.handle(ws, data));
    ws.on("close", () => this.onClose(ws));
  }

  /**
   * An event handler for when a WebSocket connection is closed
   * @param {WebSocket} ws A WebSocket connection
   */
  private onClose(ws: WebSocket): void {
    // If the server is running in devmode, log the close
    if (this.dev) console.log(`${new Date()} - close`);

    // Remove the player from the game (if they're not registered, this does nothing)
    this.game.removePlayer(ws);
  }
}
