namespace Pong.Connection
{
  partial class Connection
  {
    private class RegisterEvent : GameEvent
    {
      public string Name { get; set; }

      public RegisterEvent(string name) : base("register")
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
      // Check if this is another player registering or if it's us
      if (registerEvent.Name == Name)
      {
        // User has successfully registered so set registered to true
        Registered = true;
      }
      else
      {
        throw new NotImplementedException("Other players registering is not implemented yet");
      }
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