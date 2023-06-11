namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when a player invites another player to a session
    /// <br/>
    /// Sent to both players who are in the invite
    /// </summary>
    public class InviteEvent : GameEvent
    {
      public string By { get; set; }
      public string To { get; set; }

      public InviteEvent(string by, string to)
        : base("invite")
      {
        By = by;
        To = to;
      }

      /// <summary>
      /// Deserialize an InviteEvent from json
      /// </summary>
      public static new InviteEvent Deserialize(string json)
      {
        return Deserialize<InviteEvent>(json);
      }
    }

    /// <summary>
    /// Invite another player to a game
    /// </summary>
    /// <param name="name">The name of the player to invite</param>
    public async Task Invite(string name)
    {
      var InviteEvent = new Dictionary<string, string> { { "type", "invite" }, { "name", name } };

      await Send(InviteEvent);
    }
  }
}
