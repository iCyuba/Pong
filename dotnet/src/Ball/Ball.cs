// BEGIN: c8a9d1d5fj3w
namespace Pong
{
  public abstract class Ball : RenderedBox
  {
    /// <summary>
    /// The base radius of the ball. Scaled by the size of the game
    /// </summary>
    public const double BaseRadius = 2;

    /// <summary>
    /// The base velocity of the ball in percent of the screen per second
    /// </summary>
    public const double BaseVelocity = 50;

    /// <summary>
    /// The game client that created this ball
    /// </summary>
    protected Game Game { get; set; }

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
    public double VelX { get; set; }

    /// <summary>
    /// The velocity of the ball on the Y axis
    /// </summary>
    public double VelY { get; set; }

    /// <summary>
    /// New instance of a Ball..
    /// <br/>
    /// Should be initialized by a Game class (e.g. GameServer or GameClient)
    /// </summary>
    /// <param name="game">The game that the ball is in</param>
    public Ball(Game game)
      // The width and height are multiplied by 2.. it's a radius... It's kinda ugly ik
      : base(50, 50, BaseRadius * 2, BaseRadius * 2, Brushes.HotPink, game.Scale, game.Offset)
    {
      Game = game;

      VelX = 0;
      VelY = 0;
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
          VelY *= -1;

        lastCollisionForWalls = true;
      }
      else
        lastCollisionForWalls = false;
    }
  }
}
