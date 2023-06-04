import { WebSocket } from "ws";

import GameEventHandler, { GameEvent } from "@/event";

interface RegistrationEvent extends GameEvent {
  type: "register";
  name: string;
}

export default class RegistrationHandler extends GameEventHandler<RegistrationEvent> {
  static type = "register";

  handle(ws: WebSocket, event: RegistrationEvent) {
    // Check if the name is valid (not empty and less than 16 characters)
    if (typeof event.name !== "string" || !event.name.length || event.name.length > 16) {
      throw new Error("Invalid name: " + event.name);
    }

    // Log the registration
    console.log(new Date(), "Registering", event.name);

    // Add the player to the game
    this.game.addPlayer(ws, event.name);
  }
}
