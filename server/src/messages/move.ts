import Paddle, { Speed } from "@/sessions/paddle";

/**
 * A message about a move event.
 *
 * Sent to the other player in the session when a player moves
 */
export interface MoveMessage {
  type: "move";
  position: number;
  speed: Speed;
}

/**
 * Forward a move event to the other player
 * @param {Paddle} paddle The paddle that moved
 * @returns {MoveMessage} A Move message
 */
export function Move(paddle: Paddle): MoveMessage {
  return {
    type: "move",
    position: paddle.position,
    speed: paddle.speed,
  };
}

export default Move;
