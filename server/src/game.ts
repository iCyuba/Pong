import Invite from "@/invite";
import Players from "@/players";

/**
 * A class that handles the game, players, and sessions
 */
export default class Game {
  /** A list of all the players in the game */
  players: Players;
  /** A list of all the invites */
  invites: Invite[] = [];
  /** TODO: A list of all the active sessions */
  // sessions: any[] = [];

  /** Create a new game and all the required handlers */
  constructor() {
    this.players = new Players(this);
  }
}
