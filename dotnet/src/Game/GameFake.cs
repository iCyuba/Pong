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

      // Set the velocity of the ball to a static value (idk it just looks nice)
      Ball.VelX = 35;
      Ball.VelY = 15;
    }

    public override void Move(double deltaTime)
    {
      // Call the base method
      base.Move(deltaTime);

      // Run the bot method on the left paddle (right is called in the base method)
      LeftPaddle.BotMovement(Ball, GameType.BotImpossible);
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
