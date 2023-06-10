namespace Pong
{
  public class BallServer : Ball
  {
    /// <summary>
    /// Create a new instance of a BallServer
    /// </summary>
    /// <param name="game">The game that the ball is in</param>
    public BallServer(Game game)
      : base(game) { }

    /// <summary>
    /// Move the ball according to its velocity and the time passed
    /// </summary>
    /// <param name="deltaTime">The time passed since the last frame</param>
    public void Move(double deltaTime)
    {
      PosX += VelX * deltaTime;
      PosY += VelY * deltaTime;
    }

    /// <summary>
    /// Set the position of the ball to the middle of the screen
    /// </summary>
    public void SetPosToMiddle(int width, int height)
    {
      PosX = width / 2;
      PosY = height / 2;
    }

    /// <summary>
    /// Randomly start moving the ball in a random direction
    /// </summary>
    public void RandomlyStartMoving(int vel)
    {
      Random r = new();

      int angleInDegrees = r.Next(0, 360);
      double angleInRadians = (angleInDegrees / 180.0) * Math.PI;

      // Math :/
      VelX = Math.Cos(angleInRadians) * vel;
      VelY = Math.Sin(angleInRadians) * vel;
    }

    /// Checks if the ball is colliding with the left or right of the screen. If it is, it returns true
    public bool CheckGoalCollision()
    {
      return Left < 0 || Right > Game.Width;
    }

    public override void Bounce(Box[]? boxes = null)
    {
      base.Bounce(boxes);

      if (CheckGoalCollision())
        VelX *= -1;
    }
  }
}
