namespace Pong
{
  partial class Connection
  {
    public class UnregisterEvent : GameEvent
    {
      public string Name { get; set; }

      public UnregisterEvent(string name)
        : base("unregister")
      {
        Name = name;
      }

      /// <summary>
      /// Deserialize an UnregisterEvent from json
      /// </summary>
      public static new UnregisterEvent Deserialize(string json)
      {
        return Deserialize<UnregisterEvent>(json);
      }
    }

    /// <summary>
    /// Unregister with the server
    /// </summary>
    public async Task Unregister()
    {
      var UnregisterEvent = new Dictionary<string, string> { { "type", "unregister" } };

      await Send(UnregisterEvent);
    }
  }
}
