using System.Numerics;

namespace Pong
{
  public abstract class Game<BallType, PaddleType>
    where BallType : Ball
    where PaddleType : Paddle
  {
    /// <summary>
    /// The width of the game, this is different from the size of the game!
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the game, this is different from the size of the game!
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Whether or not the game is running
    /// </summary>
    public bool IsRunning { get; set; }

    /// <summary>
    /// The ball in the game (abstract)
    /// </summary>
    abstract public BallType BallInstance { get; set; }

    /// <summary>
    /// The left paddle (controlled with W and S)
    /// </summary>
    abstract public PaddleType LeftPaddle { get; set; }

    /// <summary>
    /// The right paddle (controlled by the remote player, or with the arrow keys when playing locally)
    /// </summary>
    abstract public PaddleType RightPaddle { get; set; }

    /// <summary>
    /// The score of the left player
    /// </summary>
    public int LeftScore { get; set; }

    /// <summary>
    /// The score of the right player
    /// </summary>
    public int RightScore { get; set; }

    /// <summary>
    /// The size of the playable area (a square)
    /// <br/>
    /// Required because the on the server the game is a square, but on the client it's a rectangle
    /// </summary>
    public int Size
    {
      get => Math.Min(Width, Height);
    }

    /// <summary>
    /// The offset of the game from the top left corner of the window
    /// <br/>
    /// Required because the on the server the game is a square, but on the client it's a rectangle
    /// </summary>
    public Vector2 Offset
    {
      get => new((float)(Width - Size) / 2, (float)(Height - Size) / 2);
    }

    /// <summary>
    /// The scaling factor for all of the sizes of the objects in the game
    /// <br/>
    /// In GameServer this is required because the on the server the width and height are 100%. On the client we have to scale it up to the size of the window
    /// </summary>
    public double Scale
    {
      get => (double)Size / 100;
    }

    // Events <3

    /// <summary>
    /// Called when a message should be shown
    /// </summary>
    public EventHandler<string>? OnShowMessage { get; set; }

    /// <summary>
    /// Called when a message should be hidden
    /// </summary>
    public EventHandler? OnHideMessage { get; set; }

    /// <summary>
    /// Called when the game will start in some time
    /// </summary>
    public EventHandler<DateTime>? OnStartIn { get; set; }

    /// <summary>
    /// Called when the score changes
    /// </summary>
    public EventHandler? OnScore { get; set; }

    /// <summary>
    /// Create a new instance of a Game with the specified width and height
    /// </summary>
    public Game(int width, int height)
    {
      Width = width;
      Height = height;

      // By default the game is stopped
      IsRunning = false;
    }

    /// <summary>
    /// Position the paddles (they are placed OUTSIDE of the game area. this is intentional)
    /// </summary>
    protected void PositionPaddles()
    {
      LeftPaddle.Right = 0;
      RightPaddle.Left = 100;
    }

    /// <summary>
    /// Start the game when a key is pressed
    /// </summary>
    public virtual void Start()
    {
      IsRunning = true;
      StartAt = null;

      BallInstance!.SetPosToMiddle();

      OnHideMessage?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// The time to start the game at
    /// </summary>
    protected DateTime? StartAt = null;

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    /// <param name="deltaTime">The time since the last update</param>
    public virtual void Update(double deltaTime)
    {
      // If we're not running and past the start time, start the game
      if (!IsRunning && StartAt.HasValue && StartAt.Value.Ticks < DateTime.Now.Ticks)
      {
        Start();

        // Reset the start time
        StartAt = null;
      }
    }

    /// <summary>
    /// Draw the game. This is called every tick
    /// </summary>
    /// <param name="g">The graphics object to draw with</param>
    public void Draw(Graphics g)
    {
      BallInstance!.Draw(g);
      LeftPaddle!.Draw(g);
      RightPaddle!.Draw(g);
    }

    /// <summary>
    /// This is the method that gets called when a key is pressed
    /// </summary>
    public virtual void KeyDown(Keys key)
    {
      // They both intentioanlly control the same paddle.
      // This is because out of 4 game types. 3 of them can be controlled by both keys
      // In local multiplayer, simply a return will be called before reaching this point
      if (key == Keys.W)
        LeftPaddle!.MoveUp();
      else if (key == Keys.S)
        LeftPaddle!.MoveDown();
      else if (key == Keys.Up)
        LeftPaddle!.MoveUp();
      else if (key == Keys.Down)
        LeftPaddle!.MoveDown();
    }

    /// <summary>
    /// This is the method that gets called when a key is released
    /// <br/>
    /// This calls the opposite function of the key that was released intentionally. It cancels out the velocity change and it is smoother imo.
    /// </summary>
    public virtual void KeyUp(Keys key)
    {
      if (key == Keys.W)
        LeftPaddle!.MoveDown();
      else if (key == Keys.S)
        LeftPaddle!.MoveUp();
      else if (key == Keys.Up)
        LeftPaddle!.MoveDown();
      else if (key == Keys.Down)
        LeftPaddle!.MoveUp();
    }
  }
}
