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

    /// <summary>
    /// Whether or not the ball was colliding with a paddle in the last frame
    /// </summary>
    private Dictionary<Paddle, bool> lastCollisionForPaddles { get; set; } = new();

    /// <summary>
    /// Checks if the ball is behind the goal of the paddles
    /// </summary>
    public bool CheckGoalCollision()
    {
      // return Right < Game.Offset.X || Left > Game.Width - Game.Offset.X;
      return Right < 0 || Left > Game.Width;
    }

    /// <summary>
    /// Check the collisions for the given paddles
    /// <br/>
    /// Note: In my pong game the paddles are actually behind the playable area. Do with that what you will
    /// </summary>
    /// <param name="paddles">The paddles to check the collisions for</param>
    public bool CheckPaddleCollision(Paddle[] paddles)
    {
      // Check the collisions for each paddle
      foreach (var paddle in paddles)
      {
        // Check if the ball is colliding with the paddle
        bool colliding = CollidesWith(paddle);

        // Get the last collision for the paddle
        bool wasColliding = lastCollisionForPaddles.ContainsKey(paddle)
          ? lastCollisionForPaddles[paddle]
          : false;

        // Remember the collision for the next frame
        lastCollisionForPaddles[paddle] = colliding;

        // If the ball is colliding, return true if it wasn't colliding in the last frame and false if it was
        if (colliding)
          return !wasColliding;

        // Otherwise, continue
      }

      // If the ball isn't colliding with any of the paddles, return false
      return false;
    }

    public override void Bounce()
    {
      // The check for the walls is done in the base class
      base.Bounce();

      // First we want to check if the ball is colliding with a paddle
      // If it is, we want to bounce it and return
      // Note: Since I am passing Game in the constructor, I can access the paddles from the Game property. No need for a parameter here
      if (CheckPaddleCollision(new[] { Game.LeftPaddle, Game.RightPaddle }))
      {
        VelX *= -1;

        return;
      }

      // If the ball didn't collide with a paddle, check if it is behind the goal
      if (CheckGoalCollision())
        // TODO: Change this to a score system
        VelX *= -1;
    }
  }
}
