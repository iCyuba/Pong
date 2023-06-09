import Game from "@/game";
import Invite from "@/invite";
import * as Messages from "@/messages";
import Player from "@/players/player";
import Ball from "@/sessions/ball";

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
  running = true; // TODO: FALSE by default!!

  /** The scores for each player */
  scores: Record<Player["name"], number>;

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

    // Empty scores
    this.scores = {
      [this.player1.name]: 0,
      [this.player2.name]: 0,
    };

    // Event listeners for the ball
    this.ball.on("bounce", this.bounce.bind(this));
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
   * A bounce happened
   */
  private bounce() {
    // Send a bounce message to both players
    this.game.players.broadcast(Messages.Bounce(this.ball), [this.player1, this.player2]);
  }
}
