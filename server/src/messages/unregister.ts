import Player from "@/players/player";

/**
 * A message that is sent when a user unregisters
 * Sent to all players who aren't in a session
 */
export interface UnregisterMessage {
  type: "unregister";
  name: string;
}

/**
 * Unregister a user
 * Sent to all players who aren't in a session
 * @param {Player} player The player who registered
 * @returns {UnregisterMessage} A Register message
 */
export function Unregister(player: Player): UnregisterMessage {
  return {
    type: "unregister",
    name: player.name,
  };
}

export default Unregister;
