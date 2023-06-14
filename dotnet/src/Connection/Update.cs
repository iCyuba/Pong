namespace Pong
{
  partial class Connection
  {
    /// <summary>
    /// A message that is sent when the ball is updated in a session
    /// <br/>
    /// Sent to both players in the session
    /// </summary>
    public class UpdateEvent : GameEvent
    {
      public double PosX { get; set; }
      public double PosY { get; set; }
      public double VelX { get; set; }
      public double VelY { get; set; }

      public UpdateEvent(double posX, double posY, double velX, double velY)
        : base("update")
      {
        PosX = posX;
        PosY = posY;
        VelX = velX;
        VelY = velY;
      }

      /// <summary>
      /// Deserialize an UpdateEvent from json
      /// </summary>
      public static new UpdateEvent Deserialize(string json)
      {
        return Deserialize<UpdateEvent>(json);
      }
    }
  }
}
