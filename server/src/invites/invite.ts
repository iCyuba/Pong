import Player from "@/players/player";

/**
 * Represents an invite from a player to another player.
 */
export default class Invite {
  /** The player that sent the invite */
  player1: Player;
  /** The player that the invite was sent to */
  player2: Player;

  /**
   * Create a new invite
   * @param {Player} player1 The player that sent the invite
   * @param {Player} player2 The player that received the invite
   */
  constructor(player1: Player, player2: Player) {
    this.player1 = player1;
    this.player2 = player2;
  }
}
