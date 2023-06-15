namespace Pong
{
  public class GameServer : Game<BallServer, PaddleServer>
  {
    /// <summary>
    /// The type of game that is being played
    /// </summary>
    public enum GameType
    {
      /// <summary>
      /// A game against a bot that is easy to beat. It copies 75% of the velocity of the ball
      /// </summary>
      BotEasy = 50,

      /// <summary>
      /// A game against a bot that is medium difficulty. It copies 65% of the velocity of the ball
      /// </summary>
      BotMedium = 65,

      /// <summary>
      /// A game against a bot that is hard to beat. It copies 80% of the velocity of the ball
      /// </summary>
      BotHard = 80,

      /// <summary>
      /// A game that is meant just for watching.. It copies 100% of the velocity of the ball
      /// </summary>
      BotImpossible = 100,

      /// <summary>
      /// A game where both players are controlled. Left = W/S, Right = Up/Down
      /// </summary>
      Local = 0,
    }

    public override BallServer BallInstance { get; set; }
    public override PaddleServer LeftPaddle { get; set; }
    public override PaddleServer RightPaddle { get; set; }

    /// <summary>
    /// The game type used in this "session", by default it is Local
    /// </summary>
    public GameType Type { get; set; }

    /// <summary>
    /// Create a new instance of a GameServer with the specified width and height
    /// </summary>
    /// <param name="width">The width of the game</param>
    /// <param name="height">The height of the game</param>
    /// <param name="type">The type of game that is being played</param>
    public GameServer(int width, int height, GameType type = GameType.Local)
      : base(width, height)
    {
      // This is here so the ball is of type BallServer..
      BallInstance = new BallServer(this);

      // Create the paddles
      LeftPaddle = new(this);
      RightPaddle = new(this);
      PositionPaddles();

      // Set the game type
      Type = type;

      // Add event handlers for the ball
      BallInstance.OnOutOfBounds += OnOutOfBounds;
    }

    public override void Start()
    {
      base.Start();

      // Place the ball in the middle of the screen and start it moving
      BallInstance.RandomlyStartMoving(Ball.BaseVelocity);
    }

    /// <summary>
    /// Start the game after 3 seconds (or more) pass
    /// </summary>
    public virtual void StartIn3Seconds()
    {
      // If there's already a start time, don't do anything
      if (StartAt != null)
        return;

      StartAt = DateTime.Now.AddSeconds(3);

      OnStartIn?.Invoke(this, StartAt.Value);
    }

    public override void Update(double deltaTime)
    {
      // Run the base method
      base.Update(deltaTime);

      // Call the bot movement method if the type is a bot and the game is running
      if (IsRunning && Type != GameType.Local)
        RightPaddle.BotMovement(BallInstance, Type);

      // Paddles (these should be moved first so that the ball can bounce off them)
      LeftPaddle.Move(deltaTime);
      RightPaddle.Move(deltaTime);

      // Ball
      BallInstance.Move(deltaTime);
      // Only update collisions if the game is running
      if (IsRunning)
        BallInstance.Bounce();
    }

    /// <summary>
    /// This is called when the ball goes out of bounds
    /// </summary>
    /// <param name="side">The side that the ball went out of bounds on</param>
    private void OnOutOfBounds(object? _, BallServer.Side side)
    {
      // Stop the game
      IsRunning = false;

      // If the ball went out of bounds on the left side, the right paddle scored
      if (side == BallServer.Side.Left)
        RightScore++;
      // If the ball went out of bounds on the right side, the left paddle scored
      else if (side == BallServer.Side.Right)
        LeftScore++;

      // Invoke the OnScore event
      OnScore?.Invoke(this, EventArgs.Empty);

      // Start the game again in 3 seconds
      StartIn3Seconds();
    }

    // Add support for the right paddle
    public override void KeyDown(Keys key)
    {
      // Start the game if it isn't running
      if (!IsRunning)
        StartIn3Seconds();

      // When the type isn't local, the right paddle can be controlled by the arrow keys so just call the base method and return
      if (Type != GameType.Local)
      {
        base.KeyDown(key);
        return;
      }

      // If the type is local, the right paddle is controlled by the Up/Down keys so we need to handle them here
      // Return so that the base method isn't called (cuz it'd also move the left paddle)
      if (key == Keys.Up)
        RightPaddle.MoveUp();
      else if (key == Keys.Down)
        RightPaddle.MoveDown();
      // Handle W/S keys normally (assuming that's what the "else" here.. can be something else ig)
      else
        base.KeyDown(key);
    }

    // Add support for the right paddle
    public override void KeyUp(Keys key)
    {
      // When the type isn't local, call the base method and return (explanation is in the KeyDown method)
      if (Type != GameType.Local)
      {
        base.KeyUp(key);
        return;
      }

      // This code looks stupid. See the explanation in Game.cs or hover over the method name
      // Otherwise just call the opposite method of the KeyDown method when the keys are Up or Down
      // The reason why I'm not doing .Stop() or something is explained (kinda) in the base method in the summary
      if (key == Keys.Up)
        RightPaddle.MoveDown();
      else if (key == Keys.Down)
        RightPaddle.MoveUp();
      // For W/S keys, call the base method
      else
        base.KeyUp(key);
    }
  }
}
