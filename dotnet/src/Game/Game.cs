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
    abstract public Paddle LeftPaddle { get; set; }

    /// <summary>
    /// The right paddle (controlled by the remote player, or with the arrow keys when playing locally)
    /// </summary>
    abstract public Paddle RightPaddle { get; set; }

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

      IsRunning = false;
    }

    /// <summary>
    /// Start the game when a key is pressed
    /// </summary>
    public abstract void Start();

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
    public abstract void KeyDown(Keys key);

    /// <summary>
    /// This is the method that gets called when a key is released
    /// <br/>
    /// This calls the opposite function of the key that was released intentionally. It cancels out the velocity change and it is smoother imo.
    /// </summary>
    public abstract void KeyUp(Keys key);
  }
}
