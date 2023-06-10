namespace Pong
{
  public class RenderedBox : Box
  {
    /// <summary>
    /// The brush used to render the box
    /// </summary>
    public Brush Brush { get; set; }

    /// <summary>
    /// Create a new instance of a RenderedBox with the specified position, width, height and brush
    /// </summary>
    public RenderedBox(double posX, double posY, double width, double height, Brush brush)
      : base(posX, posY, width, height)
    {
      Brush = brush;
    }

    /// <summary>
    /// Draw the box using the specified graphics object
    /// </summary>
    /// <param name="g">The graphics object to use to draw the box</param>
    public void Draw(Graphics g)
    {
      g.FillRectangle(Brush, (float)Left, (float)Top, (int)Width, (int)Height);
    }
  }
}
