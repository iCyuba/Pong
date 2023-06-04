import Player from "@/player";

export interface RegisterMessage {
  type: "register";
  name: string;
}

/**
 * Register a new user
 * Sent to all players who aren't in a session
 * @param {Player} player The player who registered
 * @returns {RegisterMessage} A Register message
 */
export function Register(player: Player): RegisterMessage {
  return {
    type: "register",
    name: player.name,
  };
}

export default Register;
