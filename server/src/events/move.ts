import { Event, SessionEventHandler } from "@/handlers/event";
import Player from "@/players/player";
import { Speed } from "@/sessions/paddle";
import Session from "@/sessions/session";

interface MoveEvent extends Event {
  type: "Move";
  speed: Speed;
}

export default class MoveHandler extends SessionEventHandler<MoveEvent> {
  static type = "move";

  handleSession(player: Player, session: Session, event: MoveEvent) {
    // Move the player
    session.move(player, event.speed);
  }
}
