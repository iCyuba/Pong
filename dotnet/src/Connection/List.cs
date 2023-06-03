namespace Pong
{
  partial class Connection
  {
    public class ListEvent : GameEvent
    {
      public List<string> Players { get; set; }

      public ListEvent(List<string> players)
        : base("list")
      {
        Players = players;
      }

      /// <summary>
      /// Deserialize a ListEvent from json
      /// </summary>
      public static new ListEvent Deserialize(string json)
      {
        return Deserialize<ListEvent>(json);
      }
    }

    /// <summary>
    /// Ask the server for a list of players
    /// </summary>
    public async Task List()
    {
      var listEvent = new Dictionary<string, string> { { "type", "list" } };

      await Send(listEvent);
    }
  }
}