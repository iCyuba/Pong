import { WebSocket } from "ws";

import Server from "@/server";

export interface GameEvent {
  type: string;
}

/**
 * A base class for WebSocket game event handlers
 * @abstract
 */
export default abstract class GameEventHandler<Event extends GameEvent = GameEvent> {
  /** The type of event that this handler handles */
  static type: string;

  /** The WebSocket server, used inside the handle function when accessing the game class */
  wss: Server;

  /**
   * A function that handles the game event
   * @param {WebSocket} connection The WebSocket connection that sent the event
   * @param {GameEvent} event The event that was sent
   */
  abstract handle(connection: WebSocket, event: Event): void | Promise<void>;

  constructor(wss: Server) {
    // Remember the WSServer. This is needed for the handler to be able to access the game
    this.wss = wss;
  }
}
