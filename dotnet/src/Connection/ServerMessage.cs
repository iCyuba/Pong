namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// An unused event. Could be used to show a message from the server
    /// </summary>
    public class ServerMessageEvent : GameEvent
    {
      public string Message { get; set; }

      public ServerMessageEvent(string message)
        : base("message")
      {
        Message = message;
      }

      /// <summary>
      /// Deserialize a ServerMessageEvent from json
      /// </summary>
      public static new ServerMessageEvent Deserialize(string json)
      {
        return Deserialize<ServerMessageEvent>(json);
      }
    }
  }
}
