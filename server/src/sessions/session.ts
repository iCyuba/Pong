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

  /** Whether the players have moved their paddles. Idk what to name this lmaoo */
  consent: Record<SessionPlayer, boolean> = {
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

  /**
   * Start the game
   * (Called when both players have moved their paddles)
   */
  start() {
    // Check if the game is already running
    if (this.running) return;

    // Check if both players have moved their paddles
    // TODO: UNCOMMENT THIS!!
    // if (!this.consent[SessionPlayer.Player1] || !this.consent[SessionPlayer.Player2]) return;

    // Start the game loop in half a second
    // This is intentional, so the players have some time to react? ig. idkkk
    setTimeout(() => {
      // Set the game to running and start the game loop
      this.running = true;
      this.game.sessions.startGameLoop();

      // Send the start message
      this.player1.send(Messages.Start(this.ball, false));
      this.player2.send(Messages.Start(this.ball, true));
    }, 500);
  }

  /**
   * Update the game state based on the delta time
   * This gets called by the Sessions class every 20ms
   * @param {number} delta The time since the last tick in ms
   */
  update(delta: number) {
    // If the game is not running, don't continue the game loop
    if (!this.running) return;

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
    this.player1.send(Messages.Score(this.scores, player));
    this.player2.send(Messages.Score(this.scores, player));

    // Reset the ball position and make it go towards the other player
    this.ball.reset(player);

    // Stop the game loop for half a second
    this.running = false;

    // And start again lmaoo
    this.start(); // This gets called in half a second
  }
}
