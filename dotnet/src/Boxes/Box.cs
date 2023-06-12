namespace Pong
{
  public class Box
  {
    /// <summary>
    /// The position of the box on the X axis on the center of the box.
    /// </summary>
    public double PosX { get; set; }

    /// <summary>
    /// The position of the paddle on the Y axis on the center of the box.
    /// </summary>
    public double PosY { get; set; }

    /// <summary>
    /// The width of the paddle.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// The height of the paddle.
    /// </summary>
    public virtual double Height { get; set; }

    /// <summary>
    /// Create a new instance of a Box with the specified position, width and height
    /// </summary>
    /// <param name="posX">The position of the box on the X axis</param>
    /// <param name="posY">The position of the box on the Y axis</param>
    /// <param name="width">The width of the box</param>
    /// <param name="height">The height of the box</param>
    public Box(double posX, double posY, double width, double height)
    {
      PosX = posX;
      PosY = posY;
      Width = width;
      Height = height;
    }

    /// <summary>
    /// Check if the box collides with another box
    /// </summary>
    /// <param name="other">The other box to check for collision</param>
    /// <returns>True if the boxes collide, false otherwise</returns>
    public bool CollidesWith(Box other)
    {
      bool collidesX = Right > other.Left && Left < other.Right;
      bool collidesY = Bottom > other.Top && Top < other.Bottom;

      bool collides = collidesX && collidesY;

      // If the box doesn't collide with the other box, return null
      return collides;
    }

    // The following properties are used to make it easier to work with the box
    // and to make the code more readable. They are not strictly necessary.

    /// <summary>
    /// The top side of the box
    /// </summary>
    public virtual double Top
    {
      get => PosY - Height / 2;
      set => PosY = value + Height / 2;
    }

    /// <summary>
    /// The bottom side of the box
    /// </summary>
    public virtual double Bottom
    {
      get => PosY + Height / 2;
      set => PosY = value - Height / 2;
    }

    /// <summary>
    /// The left side of the box
    /// </summary>
    public virtual double Left
    {
      get => PosX - Width / 2;
      set => PosX = value + Width / 2;
    }

    /// <summary>
    /// The right side of the box
    /// </summary>
    public virtual double Right
    {
      get => PosX + Width / 2;
      set => PosX = value - Width / 2;
    }
  }
}
