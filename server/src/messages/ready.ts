import Player from "@/players/player";

/**
 * A response to a ready event
 *
 * Sent to all players in the session when the first player is ready
 */
export interface ReadyMessage {
  type: "ready";
  name: string;
}

/**
 * Respond to a ready event
 *
 * Sent to all players in the session when the first player is ready
 * @param {Player} player The player who is ready
 * @returns {ReadyMessage} A Ready message
 */
export function Ready(player: Player): ReadyMessage {
  return {
    type: "ready",
    name: player.name,
  };
}

export default Ready;
