import { WebSocket } from "ws";

import Game from "@/game";
import Player from "@/players/player";

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
   * @param {WebSocket} player The player that sent the event
   * @param {GameEvent} event The event that was sent
   */
  abstract handleRegistered(player: Player, event: Event): void | Promise<void>;
}
