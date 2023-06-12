import { memoize } from "lodash-es";
import { TypedEmitter } from "tiny-typed-emitter";

import { SessionPlayer } from "@/sessions/session";

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
  outOfBounds: (player: SessionPlayer) => void;
}

/**
 * A ball in the game
 *
 * Side note: Unlike the rest of my classes, this emits events cuz like.. why not
 *            (it's more appropriate to use events here)
 *
 * The server handles it as a square, but the client sees it as a circle lmao
 */
export default class Ball extends TypedEmitter<BallEvents> {
  // Note: the following properties are initialized in the constructor with the reset method
  //       but typescript doesn't know that so I'm just gonna use ! to tell it that it's not undefined

  /** The position of the ball */
  position!: XandY;

  /** The velocity of the ball */
  velocity!: number;

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
   * @param {number} radius The radius of the ball (defaults to 2) [percentages of the screen]
   */
  constructor(radius: number = 2) {
    // Call the EventEmitter constructor (so we can emit events)
    super();

    this.radius = radius;

    // Reset the ball to the base position and speed with a random angle (no player specified)
    this.reset();
  }

  /**
   * Reset the ball to the middle of the screen with a random angle and speed of 50.
   *
   * Optionally a player can be specified, which makes the ball not go towards that player.
   * This is used when a player scores a goal. The ball will go towards the other player.
   * @param {SessionPlayer} player The player to not go towards
   */
  reset(player?: SessionPlayer) {
    // The starting position of the ball is 50, 50 (in the middle of the screen)
    // On the server the positions are in percentages, but on the client they are in pixels
    // The only downside is that the client will have to be a square (or else the ball will be an oval lmfaoo)
    this.position = { x: 50, y: 50 };

    // The starting velocity of the ball is 50 (percent per second)
    this.velocity = 50;

    // The default minimum and maximum angle for the ball
    let min = 0;
    let max = 360;

    // If a player is specified, make sure the ball doesn't go towards that player
    if (player === SessionPlayer.Player1) {
      // If the player is player 1, make sure the ball doesn't go towards the left
      min = -90;
      max = 89; // In the chance 90 is chosen, the ball will go towards the left (which we don't want)
    } else if (player === SessionPlayer.Player2) {
      // If the player is player 2, make sure the ball doesn't go towards the right
      min = 90;
      max = 269; // In the chance 270 is chosen, the ball will go towards the right (which we don't want)
    }

    // Set a random (but good) angle for the ball
    this.angle = Ball.randomGoodAngle(min, max);
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
    // If the ball is behind the left or right wall, stop the ball
    if (this.right < 0) {
      this.velocity = 0;

      this.emit("outOfBounds", SessionPlayer.Player2);
    } else if (this.left > 100) {
      this.velocity = 0;

      this.emit("outOfBounds", SessionPlayer.Player1);
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
   * Make sure the angle is between 0 and 360
   * @param {number} angle The angle to convert (in degrees)
   */
  static baseAngle(angle: number) {
    // If the angle is less than 0, add 360 to it
    if (angle < 0) angle += 360;

    // Do modulo 360 to make sure the angle is between 0 and 360
    return angle % 360;
  }

  /**
   * Generate a random angle
   * @param {number} minAngle The minimum angle (in degrees) (defaults to 0)
   * @param {number} maxAngle The maximum angle (in degrees) (defaults to 360)
   * @returns {number} The angle generated (in degrees)
   */
  static randomAngle(minAngle: number = 0, maxAngle: number = 360): number {
    // I'm not normalizing the angle here because if I do, the angle will always be 0

    // Make sure the minimum angle is less than the maximum angle
    // If it isn't, swap the values
    let temp: number = minAngle;
    minAngle = Math.min(temp, maxAngle);
    maxAngle = Math.max(temp, maxAngle);

    // Get the difference between the angles
    const angleDifference = Math.abs(maxAngle - minAngle);

    // Generate a random angle in degrees
    const angle = Math.random() * angleDifference + minAngle;

    // Normalize the generated angle
    const normalizedAngle = this.baseAngle(angle);

    return normalizedAngle;
  }

  /**
   * Take an angle and makes sure it's not an annoying angle (like 0, 90, 180, ...) and others close to it
   * This is because the game isn't fun when the ball is going straight only in one direction
   * (please ignore the method name. it's trash ik)
   * @param {number} angle The angle to change (in degrees)
   * @returns {number} The good angle (in degrees)
   */
  static goodAngle(angle: number): number {
    // First, make sure the angle is between 0 and 360
    angle = this.baseAngle(angle);

    // These checks are the same in each quadrant, so I'm doing modulo 90 to make it easier
    const angleInQuadrant = angle % 90;

    // If the angle is less than 10, add 10 to it
    if (angleInQuadrant < 10) return angle + 10;

    // If the angle is greater than 80, subtract 10 from it
    if (angleInQuadrant > 80) return angle - 10;

    // Otherwise, return the angle as is because it's fine
    return angle;
  }

  /**
   * Generate a random angle that is good for the game
   * (yes, this is a separate method. don't @ me)
   * @param {number} minAngle The minimum angle (in degrees) (defaults to 0)
   * @param {number} maxAngle The maximum angle (in degrees) (defaults to 360)
   * @returns {number} The angle generated (in degrees)
   */
  static randomGoodAngle(minAngle: number = 0, maxAngle: number = 360): number {
    // Generate a random angle in degrees
    const angle = Ball.randomAngle(minAngle, maxAngle);

    // Check if the angle is good, and if it isn't, make it a nice angle
    // (i hate the term "good / nice angle". but idk what else to call it)
    // (i'd call it a normalized angle but that's already a thing for something else)
    const goodAngle = Ball.goodAngle(angle);

    return goodAngle;
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
