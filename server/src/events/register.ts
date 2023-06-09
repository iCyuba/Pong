import { WebSocket } from "ws";

import GameEventHandler, { GameEvent } from "@/event";
import * as Messages from "@/messages";

interface RegistrationEvent extends GameEvent {
  type: "register";
  name: string;
}

export default class RegistrationHandler extends GameEventHandler<RegistrationEvent> {
  static type = "register";

  handle(ws: WebSocket, event: RegistrationEvent) {
    // Log the registration
    console.log(new Date(), "Registering", event.name);

    // Add the player to the game
    const player = this.game.players.addPlayer(ws, event.name);

    // Send them a list of players
    player.send(Messages.List(this.game.players.notInSession, player));
  }
}
