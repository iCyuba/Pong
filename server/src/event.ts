import { WebSocket } from "ws";

import Game from "@/game";
import Player from "@/players/player";
import Session from "@/sessions/session";

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

  /** The game instace, used inside the handle function */
  game: Game;

  /**
   * A function that handles the game event
   * @param {WebSocket} ws The WebSocket connection that sent the event
   * @param {GameEvent} event The event that was sent
   */
  abstract handle(ws: WebSocket, event: Event): void | Promise<void>;

  constructor(game: Game) {
    this.game = game;
  }
}

/**
 * A base class for WebSocket game event handlers that require the player to be registered
 * @abstract
 * @extends GameEventHandler
 */
export abstract class RegisteredGameEventHandler<
  Event extends GameEvent = GameEvent
> extends GameEventHandler<Event> {
  /**
   * Checks if the player is registered and calls the handleRegistered function
   */
  handle(ws: WebSocket, event: Event): void | Promise<void> {
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
   * @param {GameEvent} event The event that was sent
   */
  abstract handleRegistered(player: Player, event: Event): void | Promise<void>;
}

/**
 * A base class for WebSocket game event handlers that require the player to be in a session
 * @abstract
 * @extends RegisteredGameEventHandler
 */
export abstract class SessionGameEventHandler<
  Event extends GameEvent = GameEvent
> extends RegisteredGameEventHandler<Event> {
  /**
   * Checks if the player is in a session and calls the handleSession function
   */
  handleRegistered(player: Player, event: Event): void | Promise<void> {
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
   * @param {GameEvent} event The event that was sent
   */
  abstract handleSession(player: Player, session: Session, event: Event): void | Promise<void>;
}
