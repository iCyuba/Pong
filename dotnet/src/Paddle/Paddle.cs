using System.Numerics;

namespace Pong
{
  public class Paddle : RenderedBox
  {
    /// <summary>
    /// The width used for all paddles in percentage of the screen width
    /// </summary>
    public const double BaseWidth = 4;

    /// <summary>
    /// The height used for all paddles in percentage of the screen height
    /// </summary>
    public const double BaseHeight = 20;

    /// <summary>
    /// The base speed of the paddle in percentage of the screen height per second
    /// </summary>
    public const double BaseSpeed = 80;

    /// <summary>
    /// The velocity of the paddle on the Y axis.
    /// </summary>
    public double VelY { get; set; }

    /// <summary>
    /// Create a new instance of a Paddle in the middle of the screen and scale it to an appropriate size
    /// (It's created at X=0 intentionally.. I use the setters <see cref="Box.Left"/> and <see cref="Box.Right"/> to set the position)
    /// </summary>
    /// <param name="scale">The scale of the game</param>
    /// <param name="offset">The offset of the game</param>
    public Paddle(double scale, Vector2 offset)
      : base(0, 50, BaseWidth, BaseHeight, Brushes.DeepPink, scale, offset) { }

    /// <summary>
    /// Move the paddle by the specified amount of time
    /// </summary>
    public void Move(double deltaTime)
    {
      // Update the position of the paddle
      PosY += VelY * deltaTime;

      // If the paddle is outside of the screen, move it back in
      // Yes, I could return (like I did before) but then I would have to manually calculate the Top and Bottom.. andddd I'm lazy
      if (Top < 0)
        Top = 0;
      else if (Bottom > 100)
        Bottom = 100;
    }

    /// <summary>
    /// Start moving the paddle up / stop moving the paddle down
    /// </summary>
    public void MoveUp()
    {
      // Return if the paddle is already moving up
      if (VelY < 0)
        return;

      VelY -= BaseSpeed;
    }

    /// <summary>
    /// Start moving the paddle down / stop moving the paddle up
    /// </summary>
    public void MoveDown()
    {
      // Return if the paddle is already moving down
      if (VelY > 0)
        return;

      VelY += BaseSpeed;
    }
  }
}
