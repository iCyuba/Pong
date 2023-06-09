import { RegisteredGameEventHandler } from "@/event";
import * as Messages from "@/messages";
import Player from "@/players/player";

export default class ListHandler extends RegisteredGameEventHandler {
  static type = "list";

  handleRegistered(player: Player) {
    // Log the list event
    console.log(new Date(), "Listing players", player.name);

    // Send them the list of all players
    player.send(Messages.List(this.game.players.notInSession, player));
  }
}
