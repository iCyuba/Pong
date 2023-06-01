import Player from "@/player";

interface ListMessage {
  type: "list";
  players: string[];
}

/**
 * List all players who aren't in a session (can be used to list all players. no reason to tho)
 * Sent to a player on registration, on session end, and on request
 * @param {Player[]} players The players to list
 * @param {Player} requestedBy The player who requested the list event (their name won't be sent)
 * @returns {ListMessage} A List message
 */
function List(players: Player[], requestedBy: Player): ListMessage {
  return {
    type: "list",
    players: players.filter(player => player !== requestedBy).map(player => player.name),
  };
}

export default List;
