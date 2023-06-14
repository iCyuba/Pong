namespace Pong
{
  public class Paddle : RenderedBox
  {
    /// <summary>
    /// The width used for all paddles in percentage of the screen width
    /// </summary>
    public const double BaseWidth = 4;

    /// <summary>
    /// The height used for all paddles in percentage of the screen height
    /// </summary>
    public const double BaseHeight = 20;

    /// <summary>
    /// The base speed of the paddle in percentage of the screen height per second
    /// </summary>
    public const double BaseSpeed = 80;

    /// <summary>
    /// The game the paddle belongs to
    /// </summary>
    private Game Game { get; set; }

    /// <summary>
    /// The velocity of the paddle on the Y axis.
    /// </summary>
    public double VelY { get; set; }

    /// <summary>
    /// Create a new instance of a Paddle in the middle of the screen and scale it to an appropriate size
    /// (It's created at X=0 intentionally.. I use the setters <see cref="Box.Left"/> and <see cref="Box.Right"/> to set the position)
    /// </summary>
    /// <param name="game">The game that the paddle is in</param>
    public Paddle(Game game)
      : base(0, 50, BaseWidth, BaseHeight, Brushes.DeepPink, game.Scale, game.Offset)
    {
      Game = game;
    }

    /// <summary>
    /// Move the paddle by the specified amount of time
    /// </summary>
    public void Move(double deltaTime)
    {
      // Update the position of the paddle
      PosY += VelY * deltaTime;

      // If the paddle is outside of the screen, move it back in
      // Yes, I could return (like I did before) but then I would have to manually calculate the Top and Bottom.. andddd I'm lazy
      if (Top < 0)
        Top = 0;
      else if (Bottom > 100)
        Bottom = 100;
    }

    /// <summary>
    /// Start moving the paddle up / stop moving the paddle down
    /// </summary>
    public void MoveUp()
    {
      // Return if the paddle is already moving up
      if (VelY < 0)
        return;

      VelY -= BaseSpeed;
    }

    /// <summary>
    /// Start moving the paddle down / stop moving the paddle up
    /// </summary>
    public void MoveDown()
    {
      // Return if the paddle is already moving down
      if (VelY > 0)
        return;

      VelY += BaseSpeed;
    }

    /// <summary>
    /// Handle the movement for a bot player
    /// </summary>
    /// <param name="ball">The ball to copy the velocity from</param>
    /// <param name="type">The type of game that is being played</param>
    public void BotMovement(Ball ball, GameServer.GameType type)
    {
      // Copy the velocity of the ball onto the right paddle and apply the multiplier
      // The multiplier is taken from the GameType enum. It's in percentages so it needs to be divided by 100
      VelY = ball.VelY * ((int)type / 100.0);

      // If the the ball is moving down and the top position of the paddle is below the top position of the ball, move the paddle up
      if (ball.VelY > 0 && Top > ball.Top)
        VelY *= -1;
      // If the the ball is moving up and the bottom position of the paddle is above the bottom position of the ball, move the paddle down
      else if (ball.VelY < 0 && Bottom < ball.Bottom)
        VelY *= -1;
    }
  }
}
