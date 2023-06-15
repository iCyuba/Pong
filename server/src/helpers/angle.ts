/**
 * A helper class for angles
 *
 * Only static methods
 */
export default class Angle {
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
    const angle = Angle.randomAngle(minAngle, maxAngle);

    // Check if the angle is good, and if it isn't, make it a nice angle
    // (i hate the term "good / nice angle". but idk what else to call it)
    // (i'd call it a normalized angle but that's already a thing for something else)
    const goodAngle = Angle.goodAngle(angle);

    return goodAngle;
  }
}
