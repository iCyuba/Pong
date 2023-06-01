using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Pong.Connection
{
  partial class Connection
  {
    static private JsonSerializerOptions JsonSerializerOptions =
      new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private class GameEvent
    {
      public string Type { get; set; }

      public GameEvent(string type)
      {
        Type = type;
      }

      /// <summary>
      /// Deserialize a GameEvent (or subclass) from json
      /// </summary>
      public static T Deserialize<T>(string json)
        where T : GameEvent
      {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions)
          ?? throw new Exception("Failed to parse event");
      }

      /// <summary>
      /// Deserialize a GameEvent from json
      /// </summary>
      public static GameEvent Deserialize(string json)
      {
        return Deserialize<GameEvent>(json);
      }
    }

    /// <summary>
    /// Message handler, this will be called whenever a message is received
    /// </summary>
    private void OnMessage(object? _, string message)
    {
      // Log the message
      Console.WriteLine($"< {message}");

      // Parse the response
      GameEvent gameEvent;
      try
      {
        gameEvent = GameEvent.Deserialize(message);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return;
      }

      // Call the appropriate handler
      // I'm fairly certain I have to deserialize the event again because only the base class is deserialized (but idk. shouldn't be too much of a performance hit)
      switch (gameEvent.Type)
      {
        case "register":
          OnRegisterHandler?.Invoke(this, RegisterEvent.Deserialize(message));
          break;
        case "list":
          break;
        case "start":
          break;
        case "update":
          break;
        case "end":
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// The message loop, this will run forever until the websocket is closed
    /// </summary>
    private async Task MessageLoop()
    {
      while (WS.State == WebSocketState.Open)
      {
        // Wait for a message
        byte[] response = new byte[1024];
        WebSocketReceiveResult result = await WS.ReceiveAsync(response, CancellationToken.None);

        // Convert the response to a string
        string encoded = Encoding.UTF8.GetString(response, 0, result.Count);

        // Call the message handler(s)
        OnMessageHandler?.Invoke(this, encoded);
      }

      // Call the close handler if it exists (inspired by the forms designer [kinda])
      // This is wrapped in a try catch because the handler might throw an exception (and I ain't dealing with that)
      try
      {
        OnCloseHandler?.Invoke(this, EventArgs.Empty);
      }
      catch { }
    }

    /// <summary>
    /// A helper method to send a message to the server
    /// </summary>
    public async Task Send(object data)
    {
      if (WS.State != WebSocketState.Open)
        throw new Exception("The websocket is not open");

      string serialized = JsonSerializer.Serialize(data);

      // Log the message
      Console.WriteLine($"> {serialized}");

      byte[] bytes = Encoding.UTF8.GetBytes(serialized);
      await WS.SendAsync(bytes, WebSocketMessageType.Text, true, default);
    }
  }
}
