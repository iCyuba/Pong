import Player from "@/player";

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
