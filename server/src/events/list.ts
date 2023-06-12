import * as Messages from "@/messages";

import { Event, RegisteredEventHandler } from "@/handlers/event";
import Player from "@/players/player";

interface ListEvent extends Event {
  type: "list";
}

export default class ListHandler extends RegisteredEventHandler<ListEvent> {
  static type = "list";

  handleRegistered(player: Player) {
    // Log the list event
    console.log(new Date(), "Listing players", player.name);

    // Send them the list of all players
    player.send(Messages.List(this.game.players.notInSession, player));
  }
}
