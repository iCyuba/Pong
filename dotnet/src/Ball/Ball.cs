namespace Pong
{
  public abstract class Ball : RenderedBox
  {
    /// <summary>
    /// The base radius of the ball. Scaled by the size of the game
    /// </summary>
    public static double BaseRadius = 2;

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
      // The width and height are multiplied by 2 it's a radius... It's kinda ugly ik
      : base(0, 0, BaseRadius * 2 * game.Scale, BaseRadius * 2 * game.Scale, Brushes.Olive)
    {
      Game = game;

      VelX = 0;
      VelY = 0;
    }

    /// <summary>
    /// Draw the ball on the screen using the Graphics object passed
    /// </summary>
    /// <param name="g">The Graphics object to draw the ball on</param>
    public override void Draw(Graphics g)
    {
      double x = PosX - Radius;
      double y = PosY - Radius;

      Font f = new("Segoe MDL2 Assets", (float)Diameter);

      g.FillRectangle(Brushes.Orange, (float)x, (float)y, (float)Diameter, (float)Diameter);
      // g.DrawString("", f, Brushes.HotPink, (float)x, (float)y);
    }

    /// <summary>
    /// Whether the ball was colliding with the walls in the last frame
    /// </summary>
    private bool lastCollisionForWalls { get; set; } = false;

    /// <summary>
    /// Whether or not the ball was colliding with each box in the last frame
    /// </summary>
    private Dictionary<Box, bool> lastCollisionForBoxes { get; set; } = new();

    /// <summary>
    /// Checks if the ball is colliding with the top or bottom of the screen. If it is, it returns true
    /// </summary>
    public bool CheckWallCollision()
    {
      return Top < 0 || Bottom > Game.Height;
    }

    /// <summary>
    /// Checks the collisions and acts accordingly
    /// </summary>
    /// <param name="boxes">The boxes to check collisions with</param>
    public virtual void Bounce(Box[]? boxes = null)
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

      // If there are no boxes, don't bother checking for collisions
      if (boxes == null)
        return;

      // Check the collisions for each box
      foreach (Box box in boxes)
      {
        bool collision = CollidesWith(box);

        bool lastCollisionForBox = lastCollisionForBoxes.ContainsKey(box)
          ? lastCollisionForBoxes[box]
          : false;

        // If the ball is colliding with the box, but it wasn't in the last frame
        // Bounce the ball
        if (collision && !lastCollisionForBox)
          VelX *= -1;

        // Remember te last collision for the box
        lastCollisionForBoxes[box] = collision;
      }
    }
  }
}
