import { WebSocket } from "@/server";

/**
 * A class that represents a player in the game
 */
export default class Player {
  /** The websocket connection of the player */
  private readonly ws: WebSocket;

  /** The UUID of the connection */
  readonly uuid: string;

  /** The name of the player */
  readonly name: string;

  /**
   * Create a new Player instance
   * Should only be called by the Players class (when a player registers)
   * @param {WebSocket} ws The WebSocket connection of the player
   * @param {string} name The name of the player
   */
  constructor(ws: WebSocket, name: string) {
    // Validate the name
    Player.validateName(name);

    this.ws = ws;
    this.uuid = ws.getUserData().uuid;
    this.name = name;
  }

  /**
   * Send a message to the player
   *
   * (Just a wrapper around WebSocket.send that stringifies the message)
   * @param {any} message A message to send to the player
   */
  send(message: any) {
    this.ws.send(JSON.stringify(message));
  }

  /**
   * Checks whether a name is valid
   *
   * For a method that doesn't throw see: {@link safeValidateName}
   *
   * The following must all be true:
   * - Must be a string (obv)
   * - Must not be empty
   * - Not longer than 16 characters
   *
   * @param name The name to check
   * @throws When the name is invalid
   */
  static validateName(name: string) {
    if (typeof name !== "string") throw new Error(`A name must be a string (got ${typeof name})`);

    if (name.length == 0) throw new Error("A name must not be empty");
    else if (name.length > 16) throw new Error("A name must not be longer than 16 characters");
  }

  /**
   * A version of the {@link validateName} function that returns a boolean instead of throwing an error
   *
   * Not used cuz I want the error messages. but yk.. might be of use sometime
   *
   * A name is valid when all of the following are true after trimming:
   * - Must be a string (obv)
   * - Must not be empty
   * - Not longer than 16 characters
   * @param name The name to check
   * @returns {boolean} Whether the name is valid
   */
  static safeValidateName(name: string): boolean {
    try {
      this.validateName(name);

      return true;
    } catch (_) {
      return false;
    }
  }
}
