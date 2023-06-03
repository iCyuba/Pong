namespace Pong
{
  partial class Connection
  {
    public class RegisterEvent : GameEvent
    {
      public string Name { get; set; }

      public RegisterEvent(string name)
        : base("register")
      {
        Name = name;
      }

      /// <summary>
      /// Deserialize a RegisterEvent from json
      /// </summary>
      public static new RegisterEvent Deserialize(string json)
      {
        return Deserialize<RegisterEvent>(json);
      }
    }

    /// <summary>
    /// Event handler for when a register event is received
    /// </summary>
    private void OnRegister(object? _, RegisterEvent registerEvent)
    {
      // Ignore the event if it's for another player
      if (registerEvent.Name != Name)
        return;

      // If the event is for this player, set registered to true
      Registered = true;
    }

    /// <summary>
    /// Register with the server
    /// </summary>
    public async Task Register(string name)
    {
      // Save the name for later and set registered to false (until a response is received)
      Name = name;
      Registered = false;

      var registerEvent = new Dictionary<string, string>
      {
        { "type", "register" },
        { "name", name }
      };

      await Send(registerEvent);
    }
  }
}
