namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when a player leaves a session
    /// <br/>
    /// Sent to all players who aren't in a session
    /// </summary>
    public class EndEvent : GameEvent
    {
      public string Player1 { get; set; }
      public string Player2 { get; set; }

      public EndEvent(string player1, string player2)
        : base("end")
      {
        Player1 = player1;
        Player2 = player2;
      }

      /// <summary>
      /// Deserialize an EndEvent from json
      /// </summary>
      public static new EndEvent Deserialize(string json)
      {
        return Deserialize<EndEvent>(json);
      }
    }
  }
}
