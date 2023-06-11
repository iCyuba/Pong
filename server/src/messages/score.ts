import { SessionPlayer } from "@/sessions/session";

/**
 * A message that is sent when a player scores
 * Sent to the players in the session whenever a player scores
 */
export interface ScoreMessage {
  type: "score";
  you: number;
  opponent: number;
}

/**
 * Update the scores
 * Sent to the players in the session whenever a player scores
 * @param {Record<SessionPlayer, number>} scores The scores for each player
 * @param {SessionPlayer} player The player who this score is for
 * @returns {ScoreMessage} A Register message
 */
export function Score(scores: Record<SessionPlayer, number>, player: SessionPlayer): ScoreMessage {
  const you = scores[player];
  const opponent =
    scores[player === SessionPlayer.Player1 ? SessionPlayer.Player2 : SessionPlayer.Player1];

  return {
    type: "score",
    you,
    opponent,
  };
}

export default Score;
