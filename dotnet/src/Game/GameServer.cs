namespace Pong
{
  public class GameServer : Game
  {
    /// <summary>
    /// The ball in the game (server version)
    /// </summary>
    public new BallServer Ball { get; set; }

    /// <summary>
    /// Create a new instance of a GameServer with the specified width and height
    /// </summary>
    public GameServer(int width, int height)
      : base(width, height, new BallServer())
    {
      // This is here so the ball is of type BallServer..
      Ball = (BallServer)base.Ball;

      Start();
    }

    /// <summary>
    /// Start the game (set IsRunning to true)
    /// </summary>
    public new void Start()
    {
      base.Start();

      Ball.RandomlyStartMoving(600);

      Ball.SetPosToMiddle(Width, Height);
    }

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    public override void Move(double deltaTime)
    {
      if (!IsRunning)
        return;

      // Paddles (these should be moved first so that the ball can bounce off them)
      LeftPaddle.Move(deltaTime, Height);

      // Ball
      Ball.Move(deltaTime);
      Ball.Bounce(Width, Height, LeftPaddle);
    }
  }
}
