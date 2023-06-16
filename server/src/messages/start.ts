import Ball from "@/sessions/ball";

/**
 * A message that is sent when the session starts
 *
 * Note: A session doesn't start when it's created, it starts when both players are ready (moved for the first time).
 *       It also starts 3 seconds after the ball goes out of bounds (to give the players time to move their paddles)
 */
export interface StartMessage {
  type: "start";

  angle: number;
}

/**
 * A session will start in 3 seconds
 *
 * Sent to the players in the session
 * @param {Ball} ball The ball in the session
 * @param {boolean} reverse Whether to flip the game. This is what player 2 sees. (both are on the left side of the screen)
 * @returns {StartMessage} A Start message
 */
export function Start(ball: Ball, reverse: boolean): StartMessage {
  // Get the x and y velocity of the ball in the axes
  let angle = ball.angle;

  // Reverse the angle if the game is flipped
  if (reverse) {
    angle = 180 - angle;
  }

  return {
    type: "start",
    angle,
  };
}

export default Start;
