namespace Pong
{
  public abstract class Game
  {
    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsRunning { get; set; }

    public Ball Ball { get; set; }
    public Paddle LeftPaddle { get; set; }

    public Game(int width, int height, Ball ball)
    {
      Width = width;
      Height = height;

      IsRunning = false;

      // Use the ball passed in
      Ball = ball;

      LeftPaddle = new(Width / 8, Height / 2, 20, 100);
    }

    /// <summary>
    /// Start the game (set IsRunning to true)
    /// </summary>
    public void Start()
    {
      IsRunning = true;
    }

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    public abstract void Move(double deltaTime);

    /// <summary>
    /// Draw the game. This is called every tick
    /// </summary>
    public void Draw(Graphics g, double deltaTime)
    {
      Ball.Draw(g);
      LeftPaddle.Draw(g);
    }

    /// <summary>
    /// This is the method that gets called when a key is pressed
    /// </summary>
    public void KeyDown(Keys key)
    {
      if (key == Keys.W)
        LeftPaddle.MoveUp();
      else if (key == Keys.S)
        LeftPaddle.MoveDown();
    }

    /// <summary>
    /// This is the method that gets called when a key is released
    /// </summary>
    public void KeyUp(Keys key)
    {
      LeftPaddle.Stop();
    }
  }
}
