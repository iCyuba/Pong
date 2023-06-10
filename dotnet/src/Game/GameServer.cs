namespace Pong
{
  public class GameServer : Game
  {
    /// <summary>
    /// The ball in the game (abstract)
    /// </summary>
    public override Ball Ball { get; set; }

    /// <summary>
    /// The ball in the game (Server) [i don't like this but it works for now]
    /// </summary>
    private BallServer BallInstance
    {
      get =>
        Ball as BallServer ?? throw new InvalidOperationException("Ball is not of type BallServer");
    }

    /// <summary>
    /// Create a new instance of a GameServer with the specified width and height
    /// </summary>
    public GameServer(int width, int height)
      : base(width, height)
    {
      // This is here so the ball is of type BallServer..
      Ball = new BallServer();

      Start();
    }

    /// <summary>
    /// Start the game (set IsRunning to true)
    /// </summary>
    public new void Start()
    {
      base.Start();

      BallInstance.RandomlyStartMoving(600);

      BallInstance.SetPosToMiddle(Width, Height);
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
      BallInstance.Move(deltaTime);
      BallInstance.Bounce(Width, Height, LeftPaddle);
    }
  }
}
