import { Event, RegisteredEventHandler } from "@/handlers/event";
import Player from "@/players/player";

interface UnregisterEvent extends Event {
  type: "unregister";
}

export default class UnregistrationHandler extends RegisteredEventHandler<UnregisterEvent> {
  static type = "unregister";

  handleRegistered(player: Player) {
    // Remove the player from the game
    this.game.players.removePlayer(player);

    // Log the unregistration
    console.log(new Date(), "Unregistering", player.name);
  }
}
