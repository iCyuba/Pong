import { WebSocket } from "ws";

import { List, Register } from "@/messages";
import Player from "@/player";
import Server from "@/server";

/**
 * A simple helper type for x and y values (used for position and velocity)
 * @property {number} x The x value
 * @property {number} y The y value
 */
interface XandY {
  x: number;
  y: number;
}

/**
 * A session between two players
 * @property {@link Player} player1 The first player
 * @property {@link Player} player2 The second player
 *
 * @property {@link XandY} position The position of the ball
 * @property {@link XandY} velocity The velocity of the ball
 */
export interface Session {
  player1: Player;
  player2: Player;

  position: XandY;
  velocity: XandY;
}

/**
 * A class that handles the game, players, and sessions
 */
export default class Game {
  /** A list of all the players in the game */
  players: Player[];
  /** A record of all the active invites */
  invites: Record<string, string>;
  /** A list of all the active sessions */
  sessions: Session[];

  /** The WebSocket server */
  private wss: Server;

  /**
   * Create a new Game instance
   * @param {Server} wss The WebSocket server
   */
  constructor(wss: Server) {
    this.players = [];
    this.invites = {};
    this.sessions = [];

    this.wss = wss;
  }

  /**
   * Send a message to all players
   * @param {string} message A message to send to all players
   * @param {Player[]} players A list of players to send the message to (defaults to all players)
   */
  broadcast(message: any, players: Player[] = this.players) {
    for (const player of players) {
      player.send(message);
    }
  }

  /**
   * Add a player to the list of players
   * @param {WebSocket} ws A WebSocket connection
   * @param {string} name The player's username
   * @throws Error
   */
  addPlayer(ws: WebSocket, name: string): void {
    // Check if the player is already registered
    if (Object.values(this.players).find(player => player.ws === ws))
      throw new Error("Already registered");

    // Check if the username is already taken
    if (this.players.find(player => player.name === name))
      throw new Error("Username already taken");

    // Add the player to the list of players
    const player = new Player(ws, name);
    this.players.push(player);

    // Send the player registered message to players who aren't in a session
    this.broadcast(Register(player), this.getPlayersNotInSession());

    // Send the player a list of all the players who aren't in a session
    player.send(List(this.getPlayersNotInSession(), player));
  }

  /**
   * Remove a player from the list of players
   * @param {WebSocket} ws A WebSocket connection
   */
  removePlayer(ws: WebSocket): void {
    // Remove the player from the list of players
    this.players = this.players.filter(player => player.ws !== ws);

    // End the player's session
    const session = this.getPlayerSession(ws);

    if (session) return; // TODO: End the session
  }

  // My convenience methods:

  /**
   * Find a player from their websocket
   * @param {WebSocket} ws The player's WebSocket connection
   * @returns {Player | null}
   */
  getPlayer(ws: WebSocket): Player | undefined {
    return this.players.find(player => player.ws === ws);
  }

  /**
   * Find player's session
   * @param {WebSocket} ws A WebSocket connection
   * @returns {Session} The player's session
   */
  getPlayerSession(ws: WebSocket): Session | undefined {
    return this.sessions.find(session => session.player1.ws === ws || session.player2.ws === ws);
  }

  /**
   * Check if a player is in a session
   * @param {WebSocket} ws A WebSocket connection
   * @returns {boolean} Whether the player is in a session
   */
  isPlayerInSession(ws: WebSocket): boolean {
    return this.getPlayerSession(ws) !== undefined;
  }

  /**
   * Get all players who aren't in a session
   * @returns {Player[]} A list of players who aren't in a session
   */
  getPlayersNotInSession(): Player[] {
    return this.players.filter(player => !this.isPlayerInSession(player.ws));
  }
}
