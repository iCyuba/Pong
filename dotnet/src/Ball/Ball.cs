namespace Pong
{
  public abstract class Ball
  {
    /// <summary>
    /// The position of the center of the ball on the X axis
    /// </summary>
    public double PosX { get; set; }

    /// <summary>
    /// The position of the center of the ball on the Y axis
    /// </summary>
    public double PosY { get; set; }

    /// <summary>
    /// The velocity of the ball on the X axis
    /// </summary>
    public double VelX { get; set; }

    /// <summary>
    /// The velocity of the ball on the Y axis
    /// </summary>
    public double VelY { get; set; }

    /// <summary>
    /// The radius of the ball
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// The diameter of the ball
    /// </summary>
    public double Diameter
    {
      get => Radius * 2;
      set => Radius = value / 2;
    }

    /// <summary>
    /// New instance of a Ball..
    /// <br/>
    /// Should be initialized by a Game class (e.g. GameServer or GameClient)
    /// </summary>
    public Ball()
    {
      PosX = 0;
      PosY = 0;
      VelX = 0;
      VelY = 0;
      Diameter = 25;
    }

    /// <summary>
    /// Draw the ball on the screen using the Graphics object passed
    /// </summary>
    /// <param name="g">The Graphics object to draw the ball on</param>
    public void Draw(Graphics g)
    {
      double x = PosX - Radius;
      double y = PosY - Radius;

      Font f = new("Segoe MDL2 Assets", (float)Diameter);

      g.FillRectangle(Brushes.Orange, (float)x, (float)y, (float)Diameter, (float)Diameter);
      // g.DrawString("", f, Brushes.HotPink, (float)x, (float)y);
    }
  }
}
