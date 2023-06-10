namespace Pong
{
  public class GameServer : Game
  {
    public override Ball Ball { get; set; }
    public override Paddle LeftPaddle { get; set; }
    public override Paddle RightPaddle { get; set; }

    /// <summary>
    /// The ball in the game (Server) [i don't like this but it works for now]
    /// </summary>
    private BallServer BallInstance
    {
      get => (BallServer)Ball;
    }

    /// <summary>
    /// Create a new instance of a GameServer with the specified width and height
    /// </summary>
    public GameServer(int width, int height)
      : base(width, height)
    {
      // This is here so the ball is of type BallServer..
      Ball = new BallServer();

      // Initialize the paddles
      LeftPaddle = new(this);
      RightPaddle = new(this);

      // Position the paddles
      LeftPaddle.Left = Offset.X;
      RightPaddle.Right = Width - Offset.X;
    }

    public override void Start()
    {
      IsRunning = true;

      // Place the ball in the middle of the screen and start it moving
      BallInstance.RandomlyStartMoving(600);
      BallInstance.SetPosToMiddle(Width, Height);
    }

    public override void Move(double deltaTime)
    {
      if (!IsRunning)
        return;

      // Paddles (these should be moved first so that the ball can bounce off them)
      LeftPaddle.Move(deltaTime, Height);
      RightPaddle.Move(deltaTime, Height);

      // Ball
      BallInstance.Move(deltaTime);
      BallInstance.Bounce(Width, Height, LeftPaddle);
    }

    public override void KeyDown(Keys key)
    {
      // Start the game when a key is pressed
      if (!IsRunning)
        Start();

      if (key == Keys.W)
        LeftPaddle.MoveUp();
      else if (key == Keys.S)
        LeftPaddle.MoveDown();
      else if (key == Keys.Up)
        RightPaddle.MoveUp();
      else if (key == Keys.Down)
        RightPaddle.MoveDown();
    }

    public override void KeyUp(Keys key)
    {
      // This code looks stupid. See the explanation in Game.cs or hover over the method name

      if (key == Keys.W)
        LeftPaddle.MoveDown();
      else if (key == Keys.S)
        LeftPaddle.MoveUp();
      else if (key == Keys.Up)
        RightPaddle.MoveDown();
      else if (key == Keys.Down)
        RightPaddle.MoveUp();
    }
  }
}
