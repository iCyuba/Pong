namespace Pong
{
  public class GameServer : Game
  {
    public override Ball Ball { get; set; }

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
      Ball = new BallServer(this);
    }

    public override void Start()
    {
      base.Start();

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
      BallInstance.Bounce();
    }

    // Add support for the right paddle
    public override void KeyDown(Keys key)
    {
      base.KeyDown(key);

      if (key == Keys.Up)
        RightPaddle.MoveUp();
      else if (key == Keys.Down)
        RightPaddle.MoveDown();
    }

    // Add support for the right paddle
    public override void KeyUp(Keys key)
    {
      base.KeyUp(key);

      // This code looks stupid. See the explanation in Game.cs or hover over the method name
      if (key == Keys.Up)
        RightPaddle.MoveDown();
      else if (key == Keys.Down)
        RightPaddle.MoveUp();
    }
  }
}
