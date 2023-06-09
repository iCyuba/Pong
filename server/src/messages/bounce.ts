import Ball from "@/sessions/ball";

export interface BounceMessage {
  type: "bounce";

  // Position
  posX: number;
  posY: number;

  // Velocity
  velX: number;
  velY: number;
}

/**
 * Notify that the ball has bounced
 * Sent to both players in the session
 * @param {Ball} ball The ball that bounced
 * @returns {BounceMessage} A Bounce message
 */
function Bounce(ball: Ball): BounceMessage {
  // Get the x and y coordinates of the ball
  const { x: posX, y: posY } = ball.position;

  // Get the x and y velocity of the ball in the axes
  const { x: velX, y: velY } = ball.velocityInAxes;

  return {
    type: "bounce",
    posX,
    posY,
    velX,
    velY,
  };
}

export default Bounce;
