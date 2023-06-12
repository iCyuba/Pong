import { find, remove } from "lodash-es";

import { WebSocket } from "ws";

import * as Messages from "@/messages";

import Handler from "@/handlers";
import Invite from "@/invite";
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
    // TODO: This ain't implemented yet cuz there's no sessions

    return this.all;
  }

  /**
   * Send a message to all players
   * @param {string} message A message to send to all players
   * @param {Player[]} players A list of players to send the message to (defaults to all players)
   */
  broadcast(message: any, players: Player[] = this.all) {
    for (const player of players) {
      player.send(message);
    }
  }

  /**
   * Find a player from their WebSocket
   * @param {WebSocket} ws A WebSocket connection
   * @returns {Player | undefined} The player with the WebSocket connection, or undefined if not found
   */
  fromWebSocket(ws: WebSocket): Player | undefined {
    return find(this.all, { ws });
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
   * Add a player to the list of players
   * @param {WebSocket} ws A WebSocket connection
   * @param {string} name The player's username
   * @returns {Player} The player created
   * @throws Error
   */
  addPlayer(ws: WebSocket, name: string): Player {
    // Check if the player is already registered
    if (this.fromWebSocket(ws)) throw new Error("Already registered");

    // Check if the username is already taken
    if (this.fromName(name)) throw new Error("Username is already in use");

    // Add the player to the list of players
    const player = new Player(ws, name);
    this.all.push(player);

    // Send the player registered message to players who aren't in a session
    this.broadcast(Messages.Register(player), this.notInSession);

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

    // Send the player unregistered message to players who aren't in a session
    this.broadcast(Messages.Unregister(player), this.notInSession);

    // Delete any invites the player sent or received
    Invite.deleteForPlayer(this.game, player);

    // TODO: If they're in a session end it

    // Return the player
    return player;
  }

  /**
   * Remove a player by their WebSocket from the list
   * @param {WebSocket} ws The player's WebSocket connection
   * @returns {Player | undefined} The player who was removed or undefined if the WebSocket doesn't belong to anyone
   */
  removeWebSocket(ws: WebSocket): Player | undefined {
    // Find the player to remove
    const player = this.fromWebSocket(ws);

    // If no player was found, simply return
    if (!player) return;

    // Call the remove player method
    return this.removePlayer(player);
  }
}
