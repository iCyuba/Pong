import { WebSocket } from "ws";

/**
 * A player in the game
 * @property {WebSocket} socket The WebSocket connection of the player
 * @property {string} name The name of the player
 * @property {Session} session The session that the player is in (Ommited in SessionPlayer)
 * @property {number} position The position of the player in the session
 * @property {number} score The score of the player in the session
 */

/**
 * A class that represents a player in the game
 */
export default class Player {
  /** The websocket connection of the player */
  readonly ws: WebSocket;
  /** The name of the player */
  readonly name: string;

  /**
   * Create a new Player instance
   * The position and score are undefined by default
   * @param {WebSocket} ws The WebSocket connection of the player
   * @param {string} name The name of the player
   */
  constructor(ws: WebSocket, name: string) {
    this.ws = ws;
    this.name = name;
  }

  /**
   * Send a message to the player
   * (Just a wrapper around WebSocket.send that stringifies the message)
   * @param {any} message A message to send to the player
   */
  send(message: any) {
    this.ws.send(JSON.stringify(message));
  }
}
