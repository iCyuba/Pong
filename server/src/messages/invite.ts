import Player from "@/player";

export interface InviteMessage {
  type: "invite";
  by: string;
  to: string;
}

/**
 * Invite a player to a session
 * Sent to both players who are in the invite
 * @param {Player} by The player who sent the invite
 * @param {Player} to The player who received the invite
 * @returns {InviteMessage} An Invite message
 */
function Invite(by: Player, to: Player): InviteMessage {
  return {
    type: "invite",
    by: by.name,
    to: to.name,
  };
}

export default Invite;
