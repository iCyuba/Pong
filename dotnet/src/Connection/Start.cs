namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when the session starts
    /// <br/>
    /// Note: A session doesn't start when it's created, it starts when both players are ready (moved for the first time).
    ///       It also starts 0.5 seconds after the ball goes out of bounds (to give the players time to move their paddles)
    /// </summary>

    public class StartEvent : GameEvent
    {
      public double VelX { get; set; }
      public double VelY { get; set; }
      public long Timestamp { get; set; }

      public StartEvent(double velX, double velY, long timestamp)
        : base("start")
      {
        VelX = velX;
        VelY = velY;
        Timestamp = timestamp;
      }

      /// <summary>
      /// Deserialize a StartEvent from json
      /// </summary>
      public static new StartEvent Deserialize(string json)
      {
        return Deserialize<StartEvent>(json);
      }
    }
  }
}
