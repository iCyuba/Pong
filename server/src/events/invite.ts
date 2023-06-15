import { Event, RegisteredEventHandler } from "@/handlers/event";
import Player from "@/players/player";

interface InviteEvent extends Event {
  type: "invite";
  name: string;
}

export default class InviteHandler extends RegisteredEventHandler<InviteEvent> {
  static type = "invite";

  handleRegistered(player: Player, event: InviteEvent) {
    const player2 = this.game.players.fromName(event.name);

    if (!player2) throw new Error("Player not found: " + event.name);

    // Log the invite event
    console.log(new Date(), player.name, "Inviting", player2.name);

    // Check if this is a response to an invite, and if so, accept the invite and create a session
    const responseTo = this.game.invites.findExact(player2, player);
    if (responseTo) return this.game.invites.accept(responseTo);

    // Create the invite
    this.game.invites.create(player, player2);
  }
}
