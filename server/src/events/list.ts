import { WebSocket } from "ws";

import GameEventHandler, { GameEvent } from "@/event";
import { List } from "@/messages";

interface ListEvent extends GameEvent {
  type: "list";
}

export default class ListHandler extends GameEventHandler<ListEvent> {
  static type = "list";

  handle(ws: WebSocket, event: ListEvent) {
    // Log the list event if in development mode
    if (this.wss.dev) console.log(new Date(), "Listing players");

    // Find the player and throw an error if they're not registered
    const player = this.wss.game.getPlayer(ws);
    if (!player) throw new Error("Not registered!");

    // Send them the list of all players
    player.send(List(this.wss.game.getPlayersNotInSession(), player));
  }
}
