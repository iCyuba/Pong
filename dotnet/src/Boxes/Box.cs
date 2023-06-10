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
    public double Height { get; set; }

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
    /// The top side of the box
    /// </summary>
    public double Top
    {
      get => PosY - Height / 2;
      set => PosY = value + Height / 2;
    }

    /// <summary>
    /// The bottom side of the box
    /// </summary>
    public double Bottom
    {
      get => PosY + Height / 2;
      set => PosY = value - Height / 2;
    }

    /// <summary>
    /// The left side of the box
    /// </summary>
    public double Left
    {
      get => PosX - Width / 2;
      set => PosX = value + Width / 2;
    }

    /// <summary>
    /// The right side of the box
    /// </summary>
    public double Right
    {
      get => PosX + Width / 2;
      set => PosX = value - Width / 2;
    }
  }
}
