import { map, without } from "lodash-es";

import Player from "@/players/player";

/**
 * A message that is sent when a player requests a list of players
 *
 * Sent on registration, on session end, and on request
 */
export interface ListMessage {
  type: "list";
  players: string[];
}

/**
 * List all players who aren't in a session (can be used to list all players. no reason to tho)
 *
 * Sent to a player on registration, on session end, and on request
 * @param {Player[]} players The players to list
 * @param {Player} requestedBy The player who requested the list event (their name won't be sent)
 * @returns {ListMessage} A List message
 */
function List(players: Player[], requestedBy: Player): ListMessage {
  const playersToSend = without(players, requestedBy);

  return {
    type: "list",
    players: map(playersToSend, "name"), // TODO: This is only for now. Some info should be sent
  };
}

export default List;
