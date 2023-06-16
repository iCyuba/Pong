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
      public double Angle { get; set; }
      public int Velocity { get; set; }

      public UpdateEvent(double posX, double posY, double angle, int velocity)
        : base("update")
      {
        PosX = posX;
        PosY = posY;
        Angle = angle;
        Velocity = velocity;
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
