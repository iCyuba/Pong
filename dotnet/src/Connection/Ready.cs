namespace Pong
{
  partial class Connection
  {
    public class ReadyEvent : GameEvent
    {
      public string Name { get; set; }

      public ReadyEvent(string name)
        : base("ready")
      {
        Name = name;
      }

      /// <summary>
      /// Deserialize a ReadyEvent from json
      /// </summary>
      public static new ReadyEvent Deserialize(string json)
      {
        return Deserialize<ReadyEvent>(json);
      }
    }

    /// <summary>
    /// Send a ready event so the game can start
    /// </summary>
    public async Task Ready()
    {
      var ReadyEvent = new Dictionary<string, string> { { "type", "ready" } };

      await Send(ReadyEvent);
    }
  }
}
