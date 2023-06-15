namespace Pong
{
  partial class Connection
  {
    public class MoveEvent : GameEvent
    {
      public double Position { get; set; }
      public double Velocity { get; set; }

      public MoveEvent(double position, double velocity)
        : base("move")
      {
        Position = position;
        Velocity = velocity;
      }

      /// <summary>
      /// Deserialize a MoveEvent from json
      /// </summary>
      public static new MoveEvent Deserialize(string json)
      {
        return Deserialize<MoveEvent>(json);
      }
    }

    /// <summary>
    /// Send a move event to the server
    /// </summary>
    /// <param name="paddle">The paddle</param>
    public async Task Move(PaddleClientLocal paddle)
    {
      var MoveEvent = new Dictionary<string, object>
      {
        { "type", "move" },
        { "position", paddle.PosY },
        { "velocity", paddle.VelY }
      };

      await Send(MoveEvent);
    }
  }
}
