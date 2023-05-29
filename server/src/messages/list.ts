import Player from "@/player";

interface ListMessage {
  type: "list";
  players: string[];
}

/**
 * List all players who aren't in a session (can be used to list all players. no reason to tho)
 * Sent to a player on registration, on session end, and on request
 * @param {Player[]} players The players to list
 * @returns {ListMessage} A List message
 */
function List(players: Player[]): ListMessage {
  return {
    type: "list",
    players: players.map(player => player.name),
  };
}

export default List;
