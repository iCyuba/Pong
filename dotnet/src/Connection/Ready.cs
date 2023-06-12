namespace Pong
{
  partial class Connection
  {
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
