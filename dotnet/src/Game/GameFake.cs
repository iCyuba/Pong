namespace Pong
{
  public class GameFake : GameServer
  {
    /// <summary>
    /// Create a new instance of a GameFake with the specified width and height
    /// <br/>
    /// This is used as a background game in the main menu. It's not controllable and it's just there to look nice
    /// </summary>
    /// <param name="width">The width of the game</param>
    /// <param name="height">The height of the game</param>
    public GameFake(int width, int height)
      : base(width, height, GameType.BotImpossible)
    {
      // Start the game immediately
      Start();

      // Set the angle of the ball to 35degrees cuz it looks nice
      BallInstance.Angle = 35;
      BallInstance.Velocity = Ball.BaseVelocity;
    }

    public override void Update(double deltaTime)
    {
      // Call the base method
      base.Update(deltaTime);

      // Run the bot method on the left paddle (right is called in the base method)
      LeftPaddle.BotMovement(BallInstance, GameType.BotImpossible);
    }

    /// <summary>
    /// This is called on keydown so we ignore it
    /// </summary>
    public override void KeyDown(Keys key) { }

    /// <summary>
    /// This is called on keyup so we ignore it
    /// </summary>
    public override void KeyUp(Keys key) { }
  }
}
