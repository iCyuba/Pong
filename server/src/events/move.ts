import { Event, SessionEventHandler } from "@/handlers/event";
import Player from "@/players/player";
import { Velocity } from "@/sessions/paddle";
import Session from "@/sessions/session";

interface MoveEvent extends Event {
  type: "Move";
  position: number;
  velocity: Velocity;
}

export default class MoveHandler extends SessionEventHandler<MoveEvent> {
  static type = "move";

  handleSession(player: Player, session: Session, event: MoveEvent) {
    // Move the player
    session.move(player, event.position, event.velocity);
  }
}
