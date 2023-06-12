import * as Messages from "@/messages";

import Game from "@/game";
import Invite from "@/invite";
import Player from "@/players/player";
import Ball from "@/sessions/ball";

/**
 * A helper type ig. for the scores
 */
export enum SessionPlayer {
  Player1,
  Player2,
}

/**
 * A game session between two players
 *
 * This should only be created by the Sessions class
 *
 * This class is responsible for updating the game state, sending the
 * game state to the players and for ending the game..
 *
 * In case you're wondering, the dimensions of the game are 100x100 and gonna be multiplied on the client side
 */
export default class Session {
  /** The player who sent the invite */
  readonly player1: Player;

  /** The player who received the invite */
  readonly player2: Player;

  /** Whether the game is currently running */
  running = false;

  /** The scores for each player */
  scores: Record<SessionPlayer, number> = {
    [SessionPlayer.Player1]: 0,
    [SessionPlayer.Player2]: 0,
  };

  /** Whether the players have sent a "ready" message */
  isReady: Record<SessionPlayer, boolean> = {
    [SessionPlayer.Player1]: false,
    [SessionPlayer.Player2]: false,
  };

  /** The ball in the game */
  ball = new Ball();

  private readonly game: Game;

  /**
   * Create a new session
   * Anddd once again, this should only be called by the Sessions class
   * @param {Game} game The game to create the session for
   * @param {Invite} invite The invite that created this session
   */
  constructor(game: Game, invite: Invite) {
    this.player1 = invite.player1;
    this.player2 = invite.player2;

    this.game = game;

    // Event listeners for the ball
    this.ball.on("bounce", this.bounce.bind(this));
    this.ball.on("outOfBounds", this.goal.bind(this));
  }

  /** An active timeout for the start of the game */
  private startTimeout?: NodeJS.Timeout;

  /**
   * Start the game
   *
   * Called when a player sends a "ready" message or when a game ends
   *
   * Both players have to send a ready message to start the game
   */
  start() {
    // Check if the game is already running or already starting
    if (this.running || this.startTimeout) return;

    // Check if both players are ready
    if (!this.isReady[SessionPlayer.Player1] || !this.isReady[SessionPlayer.Player2]) return;

    // Start the session in 3 seconds
    // This is intentional, so the players have some time to react? ig. idkkk
    this.startTimeout = setTimeout(() => {
      // Set the game to running, set the last tick to the current time and start the game loop
      this.running = true;
      this.lastTick = Date.now();
      this.game.sessions.startGameLoop();

      // Send the start message
      this.player1.send(Messages.Start(this.ball, false));
      this.player2.send(Messages.Start(this.ball, true));

      // Clear the start timeout
      this.startTimeout = undefined;
    }, 3000);
  }

  /**
   * End the game
   *
   * Called when a player leaves the game or when a player sends a "leave" message.
   *
   * Note: This doesn't send any messages to the players, that's the responsibility of Sessions.remove()!
   */
  end() {
    // If the game isn't running, don't continue
    if (!this.running) return;

    // Set the game to not running
    this.running = false;

    // If there's a start timeout, clear it
    if (this.startTimeout) clearTimeout(this.startTimeout);
  }

  /**
   * A player has sent a "ready" message
   *
   * This message is sent when a player presses any key (maybe I'll change this. idk)
   * @param {Player} player The player who sent the message
   */
  ready(player: Player) {
    // Check if the player is in this session
    if (player !== this.player1 && player !== this.player2) return;

    // Set the player to ready
    this.isReady[player === this.player1 ? SessionPlayer.Player1 : SessionPlayer.Player2] = true;

    // Try to start the game (this will not do anything if the other player is not ready)
    this.start();
  }

  /** The timestamp of the last tick */
  private lastTick: number = Date.now();

  /**
   * Update the game state.
   *
   * This gets called by the Sessions class every tick
   */
  update() {
    // If the game is not running, don't continue updating the session
    if (!this.running) return;

    // The delta time (time since last tick in ms) [0 on first tick]
    const delta = Date.now() - this.lastTick;

    // Set lastTick to the current time
    this.lastTick = Date.now();

    // Update the ball position based on the velocity and delta time
    this.ball.move(delta);
  }

  /**
   * The ball bounced off a wall or player
   */
  private bounce() {
    // Send an update message to both players
    this.player1.send(Messages.Update(this.ball, false));
    this.player2.send(Messages.Update(this.ball, true));
  }

  /**
   * The ball went into a goal (is out of bounds)
   * @param {SessionPlayer} player The player who lost
   */
  private goal(player: SessionPlayer) {
    // Add a point to the other player
    this.scores[player]++;

    // Send a goal message to both players
    this.player1.send(Messages.Score(this.scores, SessionPlayer.Player1));
    this.player2.send(Messages.Score(this.scores, SessionPlayer.Player2));

    // Reset the ball position and make it go towards the other player
    this.ball.reset(player);

    // Stop the session for 3 seconds (if no session is running, this also stops the game loop)
    this.running = false;

    // And start again lmaoo
    this.start(); // This gets called in 3 seconds
  }

  /**
   * Check if a player is in this session
   * @param {Player} player The player to check
   * @returns {boolean} Whether the player is in this session
   */
  hasPlayer(player: Player): boolean {
    return player === this.player1 || player === this.player2;
  }
}
