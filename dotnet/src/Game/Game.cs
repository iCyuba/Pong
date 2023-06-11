using System.Numerics;

namespace Pong
{
  public abstract class Game
  {
    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsRunning { get; set; }

    /// <summary>
    /// The ball in the game (abstract)
    /// </summary>
    abstract public Ball Ball { get; set; }

    /// <summary>
    /// The left paddle (controlled with W and S)
    /// </summary>
    virtual public Paddle LeftPaddle { get; set; }

    /// <summary>
    /// The right paddle (controlled by the remote player, or with the arrow keys when playing locally)
    /// </summary>
    virtual public Paddle RightPaddle { get; set; }

    /// <summary>
    /// The size of the playable area (a square)
    /// <br/>
    /// Required because the on the server the game is a square, but on the client it's a rectangle
    /// </summary>
    public double Size
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
      get => Size / 100;
    }

    /// <summary>
    /// Create a new instance of a Game with the specified width and height
    /// </summary>
    public Game(int width, int height)
    {
      Width = width;
      Height = height;

      // Initialize the paddles
      LeftPaddle = new(this);
      RightPaddle = new(this);

      // Position the paddles (they are placed OUTSIDE of the game area. this is intentional)
      LeftPaddle.Right = Offset.X;
      RightPaddle.Left = Width - Offset.X;

      // By default the game is stopped
      IsRunning = false;
    }

    /// <summary>
    /// Start the game when a key is pressed
    /// </summary>
    public virtual void Start()
    {
      IsRunning = true;
    }

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    /// <param name="deltaTime">The time since the last update</param>
    public abstract void Move(double deltaTime);

    /// <summary>
    /// Draw the game. This is called every tick
    /// </summary>
    /// <param name="g">The graphics object to draw with</param>
    public void Draw(Graphics g)
    {
      Ball.Draw(g);
      LeftPaddle.Draw(g);
      RightPaddle.Draw(g);
    }

    /// <summary>
    /// This is the method that gets called when a key is pressed
    /// </summary>
    public virtual void KeyDown(Keys key)
    {
      // Start the game when a key is pressed
      if (!IsRunning)
        Start();

      if (key == Keys.W)
        LeftPaddle.MoveUp();
      else if (key == Keys.S)
        LeftPaddle.MoveDown();
    }

    /// <summary>
    /// This is the method that gets called when a key is released
    /// <br/>
    /// This calls the opposite function of the key that was released intentionally. It cancels out the velocity change and it is smoother imo.
    /// </summary>
    public virtual void KeyUp(Keys key)
    {
      if (key == Keys.W)
        LeftPaddle.MoveDown();
      else if (key == Keys.S)
        LeftPaddle.MoveUp();
    }
  }
}
