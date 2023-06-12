import { WebSocket } from "ws";

import Handler from "@/handlers";
import Player from "@/players/player";
import Session from "@/sessions/session";

export interface Event {
  type: string;
}

/**
 * A base class for WebSocket game event handlers
 * @abstract
 */
export default abstract class EventHandler<T extends Event = Event> extends Handler {
  /** The type of event that this handler handles */
  static type: string;

  /**
   * A function that handles the game event
   * @param {WebSocket} ws The WebSocket connection that sent the event
   * @param {T} event The event that was sent
   */
  abstract handle(ws: WebSocket, event: T): void | Promise<void>;
}

/**
 * A base class for WebSocket game event handlers that require the player to be registered
 * @abstract
 * @extends EventHandler
 */
export abstract class RegisteredEventHandler<T extends Event> extends EventHandler<T> {
  /**
   * Checks if the player is registered and calls the handleRegistered function
   * @param {WebSocket} ws The WebSocket connection that sent the event
   * @param {T} event The event that was sent
   */
  handle(ws: WebSocket, event: T): void | Promise<void> {
    // Get the player that sent the event
    const player = this.game.players.fromWebSocket(ws);

    // If the player is not registered, throw an error
    if (!player) throw new Error("Not registered");

    // Call the handleRegistered function
    return this.handleRegistered(player, event);
  }

  /**
   * A function that handles the game event
   * @param {Player} player The player that sent the event
   * @param {T} event The event that was sent
   */
  abstract handleRegistered(player: Player, event: T): void | Promise<void>;
}

/**
 * A base class for WebSocket game event handlers that require the player to be in a session
 * @abstract
 * @extends RegisteredEventHandler
 */
export abstract class SessionEventHandler<T extends Event> extends RegisteredEventHandler<T> {
  /**
   * Checks if the player is in a session and calls the handleSession function
   * @param {Player} player The player that sent the event
   * @param {T} event The event that was sent
   */
  handleRegistered(player: Player, event: T): void | Promise<void> {
    // Find the session that the player is in
    const session = this.game.sessions.fromPlayer(player);

    // If the player is not in a session, throw an error
    if (!session) throw new Error("Not in session");

    // Call the handleSession function
    return this.handleSession(player, session, event);
  }

  /**
   * A function that handles the game event
   * @param {Player} player The player that sent the event
   * @param {Session} session The session that the player is in
   * @param {T} event The event that was sent
   */
  abstract handleSession(player: Player, session: Session, event: T): void | Promise<void>;
}
