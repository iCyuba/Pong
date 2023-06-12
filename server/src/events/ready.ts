import { Event, SessionEventHandler } from "@/handlers/event";
import Player from "@/players/player";
import Session from "@/sessions/session";

interface ReadyEvent extends Event {
  type: "ready";
}

export default class ReadyHandler extends SessionEventHandler<ReadyEvent> {
  static type = "ready";

  handleSession(player: Player, session: Session) {
    // Log the Ready event
    console.log(new Date(), player.name, "is ready");

    // Set the player as ready
    session.ready(player);
  }
}
