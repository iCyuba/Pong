namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when a player wins in session
    /// <br/>
    /// Sent to both players in the session
    /// </summary>
    public class WinEvent : GameEvent
    {
      public string Player { get; set; }

      public WinEvent(string player)
        : base("win")
      {
        Player = player;
      }

      /// <summary>
      /// Deserialize a WinEvent from json
      /// </summary>
      public static new WinEvent Deserialize(string json)
      {
        return Deserialize<WinEvent>(json);
      }
    }
  }
}
