namespace Pong
{
  public class Game
  {
    public int Width { get; set; }
    public int Height { get; set; }

    public bool IsRunning { get; set; }

    public Heart Heart { get; set; }

    public Paddle LeftPaddle { get; set; }

    public Game(int width, int height)
    {
      Width = width;
      Height = height;

      IsRunning = false;

      Heart = new();
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
    public void Move(double deltaTime)
    {
      if (!IsRunning)
        return;

      // Heart
      Heart.Move(deltaTime);

      // Paddles
      LeftPaddle.Move(deltaTime, Height);
    }

    /// <summary>
    /// Draw the game. This is called every tick
    /// </summary>
    public void Draw(Graphics g, double deltaTime)
    {
      Heart.Draw(g);
      LeftPaddle.Draw(g);
    }
  }
}
