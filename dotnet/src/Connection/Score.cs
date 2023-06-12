namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message sent when a player scores
    /// </summary>
    public class ScoreEvent : GameEvent
    {
      public int You { get; set; }
      public int Opponent { get; set; }

      public ScoreEvent(int you, int opponent)
        : base("score")
      {
        You = you;
        Opponent = opponent;
      }

      /// <summary>
      /// Deserialize a ScoreEvent from json
      /// </summary>
      public static new ScoreEvent Deserialize(string json)
      {
        return Deserialize<ScoreEvent>(json);
      }
    }
  }
}
