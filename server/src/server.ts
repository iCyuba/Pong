import { IncomingMessage } from "http";

import { ServerOptions, WebSocket, WebSocketServer } from "ws";

import Game from "@/game";
import MessageHandler from "@/handlers/message";

/**
 * A WebSocket server for the game. This class is responsible for handling all connections
 * @extends WebSocketServer
 */
export default class Server extends WebSocketServer {
  /** An instance of the Game class. This is what contains the info about the players, sessions, ... */
  readonly game: Game = new Game();

  /** A handler for the messages. It's a separate class because it loads all of the GameEventHandlers */
  readonly messageHandler: MessageHandler = new MessageHandler(this.game);

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
    // Log the connection
    console.log(new Date(), "Connect", req.socket.remoteAddress);

    // Attach the event handlers to the websocket
    ws.on("message", data => this.messageHandler.handle(ws, data));
    ws.on("close", () => this.onClose(ws));
  }

  /**
   * An event handler for when a WebSocket connection is closed
   * @param {WebSocket} ws A WebSocket connection
   */
  private onClose(ws: WebSocket): void {
    // Remove the player from the game (if they're not registered, this does nothing)
    const player = this.game.players.removeWebSocket(ws);

    // Log the disconnection
    // I give up. This will only log if the NODE_ENV isn't test.
    // I want this to work yk. But it will always run after the tests are done so it will always fail
    if (process.env.NODE_ENV !== "test")
      console.log(new Date(), "Closed connection", player?.name || "unregistered");
  }
}
