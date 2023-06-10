import { memoize } from "lodash-es";
import { TypedEmitter } from "tiny-typed-emitter";

/**
 * X and Y coordinates / velocity
 *
 * Ignore it ig...
 */
interface XandY {
  x: number;
  y: number;
}

interface BallEvents {
  /** When the ball bounces */
  bounce: () => void;

  /** When the ball goes out of bounds */
  outOfBounds: (player: "player1" | "player2") => void;
}

/**
 * A ball in the game
 *
 * Side note: Unlike the rest of my classes, this emits events cuz like.. why not
 * (it's more appropriate to use events here)
 *
 * The server handles it as a square, but the client sees it as a circle lmao
 */
export default class Ball extends TypedEmitter<BallEvents> {
  /** The position of the ball */
  position: XandY;

  /** The velocity of the ball */
  velocity: number;

  /** The angle of the ball (internal) */
  private _angle!: number;

  /** The angle of the ball (in degrees) */
  get angle() {
    return this._angle;
  }

  // I did this just to make sure the angle is always between 0 and 360 lmfaoo
  set angle(angle: number) {
    // If the angle is less than 0, add 360 to it
    if (angle < 0) angle += 360;

    // Do modulo 360 to make sure the angle is between 0 and 360
    this._angle = angle % 360;
  }

  /** The radius of the ball */
  radius: number;

  /**
   * Create a new ball
   *
   * The ball is created in the middle of the screen with a random angle and speed of 100
   *
   * @param {number} radius The radius of the ball (defaults to 1) [percentages of the screen]
   */
  constructor(radius: number = 1) {
    // Call the EventEmitter constructor (so we can emit events)
    super();

    this.radius = radius;

    // The starting position of the ball is 50, 50 (in the middle of the screen)
    // On the server the positions are in percentages, but on the client they are in pixels
    // The only downside is that the client will have to be a squire (or else the ball will be an oval lmfaoo)
    this.position = { x: 50, y: 50 };

    // The starting velocity of the ball is 50 (percent per second)
    this.velocity = 50;

    // Set a random angle for the ball
    this.angle = Ball.randomAngle();
  }

  /**
   * Move the ball based on the velocity and delta time
   * @param {number} delta The time since the last tick in ms
   */
  move(delta: number) {
    // Move the ball based on the velocity and delta time
    this.position.x += (this.velocityInAxes.x * delta) / 1000;
    this.position.y += (this.velocityInAxes.y * delta) / 1000;

    // Collision detection
    this.wallBounce();
    this.goal();
  }

  /** Checks if the last time the ball was colliding with the top or bottom wall */
  private wasCollidingWithTopOrBottom: boolean = false;

  /**
   * Check if the ball is colliding with a top or bottom wall and bounce it if it is
   */
  wallBounce() {
    // If the ball is colliding with the top or bottom wall, flip the angle of the ball
    if (this.top <= 0 || this.bottom >= 100) {
      // If the ball was colliding with the top or bottom wall last tick, don't bounce the ball
      // Don't change back to true until the ball is no longer colliding with the top or bottom wall
      if (this.wasCollidingWithTopOrBottom) return;

      // Set the flag to true so we don't bounce the ball multiple times and get stuck in the wall
      this.wasCollidingWithTopOrBottom = true;

      this.angle = 360 - this.angle; // I had to draw this in the notes app lmaoo

      this.emit("bounce");
    } else {
      // Set the flag to false so we can bounce the ball again
      this.wasCollidingWithTopOrBottom = false;
    }
  }

  /**
   * Check if the ball is colliding with the left or right wall and stop the ball if it is
   */
  goal() {
    // If the ball is colliding with the left or right wall, stop the ball
    if (this.left < 0) {
      // this.emit("outOfBounds", "player2");
      // TODO: I'm bouncing the ball here
      this.angle = 180 - this.angle;

      this.emit("bounce");
    } else if (this.right > 100) {
      // this.emit("outOfBounds", "player1");
      // TODO: I'm bouncing the ball here
      this.angle = 180 - this.angle;

      this.emit("bounce");
    }
  }

  // Static methods cuz why not

  /**
   * Convert degrees to radians
   * @param {number} degrees The angle in degrees
   * @returns {number} The angle in radians
   */
  static degreesToRadians(degrees: number): number {
    return (degrees * Math.PI) / 180;
  }

  /**
   * Generate a random angle for the ball (ig can be used for other stuff, why would you tho)
   * @param {number} minAngle The minimum angle of the ball (in degrees) (defaults to 0)
   * @param {number} maxAngle The maximum angle of the ball (in degrees) (defaults to 360)
   * @returns {number} The angle of the ball (in degrees)
   */
  static randomAngle(minAngle: number = 0, maxAngle: number = 360): number {
    // Generate a random angle in degrees
    const degrees = Math.random() * (maxAngle - minAngle) + minAngle;

    return degrees;
  }

  // Helper calculated properties for the ball (used in the collision detection)
  // Please don't @ me for this. It's trash but it's more readable

  /** Get the top position of the ball */
  get top() {
    return this.position.y - this.radius;
  }

  /** Get the bottom position of the ball */
  get bottom() {
    return this.position.y + this.radius;
  }

  /** Get the left position of the ball */
  get left() {
    return this.position.x - this.radius;
  }

  /** Get the right position of the ball */
  get right() {
    return this.position.x + this.radius;
  }

  // A helper for the velocity
  // These are memoized so we don't have to calculate them every time

  /**
   * Calculate the velocity of the ball in the X and Y axes from the angle and velocity
   * @param {number} angle The angle of the ball (in degrees)
   * @param {number} velocity The velocity of the ball
   * @returns {XandY} The velocity of the ball in the X and Y axes
   */
  private internalVelocityInAxes = memoize((angle: number, velocity: number): XandY => {
    return {
      x: Math.cos(Ball.degreesToRadians(angle)) * velocity,
      y: Math.sin(Ball.degreesToRadians(angle)) * velocity,
    };
  });

  /**
   * The velocity of the ball in the X and Y axes
   * Calculated from the angle and velocity
   */
  get velocityInAxes(): XandY {
    return this.internalVelocityInAxes(this.angle, this.velocity);
  }
}
