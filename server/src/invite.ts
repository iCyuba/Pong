import { filter, find, remove } from "lodash-es";

import Game from "@/game";
import * as messages from "@/messages";
import Player from "@/player";

/**
 * Represents an invite from a player to another player.
 */
export default class Invite {
  /** The player that sent the invite */
  player1: Player;
  /** The player that the invite was sent to */
  player2: Player;

  /** The game that the invite is in */
  game: Game;

  /**
   * Create a new invite
   * @param {Game} game The game that the invite is in
   * @param {Player} player1 The player that sent the invite
   * @param {Player} player2 The player that received the invite
   */
  constructor(game: Game, player1: Player, player2: Player) {
    this.player1 = player1;
    this.player2 = player2;

    this.game = game;

    // Check if the players are already in a session
    if (game.sessions.find(session => session.player1 === player1 || session.player2 === player1))
      throw new Error("Player 1 is in a session");
    if (game.sessions.find(session => session.player1 === player2 || session.player2 === player2))
      throw new Error("Player 2 is in a session");

    // Check if the invite already exists and throw an error if it does
    if (Invite.findExact(game, player1, player2)) throw new Error("Invite already exists");

    // Add the invite to the list of invites
    game.invites.push(this);

    // Send a message to both players about the invite
    player1.send(messages.Invite(player1, player2));
    player2.send(messages.Invite(player1, player2));
  }

  /**
   * Accept the invite and create a new session
   * Called when the invite is accepted by player2
   */
  accept() {
    // Remove the invite from the list of invites
    this.delete();

    // Inform everyone that a session has been created between the two players
    // TODO: Move this to the session class
    this.game.broadcast(
      messages.Create(this.player1, this.player2),
      this.game.getPlayersNotInSession()
    );

    // Create a new session
    // TODO: Create a new session
  }

  /**
   * Delete the invite
   * Called when either player1 or player2 unregisters
   * Also called when the invite is accepted by player2
   */
  delete() {
    // Remove the invite from the list of invites
    remove(this.game.invites, this);
  }

  /**
   * Find a player's sent invites
   * @param {Game} game The game to find invites in
   * @param {Player} player1 The player to find invites for
   * @returns {Invite[]} A list of invites
   * @static
   */
  static findSent(game: Game, player1: Player): Invite[] {
    return filter(game.invites, { player1 });
  }

  /**
   * Find a player's received invites
   * @param {Game} game The game to find invites in
   * @param {Player} player2 The player to find invites for
   * @returns {Invite[]} A list of invites
   * @static
   */
  static findReceived(game: Game, player2: Player): Invite[] {
    return filter(game.invites, { player2 });
  }

  /**
   * Find a player's invites (sent and received)
   * @param {Game} game The game to find invites in
   * @param {Player} player The player to find invites for
   * @returns {Invite[]} A list of invites
   * @static
   */
  static findAll(game: Game, player: Player): Invite[] {
    return [...this.findSent(game, player), ...this.findReceived(game, player)];
  }

  /**
   * Find exact invite
   * @param {Game} game The game to find invites in
   * @param {Player} player1 The player that sent the invite
   * @param {Player} player2 The player that received the invite
   * @returns {Invite | undefined} The invite
   * @static
   */
  static findExact(game: Game, player1: Player, player2: Player): Invite | undefined {
    // Try to find the invite from player1 to player2
    const p1ToP2 = find(game.invites, { player1, player2 });
    if (p1ToP2) return p1ToP2;

    // Fallback to finding the invite from player2 to player1
    return find(game.invites, { player1: player2, player2: player1 });
  }

  /**
   * Delete all invites for a player
   * Either player1 or player2
   * Called when a player unregisters
   * @param {Game} game The game to delete invites from
   * @param {Player} player The player to delete invites for
   * @static
   */
  static deleteForPlayer(game: Game, player: Player) {
    // Get the invites for the player
    const invites = this.findAll(game, player);

    // Delete the invites
    invites.forEach(invite => invite.delete());
  }
}
