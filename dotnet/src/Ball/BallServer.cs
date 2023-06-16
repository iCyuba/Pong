namespace Pong
{
  public class BallServer : Ball
  {
    /// <summary>
    /// The side in which the ball went out of bounds
    /// </summary>
    public enum Side
    {
      Left,
      Right,
    }

    /// <summary>
    /// Event for when a player scores a goal
    /// </summary>
    public EventHandler<Side>? OnOutOfBounds { get; set; }

    /// <summary>
    /// The GameServer this instance of BallServer is in
    /// <br/>
    /// Used for collision detection with the paddles
    /// </summary>
    private GameServer Game { get; set; }

    /// <summary>
    /// Create a new instance of a BallServer
    /// </summary>
    /// <param name="game">The game that the ball is in</param>
    public BallServer(GameServer game)
      : base(game.Scale, game.Offset)
    {
      Game = game;
    }

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
    /// Randomly start moving the ball in some direction
    /// </summary>
    public void RandomlyStartMoving(Side? side = null)
    {
      // Randomly choose a side if one wasn't given
      if (side == null)
        side = new Random().Next(2) == 0 ? Side.Left : Side.Right;

      // Get the angle
      int angle = RandomAngle();

      // Normalize the angle so it's not annoying to work with
      // On the server a named these: "good angles". do not ask....
      angle = NormalizeAngle(angle);

      // Add or subtract 90 degrees depending on the side
      angle += side == Side.Left ? -90 : 90;

      // Make sure the angle is positive
      angle += 360; // don't @ me for this
      angle %= 360; // it's just 2 lines..

      // Set the angle
      Angle = angle;

      // Set the velocity to the base velocity
      Velocity = BaseVelocity;
    }

    /// <summary>
    /// Whether or not the ball was colliding with a paddle in the last frame
    /// </summary>
    private Dictionary<PaddleServer, bool> lastCollisionForPaddles { get; set; } = new();

    /// <summary>
    /// Checks if the ball is behind the goal of the paddles
    /// </summary>
    public Side? CheckGoalCollision()
    {
      if (Right < 0)
        return Side.Left;
      else if (Left > 100)
        return Side.Right;
      else
        return null;
    }

    /// <summary>
    /// Check the collisions for the given paddles
    /// <br/>
    /// Note: In my pong game the paddles are actually behind the playable area. Do with that what you will
    /// </summary>
    /// <param name="paddles">The paddles to check the collisions for</param>
    public bool CheckPaddleCollision(PaddleServer[] paddles)
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
        Angle = 180 - Angle;

        // To make the game more interesting, we want to increase the velocity of the ball
        // Note this is disabled in gametype is impossible so the ball doesn't go out of the screen on the menu
        if (Game.Type != GameServer.GameType.BotImpossible)
          Velocity += 5;

        return;
      }

      Side? goalCollision = CheckGoalCollision();

      // If the ball didn't collide with a paddle, check if it is behind the goal
      // and invoke the OnOutOfBounds event if it is
      if (goalCollision != null)
        OnOutOfBounds?.Invoke(this, goalCollision.Value);
    }

    /// <summary>
    /// Generate a random angle for the ball to go in
    /// <br/>
    /// Note: This is between 0 and 180 degrees. This is so I can pick the side by adding or removing 90 degrees
    /// </summary>
    public static int RandomAngle()
    {
      Random r = new();

      int angleInDegrees = r.Next(0, 180);

      return angleInDegrees;
    }

    /// <summary>
    /// Normalize the angle so it's not annoying to play with
    /// </summary>
    /// <param name="angle">The angle to normalize</param>
    public static int NormalizeAngle(int angle)
    {
      // Normalize the angle so it's not annoying to play with
      // The ball should never go straight up or down or straight to the side
      if (angle % 90 < 15)
        angle += 15;
      else if (angle % 90 > 75)
        angle -= 15;

      return angle;
    }
  }
}
