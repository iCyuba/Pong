import Ball from "@/sessions/ball";

export interface UpdateMessage {
  type: "update";

  // Position
  posX: number;
  posY: number;

  // Velocity
  velX: number;
  velY: number;

  // Timestamp
  timestamp: number;
}

/**
 * Notify that the velocity of the ball has changed
 * Sent to both players in the session
 * @param {Ball} ball The ball to send the update message for
 * @returns {UpdateMessage} An Update message
 */
function Update(ball: Ball): UpdateMessage {
  // Get the x and y coordinates of the ball
  const { x: posX, y: posY } = ball.position;

  // Get the x and y velocity of the ball in the axes
  const { x: velX, y: velY } = ball.velocityInAxes;

  return {
    type: "update",
    posX,
    posY,
    velX,
    velY,
    timestamp: Date.now(),
  };
}

export default Update;
