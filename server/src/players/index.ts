import { UUID } from "crypto";
import { find, remove } from "lodash-es";

import { WebSocket } from "@/server";

import * as Messages from "@/messages";

import Handler from "@/handlers";
import Player from "@/players/player";

/**
 * Handles players inside a game
 *
 * Responsible for players joining / leaving and whatever else
 */
export default class Players extends Handler {
  /** A list of all the players (please don't mutate this directly..) */
  readonly all: Player[] = [];

  /** A filtered list of all players who are currently not in any session */
  get notInSession(): Player[] {
    return this.all.filter(player => !this.game.sessions.fromPlayer(player));
  }

  /**
   * Send a message to all players
   * @param {string} message A message to send to all players
   */
  broadcast(message: any) {
    Players.broadcast(message, this.all);
  }

  /**
   * Send a message to all players who aren't in a session
   * @param {string} message A message to send to all players
   */
  broadcastNotInSession(message: any) {
    Players.broadcast(message, this.notInSession);
  }

  /**
   * Send a message to a list of players
   * @param {string} message A message to send to all players
   * @param {Player[]} players A list of players to send the message to (defaults to all players)
   */
  static broadcast(message: any, players: Player[]) {
    for (const player of players) {
      player.send(message);
    }
  }

  /**
   * Find a player by their name
   * @param {string} name The name of the player
   * @returns {Player | undefined} The player with the WebSocket connection, or undefined if not found
   */
  fromName(name: string): Player | undefined {
    return find(this.all, { name });
  }

  /**
   * Find a player from their uuid
   * @param {UUID} uuid The uuid of the player
   * @returns {Player | undefined} The player with the uuid, or undefined if not found
   */
  fromUUID(uuid: UUID): Player | undefined {
    return find(this.all, { uuid });
  }

  /**
   * Add a player to the list of players
   * @param {WebSocket} ws A WebSocket connection
   * @param {string} name The player's username
   * @returns {Player} The player created
   * @throws Error
   */
  addPlayer(ws: WebSocket, name: string): Player {
    // Get the uuid of the websocket
    const uuid = ws.getUserData().uuid;

    // Check if the player is already registered
    if (this.fromUUID(uuid)) throw new Error("Already registered");

    // Check if the username is already taken
    if (this.fromName(name)) throw new Error("Username is already in use");

    // Add the player to the list of players
    const player = new Player(ws, name);
    this.all.push(player);

    // Send the player registered message to players who aren't in a session
    this.broadcastNotInSession(Messages.Register(player));

    // Return the player that we just made
    return player;
  }

  /**
   * Remove a player from the list of players
   * @param {Player} player The player to remote
   * @returns {Player | undefined} The player who was removed (undefined if they don't exits ig)
   */
  removePlayer(player: Player): Player | undefined {
    // Remove the player from the list of players
    remove(this.all, player);

    // Delete any invites the player sent or received
    this.game.invites.removePlayer(player);

    // Remove any sessions the player is in
    this.game.sessions.removePlayer(player);

    // Send the player unregistered message to players who aren't in a session
    this.broadcastNotInSession(Messages.Unregister(player));

    // Return the player
    return player;
  }

  /**
   * Remove a player by their uuid from the list
   * @param {UUID} uuid The uuid of the player
   * @returns {Player | undefined} The player who was removed or undefined if the uuid doesn't belong to anyone
   */
  removeUUID(uuid: UUID): Player | undefined {
    // Find the player to remove
    const player = this.fromUUID(uuid);

    // If no player was found, simply return
    if (!player) return;

    // Call the remove player method
    return this.removePlayer(player);
  }
}
