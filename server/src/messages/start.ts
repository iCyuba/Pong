import Ball from "@/sessions/ball";

/**
 * A message that is sent when the session starts
 *
 * Note: A session doesn't start when it's created, it starts when both players are ready (moved for the first time).
 *       It also starts 0.5 seconds after the ball goes out of bounds (to give the players time to move their paddles)
 */
export interface StartMessage {
  type: "start";

  // Velocity
  velX: number;
  velY: number;

  // Timestamp
  timestamp: number;
}

/**
 * A session has started and ball is now moving
 *
 * Sent to the players in the session
 * @param {Ball} ball The ball in the session
 * @param {boolean} reverse Whether to flip the game. This is what player 2 sees. (both are on the left side of the screen)
 * @returns {StartMessage} A Register message
 */
export function Start(ball: Ball, reverse: boolean): StartMessage {
  // Get the x and y velocity of the ball in the axes
  let { x: velX, y: velY } = ball.velocityInAxes;

  // Reverse the X axis if this is player 2's view
  if (reverse) {
    velX *= -1;
  }

  return {
    type: "start",
    velX,
    velY,
    timestamp: Date.now(),
  };
}

export default Start;
