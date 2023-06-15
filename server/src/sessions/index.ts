import { remove } from "lodash-es";

import * as Messages from "@/messages";

import Game from "@/game";
import Handler from "@/handlers";
import Invite from "@/invites/invite";
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
    const session = new Session(invite);

    // Inform all players who aren't in a session (excluding these two) that a new session has been created
    this.game.players.broadcastNotInSession(Messages.Create(invite.player1, invite.player2));

    // Delete all invites that involve either player
    this.game.invites.removePlayer(invite.player1);
    this.game.invites.removePlayer(invite.player2);

    // Add the session to the list of all sessions
    this.all.push(session);

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

    // Inform the players that the session has been removed
    this.game.players.broadcastNotInSession(Messages.End(session.player1, session.player2));
  }

  /**
   * Remove any session that the given player is in
   * @param {Player} player The player to remove from any session
   */
  removePlayer(player: Player) {
    // Find the session that the player is in
    const session = this.fromPlayer(player);

    // If the player is not in a session, don't do anything
    if (!session) return;

    // Remove the session
    this.remove(session);
  }

  /** Game loop interval */
  private interval?: NodeJS.Timeout;

  /**
   * Start the game loop
   *
   * Called during initialization
   */
  startGameLoop() {
    // If the game loop is already running, don't start it again
    if (this.interval) return;

    // Start the game loop (every 10ms)
    this.interval = setInterval(this.gameLoop.bind(this), 10);
  }

  /**
   * Stop the game loop
   *
   * This is called when the server is shutting down
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

    // Call the game loop for each session
    try {
      this.all.forEach(session => session.update(delta));
    } catch (err) {
      // This is just to prevent the server from not continuing with the game loop if an error occurs
      console.error(err);
    }
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
