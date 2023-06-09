import { RegisteredGameEventHandler } from "@/event";
import Player from "@/players/player";

export default class UnregistrationHandler extends RegisteredGameEventHandler {
  static type = "unregister";

  handleRegistered(player: Player) {
    // Remove the player from the game
    this.game.players.removePlayer(player);

    // Log the unregistration
    console.log(new Date(), "Unregistering", player?.name);
  }
}
