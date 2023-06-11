namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when a session is created
    /// <br/>
    /// Sent to all players who are not in a session (but including the two players in the new session)
    /// </summary>
    public class CreateEvent : GameEvent
    {
      public string Player1 { get; set; }
      public string Player2 { get; set; }

      public CreateEvent(string player1, string player2)
        : base("create")
      {
        Player1 = player1;
        Player2 = player2;
      }

      /// <summary>
      /// Deserialize a CreateEvent from json
      /// </summary>
      public static new CreateEvent Deserialize(string json)
      {
        return Deserialize<CreateEvent>(json);
      }
    }
  }
}
