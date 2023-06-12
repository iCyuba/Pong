import Player from "@/players/player";

/**
 * A message that is sent when a session is created
 *
 * Sent to all players who are not in a session (but including the two players in the new session)
 */
export interface CreateMessage {
  type: "create";
  player1: string;
  player2: string;
}

/**
 * Inform everyone that a session has been created between two players
 *
 * Sent to all players who are not in a session (but including the two players in the new session)
 * @param {Player} player1 The player who sent the Create
 * @param {Player} player2 The player who received the Create
 * @returns {CreateMessage} An Create message
 */
function Create(player1: Player, player2: Player): CreateMessage {
  return {
    type: "create",
    player1: player1.name,
    player2: player2.name,
  };
}

export default Create;
