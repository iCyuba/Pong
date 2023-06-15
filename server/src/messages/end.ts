import Player from "@/players/player";

/**
 * A message that is sent when a session is ended
 *
 * Sent to all players who are not in a session (including the two players who were in the session)
 */
export interface EndMessage {
  type: "end";
  player1: string;
  player2: string;
}

/**
 * Inform everyone that a session has ended.
 *
 * Sent to all players who are not in a session (including the two players who were in the session)
 *
 * Note: the order of the parameters is not important
 * @param {Player} player1 Player1
 * @param {Player} player2 Player2
 * @returns {EndMessage} An End message
 */
function End(player1: Player, player2: Player): EndMessage {
  return {
    type: "end",
    player1: player1.name,
    player2: player2.name,
  };
}

export default End;
