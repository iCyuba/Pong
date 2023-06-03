import { RegisteredGameEventHandler } from "@/event";
import { List } from "@/messages";
import Player from "@/player";

export default class ListHandler extends RegisteredGameEventHandler {
  static type = "list";

  handleRegistered(player: Player) {
    // Log the list event
    console.log(new Date(), "Listing players", player.name);

    // Send them the list of all players
    player.send(List(this.wss.game.getPlayersNotInSession(), player));
  }
}
