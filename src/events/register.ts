import { WebSocket } from "ws";

import GameEventHandler, { GameEvent } from "@/event";

interface RegistrationEvent extends GameEvent {
  type: "register";
  name: string;
}

export default class RegistrationHandler extends GameEventHandler<RegistrationEvent> {
  static type = "register";

  handle(ws: WebSocket, event: RegistrationEvent) {
    if (typeof event.name !== "string") {
      throw new Error("Invalid name: " + event.name);
    }

    // Log the registration if in development mode
    if (this.wss.dev) console.log(new Date(), "Registering", event.name);

    // Add the player to the game
    this.wss.game.addPlayer(ws, event.name);
  }
}
