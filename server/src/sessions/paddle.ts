export enum Velocity {
  Up = 80,
  Stop = 0,
  Down = -80,
}

/**
 * A paddle in the game. This is the thing that the players control
 */
export default class Paddle {
  /** The height of the paddle */
  static readonly height = 20;

  /** The position of the paddle */
  position = 50;

  /** The speed of the paddle */
  velocity = Velocity.Stop;

  /**
   * Move the paddle based on the velocity and delta time
   * @param {number} delta The time since the last tick in ms
   */
  move(delta: number) {
    this.position += this.velocity * (delta / 1000);

    // Make sure the paddle doesn't go out of bounds
    if (this.top < 0) {
      this.top = 0;
    } else if (this.bottom > 100) {
      this.bottom = 100;
    }
  }

  /** Get the top position of the paddle */
  get top() {
    return this.position - Paddle.height / 2;
  }

  set top(value: number) {
    this.position = value + Paddle.height / 2;
  }

  /** Get the bottom position of the paddle */
  get bottom() {
    return this.position + Paddle.height / 2;
  }

  set bottom(value: number) {
    this.position = value - Paddle.height / 2;
  }
}
