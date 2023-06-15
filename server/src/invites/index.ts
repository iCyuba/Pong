import { filter, find, remove } from "lodash-es";

import * as messages from "@/messages";

import Handler from "@/handlers";
import Invite from "@/invites/invite";
import Player from "@/players/player";

/**
 * Handles invites between players
 */
export default class Invites extends Handler {
  /** A list of all the invites (please don't mutate this directly..) */
  readonly all: Invite[] = [];

  /**
   * Create a new invite from player1 to player2
   * @param {Player} player1 The player who sent the invite
   * @param {Player} player2 The player who received the invite
   * @returns {Invite} The invite that was created
   */
  create(player1: Player, player2: Player): Invite {
    // Check if the players are already in a session
    if (this.game.sessions.fromPlayer(player1) || this.game.sessions.fromPlayer(player2))
      throw new Error("Player is already in a session");

    // Check if the invite already exists and throw an error if it does
    if (this.find(player1, player2)) throw new Error("Invite already exists");

    // Create a new invite
    const invite = new Invite(player1, player2);

    // Add the invite to the list of invites
    this.all.push(invite);

    // Send a message to both players about the invite
    player1.send(messages.Invite(player1, player2));
    player2.send(messages.Invite(player1, player2));

    return invite;
  }

  /**
   * Accept the invite and create a new session
   *
   * Called when the invite is accepted by player2
   * @param {Invite} invite The invite to accept
   */
  accept(invite: Invite) {
    // Remove the invite from the list of invites
    this.delete(invite);

    // Create a new session
    this.game.sessions.create(invite);
  }

  /**
   * Delete the invite
   *
   * Called when either player1 or player2 unregisters
   *
   * Also called when the invite is accepted by player2
   * @param {Invite} invite The invite to delete
   */
  delete(invite: Invite) {
    // Remove the invite from the list of invites
    remove(this.all, invite);
  }

  /**
   * Find a player's sent invites
   * @param {Player} player1 The player to find invites for
   * @returns {Invite[]} A list of invites
   */
  findSent(player1: Player): Invite[] {
    return filter(this.all, { player1 });
  }

  /**
   * Find a player's received invites
   * @param {Player} player2 The player to find invites for
   * @returns {Invite[]} A list of invites
   */
  findReceived(player2: Player): Invite[] {
    return filter(this.all, { player2 });
  }

  /**
   * Find a player's invites (sent and received)
   * @param {Player} player The player to find invites for
   * @returns {Invite[]} A list of invites
   */
  findAll(player: Player): Invite[] {
    return [...this.findSent(player), ...this.findReceived(player)];
  }

  /**
   * Find an exact invite
   * @param {Player} player1 The player who sent the invite
   * @param {Player} player2 The player who received the invite
   * @returns {Invite | undefined} The invite
   */
  findExact(player1: Player, player2: Player): Invite | undefined {
    return find(this.all, { player1, player2 });
  }

  /**
   * Find an invite
   * @param {Player} player1 One of the players
   * @param {Player} player2 One of the players
   * @returns {Invite | undefined} The invite
   */
  find(player1: Player, player2: Player): Invite | undefined {
    // Try to find the invite from player1 to player2
    const p1ToP2 = this.findExact(player1, player2);
    if (p1ToP2) return p1ToP2;

    // Fallback to finding the invite from player2 to player1
    return this.findExact(player2, player1);
  }

  /**
   * Delete all invites for a player
   *
   * Either player1 or player2
   *
   * Called when a player unregisters or joins a session
   * @param {Player} player The player to delete invites for
   */
  removePlayer(player: Player) {
    // Get the invites for the player
    const invites = this.findAll(player);

    // Delete the invites
    invites.forEach(this.delete.bind(this));
  }
}
