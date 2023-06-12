import { SessionGameEventHandler } from "@/event";
import Player from "@/players/player";
import Session from "@/sessions/session";

export default class ReadyHandler extends SessionGameEventHandler {
  static type = "ready";

  handleSession(player: Player, session: Session) {
    // Log the Ready event
    console.log(new Date(), player.name, "is ready");

    // Set the player as ready
    session.ready(player);
  }
}
