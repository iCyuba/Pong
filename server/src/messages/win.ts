import Player from "@/players/player";

/**
 * A message that is sent when a player wins a game
 *
 * Sent to both players in the session.
 */
export interface WinMessage {
  type: "win";
  player: string;
}

/**
 * Inform the players that a someone has won a game.
 *
 * Sent to both players in the session.
 * @param {Player} player The player who won
 * @returns {WinMessage} A WIn message
 */
function Win(player: Player): WinMessage {
  return {
    type: "win",
    player: player.name,
  };
}

export default Win;
