using System.Numerics;

namespace Pong
{
  public abstract class Ball : RenderedBox
  {
    /// <summary>
    /// The base radius of the ball. Scaled by the size of the game
    /// </summary>
    public const int BaseRadius = 2;

    /// <summary>
    /// The base velocity of the ball in percent of the screen per second
    /// </summary>
    public const int BaseVelocity = 50;

    /// <summary>
    /// The velocity of the ball in percent of the screen per second
    /// </summary>
    public int Velocity { get; set; }

    /// <summary>
    /// The angle in which the ball is moving
    /// </summary>
    public double Angle { get; set; }

    /// <summary>
    /// The angle but in radians
    /// </summary>
    public double AngleRadians => Angle * Math.PI / 180;

    // Basically just make the height and width the same thing
    // I know this isn't a good thing to do. I also however do not care
    public override double Height
    {
      get => Width;
      set => Width = value;
    }

    /// <summary>
    /// The diameter of the ball (based on the width lmfaoo)
    /// </summary>
    public double Diameter
    {
      get => Width;
      set => Width = value;
    }

    /// <summary>
    /// The radius of the ball
    /// </summary>
    public double Radius
    {
      get => Diameter / 2;
      set => Diameter = value * 2;
    }

    /// <summary>
    /// The velocity of the ball on the X axis
    /// </summary>
    public double VelX => Velocity * Math.Cos(AngleRadians);

    /// <summary>
    /// The velocity of the ball on the Y axis
    /// </summary>
    public double VelY => Velocity * Math.Sin(AngleRadians);

    /// <summary>
    /// New instance of a Ball..
    /// <br/>
    /// Should be initialized by a Game class (e.g. GameServer or GameClient)
    /// </summary>
    /// <param name="scale">The scale of the game</param>
    /// <param name="offset">The offset of the game</param>
    public Ball(double scale, Vector2 offset)
      // The width and height are multiplied by 2.. it's a radius... It's kinda ugly ik
      : base(50, 50, BaseRadius * 2, BaseRadius * 2, Brushes.HotPink, scale, offset)
    {
      Velocity = 0;
    }

    /// <summary>
    /// Set the position of the ball to the middle of the screen
    /// </summary>
    public virtual void SetPosToMiddle()
    {
      PosX = 50;
      PosY = 50;
    }

    /// <summary>
    /// Draw the ball on the screen using the Graphics object passed
    /// </summary>
    /// <param name="g">The Graphics object to draw the ball on</param>
    public override void Draw(Graphics g)
    {
      // I'm using the scaled values because the ball is scaled by the size of the game
      // So if the game is bigger, the ball is bigger
      g.FillEllipse(
        Brush,
        (float)RenderedLeft,
        (float)RenderedTop,
        (float)RenderedWidth,
        (float)RenderedHeight
      );

      // Font font = new("Segoe MDL2 Assets", (float)Diameter);
      // g.DrawString("", font, Brush, (float)x, (float)y);
    }

    /// <summary>
    /// Whether the ball was colliding with the walls in the last frame
    /// </summary>
    private bool lastCollisionForWalls { get; set; } = false;

    /// <summary>
    /// Checks if the ball is colliding with the top or bottom of the screen. If it is, it returns true
    /// </summary>
    public bool CheckWallCollision()
    {
      return Top < 0 || Bottom > 100;
    }

    /// <summary>
    /// Checks the collisions and acts accordingly
    /// </summary>
    public virtual void Bounce()
    {
      // Even in the client, the ball is still checked for collisions with the walls
      // This is so if the client is lagging, the ball will still bounce off the walls
      // Also it will bounce off the walls in the space behind the paddle (on the server the game stops once the ball is behind the paddle)
      if (CheckWallCollision())
      {
        if (!lastCollisionForWalls)
          Angle = 360 - Angle;

        lastCollisionForWalls = true;
      }
      else
        lastCollisionForWalls = false;
    }
  }
}
