import {
  App,
  TemplatedApp,
  us_listen_socket,
  us_listen_socket_close,
  us_socket_local_port,
  WebSocket as uWebSocket,
} from "uWebSockets.js";

import Game from "@/game";
import MessageHandler from "@/handlers/message";

/** A WebSocket connection with some extra data ig */
export type WebSocket<UserData = any> = uWebSocket<UserData>;

/**
 * A WebSocket server for the game. This class is responsible for handling all connections
 */
export default class Server {
  /** An instance of the Game class. This is what contains the info about the players, sessions, ... */
  readonly game: Game = new Game();

  /** A handler for the messages. It's a separate class because it loads all of the EventHandlers */
  readonly messageHandler: MessageHandler = new MessageHandler(this.game);

  /** An instance of the uWebSockets.js app */
  readonly app: TemplatedApp = App();

  /** The listen socket, given to us by uWebSockets.js in the listen callback */
  listenSocket?: us_listen_socket;

  /** The port the server is listening on */
  get port(): number {
    return this.listenSocket ? us_socket_local_port(this.listenSocket) : -1;
  }

  /**
   * Create a new server
   * @param {number} port The port to listen on (defaults to a random port => 0)
   * @param {() => void} callback An optional callback to run when the server is ready
   */
  constructor(port: number = 0, callback?: () => void) {
    // Handle all WebSocket connections on any path
    this.app.ws("/*", {
      /* Handlers */
      open: this.onConnection.bind(this),
      message: this.messageHandler.handle.bind(this.messageHandler),
      close: this.onClose.bind(this),
    });

    // Listen on the given port, defaults to a random port (0)
    this.app.listen(port, listenSocket => {
      // Save the listen socket
      this.listenSocket = listenSocket;

      // Try to run the callback if there is one
      callback?.();
    });
  }

  /** Gracefully close the server */
  close(): void {
    // Close the listen socket (if it exists)
    if (this.listenSocket) us_listen_socket_close(this.listenSocket);
    this.listenSocket = undefined;

    // TODO: Close the game
    // this.game.close();
    this.game.sessions.stopGameLoop();
  }

  /** Text decoder for decoding ArrayBuffer addresses */
  decoder = new TextDecoder();

  /**
   * An event handler for when a new WebSocket connection is made
   * @param {WebSocket} ws A WebSocket connection
   */
  private onConnection(ws: WebSocket): void {
    // Log the connection
    console.log(new Date(), "Connect", this.decoder.decode(ws.getRemoteAddressAsText()));
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

  /**
   * Closes the server
   */
  override close(cb?: (err?: Error) => void): void {
    super.close(cb);

    // Stop the game
    this.game.stop();
  }
}
