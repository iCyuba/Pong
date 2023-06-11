import { UpdateMessage } from "@/messages/update";
import Ball from "@/sessions/ball";

/**
 * A message that is sent when the session starts
 *
 * Note: A session doesn't start when it's created, it starts when both players are ready (moved for the first time).
 *       It also starts 0.5 seconds the ball goes out of bounds (to give the players time to move their paddles)
 *
 * It's like an update message, but it's only sent when the session starts
 * and doesn't include the position of the ball (because it's always in the middle)
 */
export interface StartMessage extends Omit<UpdateMessage, "type" | "posX" | "posY"> {
  type: "start";
}

/**
 * A session has started and the game should begin
 * Sent to the players in the session
 * @param {Ball} ball The ball in the session
 * @returns {StartMessage} A Register message
 */
export function Start(ball: Ball): StartMessage {
  // Get the x and y velocity of the ball in the axes
  const { x: velX, y: velY } = ball.velocityInAxes;

  return {
    type: "start",
    velX,
    velY,
    timestamp: Date.now(),
  };
}

export default Start;
