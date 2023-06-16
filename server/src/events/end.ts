import * as messages from "@/messages";

import { Event, RegisteredEventHandler } from "@/handlers/event";
import Player from "@/players/player";

interface EndEvent extends Event {
  type: "end";
}

// This is intentionally RegisterEventHandler, not SessionEventHandler
// I don't want a player to receive an error if they close an already closed session
export default class EndHandler extends RegisteredEventHandler<EndEvent> {
  static type = "end";

  handleRegistered(player: Player) {
    // Log the End event
    console.log(new Date(), player.name, "ended the session");

    // End the session
    this.game.sessions.removePlayer(player);

    // Send a list of players to the player
    player.send(messages.List(this.game.players.notInSession, player));
  }
}
