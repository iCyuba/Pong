import { find } from "lodash-es";

import { GameEvent, RegisteredGameEventHandler } from "@/event";
import Invite from "@/invite";
import Player from "@/player";

interface InviteEvent extends GameEvent {
  type: "invite";
  name: string;
}

export default class InviteHandler extends RegisteredGameEventHandler<InviteEvent> {
  static type = "invite";

  handleRegistered(player: Player, event: InviteEvent) {
    const player2 = this.game.getPlayerByName(event.name);

    if (!player2) throw new Error("Player not found: " + event.name);

    // Log the invite event
    console.log(new Date(), player.name, "Inviting", player2.name);

    // Check if this is a response to an invite, and if so, accept the invite and create a session
    const responseTo = find(this.game.invites, { player1: player2, player2: player });
    if (responseTo) return responseTo.accept();

    // Create the invite
    new Invite(this.game, player, player2);
  }
}
