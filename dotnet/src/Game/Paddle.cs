namespace Pong
{
  public class Paddle : RenderedBox
  {
    /// <summary>
    /// The width used for all paddles in percentage of the screen width
    /// </summary>
    public static int BaseWidth = 4;

    /// <summary>
    /// The height used for all paddles in percentage of the screen height
    /// </summary>
    public static int BaseHeight = 20;

    /// <summary>
    /// The game the paddle belongs to
    /// </summary>
    private Game Game { get; set; }

    /// <summary>
    /// The velocity of the paddle on the Y axis.
    /// </summary>
    public double VelY { get; set; }

    /// <summary>
    /// Create a new instance of a Paddle in the middle of the screen and scale it to an appropriate size
    /// (It's created at X=0 intentionally.. I use the setters <see cref="Box.Left"/> and <see cref="Box.Right"/> to set the position)
    /// </summary>
    /// <param name="game">The game that the paddle is in</param>
    public Paddle(Game game)
      : base(
        0, // The X position is set manually later with the setters of Left and Right
        game.Height / 2, // Place the paddle in the middle of the screen
        Paddle.BaseWidth * game.Scale, // Scale the width and height of the paddle because the base sizes are in percentages
        Paddle.BaseHeight * game.Scale,
        Brushes.DeepPink
      )
    {
      Game = game;
    }

    /// <summary>
    /// Move the paddle by the specified amount of time
    /// </summary>
    public void Move(double deltaTime, int height)
    {
      // Update the position of the paddle
      PosY += VelY * deltaTime;

      // If the paddle is outside of the screen, move it back in
      // Yes, I could return (like I did before) but then I would have to manually calculate the Top and Bottom.. andddd I'm lazy
      if (Top < 0)
        Top = 0;
      else if (Bottom > height)
        Bottom = height;
    }

    /// <summary>
    /// Start moving the paddle up / stop moving the paddle down
    /// </summary>
    public void MoveUp()
    {
      // Return if the paddle is already moving up
      if (VelY < 0)
        return;

      VelY -= 500;
    }

    /// <summary>
    /// Start moving the paddle down / stop moving the paddle up
    /// </summary>
    public void MoveDown()
    {
      // Return if the paddle is already moving down
      if (VelY > 0)
        return;

      VelY += 500;
    }

    public override void Draw(Graphics g)
    {
      base.Draw(g);
    }
  }
}
