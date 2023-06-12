using System.Numerics;

namespace Pong
{
  public class RenderedBox : Box
  {
    /// <summary>
    /// The brush used to render the box
    /// </summary>
    public Brush Brush { get; set; }

    /// <summary>
    /// The scaling to use when rendering the box
    /// </summary>
    public double Scale { get; set; }

    /// <summary>
    /// The offset to use when rendering the box
    /// </summary>
    public Vector2 Offset { get; set; }

    /// <summary>
    /// Create a new instance of a RenderedBox with the specified position, width, height and brush
    /// <br/>
    /// Optionally a scaling and offset can be specified
    /// </summary>
    /// <param name="posX">The X position of the box</param>
    /// <param name="posY">The Y position of the box</param>
    /// <param name="width">The width of the box</param>
    /// <param name="height">The height of the box</param>
    /// <param name="scale">The scaling to use when rendering the box</param>
    /// <param name="brush">The brush used to render the box</param>
    public RenderedBox(
      double posX,
      double posY,
      double width,
      double height,
      Brush brush,
      double scale = 1.0,
      Vector2 offset = new Vector2()
    )
      : base(posX, posY, width, height)
    {
      Brush = brush;
      Scale = scale;
      Offset = offset;
    }

    /// <summary>
    /// Draw the box using the specified graphics object
    /// </summary>
    /// <param name="g">The graphics object to use to draw the box</param>
    public virtual void Draw(Graphics g)
    {
      // I'm using the Rendered properties here to make it easier to work with the box
      // All they do is multiply the value by the scale
      g.FillRectangle(
        Brush,
        (float)RenderedLeft,
        (float)RenderedTop,
        (float)RenderedWidth,
        (float)RenderedHeight
      );
    }

    // More properties to make it easier to work with the box cuz I'm a lazy bitch
    // These are for the scaling and don't have setters. They're just getters that multiply the value by the scale

    // Istg if there's a way to do make these automatically
    // I'm copy-pasting these like a dumbass

    /// <summary>
    /// The X position of the box scaled and offset for rendering
    /// </summary>
    public double RenderedPosX => PosX * Scale + Offset.X;

    /// <summary>
    /// The Y position of the box scaled and offset for rendering
    /// </summary>
    public double RenderedPosY => Top * Scale + Offset.Y;

    /// <summary>
    /// The width of the box scaled for rendering
    /// </summary>
    public double RenderedWidth => Width * Scale;

    /// <summary>
    /// The height of the box scaled for rendering
    /// </summary>
    public double RenderedHeight => Height * Scale;

    /// <summary>
    /// The top side of the box scaled and offset for rendering
    /// </summary>
    public virtual double RenderedTop => Top * Scale + Offset.Y;

    /// <summary>
    /// The bottom side of the box scaled and offset for rendering
    /// </summary>
    public virtual double RenderedBottom => Bottom * Scale + Offset.Y;

    /// <summary>
    /// The left side of the box scaled and offset for rendering
    /// </summary>
    public virtual double RenderedLeft => Left * Scale + Offset.X;

    /// <summary>
    /// The right side of the box scaled and offset for rendering
    /// </summary>
    public virtual double RenderedRight => Right * Scale + Offset.X;
  }
}
