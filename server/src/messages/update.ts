import Start, { StartMessage } from "@/messages/start";

import Ball from "@/sessions/ball";

/**
 * A message that is sent when the ball is updated in a session
 *
 * Sent to both players in the session
 *
 * Note: This extends the StartMessage because they're quite similar. Only here we also send the position of the ball
 */
export interface UpdateMessage extends Omit<StartMessage, "type"> {
  type: "update";

  // Position
  posX: number;
  posY: number;
}

/**
 * Notify that the velocity of the ball has changed
 *
 * Sent to both players in the session
 * @param {Ball} ball The ball to send the update message for
 * @param {boolean} reverse Whether to flip the game. This is what player 2 sees. (both are on the left side of the screen)
 * @returns {UpdateMessage} An Update message
 */
function Update(ball: Ball, reverse: boolean): UpdateMessage {
  // Get the base StartMessage
  const { velX, velY } = Start(ball, reverse);

  // Get the x and y coordinates of the ball
  let { x: posX, y: posY } = ball.position;

  // Reverse the X axis if this is player 2's view
  if (reverse) {
    posX = 100 - posX;
  }

  return {
    type: "update",
    posX,
    posY,
    velX,
    velY,
  };
}

export default Update;
