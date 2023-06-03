import { WebSocket } from "ws";

import Player from "@/player";
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
   * @param {WebSocket} ws The WebSocket connection that sent the event
   * @param {GameEvent} event The event that was sent
   */
  abstract handle(ws: WebSocket, event: Event): void | Promise<void>;

  constructor(wss: Server) {
    // Remember the WSServer. This is needed for the handler to be able to access the game
    this.wss = wss;
  }
}

export abstract class RegisteredGameEventHandler<
  Event extends GameEvent = GameEvent
> extends GameEventHandler<Event> {
  /**
   * Checks if the player is registered and calls the handleRegistered function
   */
  handle(ws: WebSocket, event: Event): void | Promise<void> {
    // Get the player that sent the event
    const player = this.wss.game.getPlayer(ws);

    // If the player is not registered, throw an error
    if (!player) throw new Error("Not registered");

    // Call the handleRegistered function
    return this.handleRegistered(player, event);
  }

  /**
   * A function that handles the game event
   * @param {WebSocket} player The player that sent the event
   * @param {GameEvent} event The event that was sent
   */
  abstract handleRegistered(player: Player, event: Event): void | Promise<void>;
}
