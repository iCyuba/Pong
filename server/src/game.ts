import Invites from "@/invites";
import Players from "@/players";
import Sessions from "@/sessions";

/**
 * A class that handles the game, players, and sessions
 */
export default class Game {
  /** A list of all the players in the game */
  players: Players;
  /** A list of all the invites */
  invites: Invites;
  /** A list of all the active sessions */
  sessions: Sessions;

  /** Create a new game and all the required handlers */
  constructor() {
    this.players = new Players(this);
    this.invites = new Invites(this);
    this.sessions = new Sessions(this);
  }

  /**
   * End the game
   *
   * Called when the server is shutting down
   */
  stop() {
    // Stop the game loop
    this.sessions.stopGameLoop();

    // TODO: Kick all players
  }
}
