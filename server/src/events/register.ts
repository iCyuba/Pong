import { WebSocket } from "@/server";

import * as Messages from "@/messages";

import EventHandler, { Event } from "@/handlers/event";

interface RegistrationEvent extends Event {
  type: "register";
  name: string;
}

export default class RegistrationHandler extends EventHandler<RegistrationEvent> {
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
