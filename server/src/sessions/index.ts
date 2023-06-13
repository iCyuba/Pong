import { remove } from "lodash-es";

import * as Messages from "@/messages";

import Game from "@/game";
import Handler from "@/handlers";
import Invite from "@/invite";
import Player from "@/players/player";
import Session from "@/sessions/session";

/**
 * Manages the sessions of the users
 *
 * Responsible for handling the creation of sessions and the joining of players
 *
 * And also the leaving of players
 */
export default class Sessions extends Handler {
  /** A list of all the sessions (please don't mutate this directly..) */
  readonly all: Session[] = [];

  /** Whether there's at least one active session */
  get active(): boolean {
    return this.all.length !== 0 && this.all.every(session => session.running);
  }

  constructor(game: Game) {
    super(game);

    // Start the game loop
    this.startGameLoop();
  }

  /**
   * Create a new session from an invite
   * @param {Invite} invite The invite to create the session from
   * @returns {Session} The created session
   */
  create(invite: Invite): Session {
    // Create a new session
    const session = new Session(this.game, invite);

    // Add the session to the list of all sessions
    this.all.push(session);

    // Start the session
    session.start(); // TODO: REMOVE!!!

    // Inform all players who aren't in a session (excluding these two) that a new session has been created
    this.game.players.broadcast(
      Messages.Create(invite.player1, invite.player2),
      this.game.players.notInSession
    );

    return session;
  }

  /**
   * Remove a session from the list of all sessions
   * @param {Session} session The session to remove
   */
  remove(session: Session) {
    // Remove the session from the list of all sessions
    remove(this.all, session); // Note: this is lodash-es. not the method 2 lines above

    // End the session
    session.end();

    // TODO: Inform the players that the session has been removed
  }

  /** Game loop interval */
  private interval?: NodeJS.Timeout;

  /**
   * Start the game loop
   *
   * Called during initialization
   */
  startGameLoop() {
    // console.log("Starting game loop", this.interval);

    // If the game loop is already running, don't start it again
    if (this.interval) return;

    // console.log("Starting game loop");

    // Start the game loop (every 10ms)
    this.interval = setInterval(this.gameLoop.bind(this), 10);
  }

  /**
   * Stop the game loop
   *
   * TODO: This is called when the server is shutting down
   */
  stopGameLoop() {
    // If the game loop is not running, don't stop it
    if (!this.interval) return;

    // Stop the game loop
    clearInterval(this.interval);
    this.interval = undefined;
  }

  /** The timestamp of the last tick */
  private lastTick: number = Date.now();

  /**
   * The game loop. Calls the update method on each session
   *
   * This method is meant to be called every 10ms but it won't be cuz it's called by setInterval.. anywayy
   */

  private gameLoop() {
    // The delta time (time since last tick in ms) [0 on first tick]
    const delta = Date.now() - this.lastTick;

    // Update the last tick timestamp
    this.lastTick = Date.now();

    // console.log("Game loop", delta);

    // console.time("Game loop");

    // Call the game loop for each session
    this.all.forEach(session => session.update(delta));

    // console.timeEnd("Game loop");
  }

  /**
   * Find a session from a player
   * @param {Player} player The player to find the session for
   * @returns {Session | undefined} The session the player is in, or undefined if the player is not in a session
   * @see {Session}
   */
  fromPlayer(player: Player): Session | undefined {
    return this.all.find(session => session.hasPlayer(player));
  }
}
