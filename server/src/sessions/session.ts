import * as Messages from "@/messages";

import Invite from "@/invite";
import Player from "@/players/player";
import Ball from "@/sessions/ball";
import Paddle, { Velocity } from "@/sessions/paddle";

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

  /** The paddles of each player */
  paddles: Record<SessionPlayer, Paddle> = {
    [SessionPlayer.Player1]: new Paddle(),
    [SessionPlayer.Player2]: new Paddle(),
  };

  /** Whether the players have sent a "ready" message */
  isReady: Record<SessionPlayer, boolean> = {
    [SessionPlayer.Player1]: false,
    [SessionPlayer.Player2]: false,
  };

  /** The ball in the game */
  ball = new Ball(this);

  /**
   * Create a new session
   * Anddd once again, this should only be called by the Sessions class
   * @param {Invite} invite The invite that created this session
   */
  constructor(invite: Invite) {
    this.player1 = invite.player1;
    this.player2 = invite.player2;

    // Event listeners for the ball
    this.ball.on("bounce", this.bounce.bind(this));
    this.ball.on("outOfBounds", this.goal.bind(this));
  }

  /**
   * Get a SessionPlayer from a player
   */
  getSessionPlayer(player: Player): SessionPlayer {
    return player === this.player1 ? SessionPlayer.Player1 : SessionPlayer.Player2;
  }

  /**
   * Start the game
   *
   * Called when a player sends a "ready" message or when a game ends
   *
   * Both players have to send a ready message to start the game
   */
  start() {
    // Check if the game is already running or already starting
    if (this.running || this.startTimestamp !== undefined) return;

    // Check if both players are ready
    if (!this.isReady[SessionPlayer.Player1] || !this.isReady[SessionPlayer.Player2]) return;

    // Set the start timestamp (this is used by the real startGame() method)
    // This is an alternative to using setTimeout() to start the game after a delay
    // I'm doing this because I already have setInterval() for the game loop
    this.startTimestamp = Date.now();

    // Send the start message with the timestamp when the game should start
    this.player1.send(Messages.Start(this.ball, false));
    this.player2.send(Messages.Start(this.ball, true));
  }

  /**
   * End the game
   *
   * Called when a player leaves the game or when a player sends a "leave" message.
   *
   * Note: This doesn't send any messages to the players, that's the responsibility of Sessions.remove()!
   */
  end() {
    // Set the game to not running
    this.running = false;

    // If there's a start timestamp, remove it
    this.startTimestamp = undefined;
  }

  /**
   * A player has sent a "ready" message
   *
   * This message is sent when a player presses any key (maybe I'll change this. idk)
   * @param {Player} player The player who sent the message
   */
  ready(player: Player) {
    // Check if the player is in this session
    if (!this.hasPlayer(player)) return;

    // Send the ready message to both players
    this.player1.send(Messages.Ready(player));
    this.player2.send(Messages.Ready(player));

    // Set the player to ready
    this.isReady[this.getSessionPlayer(player)] = true;

    // Try to start the game (this will not do anything if the other player is not ready)
    this.start();
  }

  /**
   * The player sent a "move" message
   *
   * This message is sent when a player starts moving their paddle
   * @param {Player} player The player who sent the message
   * @param {number} position The position of the paddle
   * @param {Velocity} velocity The new speed of the paddle
   */
  move(player: Player, position: number, velocity: Velocity) {
    // Check if the player is in this session
    if (!this.hasPlayer(player)) return;

    // Find the paddle of the player
    const sessionPlayer = this.getSessionPlayer(player);
    const paddle = this.paddles[sessionPlayer];

    // Update the paddle position and velocity
    paddle.position = position;
    paddle.velocity = velocity;

    // Send the move message to the other player
    const otherPlayer = sessionPlayer === SessionPlayer.Player1 ? this.player2 : this.player1;
    otherPlayer.send(Messages.Move(paddle));
  }

  /**
   * Update the game state.
   *
   * This gets called by the Sessions class every tick
   * @param {number} delta The time since the last tick in milliseconds
   */
  update(delta: number) {
    // If the game is not running, try checking in with the startGame() method
    if (!this.running) return this.startGame();

    // Update the paddle positions first
    this.paddles[SessionPlayer.Player1].move(delta);
    this.paddles[SessionPlayer.Player2].move(delta);

    // Update the ball position based on the velocity and delta time
    this.ball.move(delta);
  }

  /** The timestamp of when start() was called */
  private startTimestamp?: number;

  /**
   * Called when the game should actually start (about 3 seconds after the public start() method is called)
   */
  private startGame() {
    // Check if 1. start() was called and 2. the game is not already running and 3. the game was not started less than 3 seconds ago
    if (!this.startTimestamp || this.running || this.startTimestamp + 3000 > Date.now()) return;

    // Set the game to running and remove the start timestamp
    this.running = true;
    this.startTimestamp = undefined;
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

    // Stop the session for 3 seconds
    this.end(); // This gets called immediately

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
