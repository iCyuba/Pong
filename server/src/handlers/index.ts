import Game from "@/game";

/**
 * A base class for a game handler
 * @abstract
 */
export default abstract class Handler {
  /** The game instace, used by the handler */
  protected readonly game: Game;

  /**
   * Create a new handler for a Game class
   * @param {Game} game The Game to create the handler for
   */
  constructor(game: Game) {
    this.game = game;
  }
}
