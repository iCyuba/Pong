using System.Numerics;

namespace Pong
{
  public class GameClient : Game
  {
    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// The ball in the game (abstract)
    /// </summary>
    public override Ball Ball { get; set; }

    /// <summary>
    /// The ball in the game (Client) [i don't like this but it works for now]
    /// </summary>
    private BallClient BallInstance
    {
      get =>
        Ball as BallClient ?? throw new InvalidOperationException("Ball is not of type BallClient");
    }

    /// <summary>
    /// The size of the playable area
    /// <br/>
    /// Required because the on the server the game is a square, but on the client it's a rectangle
    /// </summary>
    public double Size
    {
      get => Math.Min(Width, Height);
    }

    /// <summary>
    /// The scaling factor for all of the positions, sizes, velocities, etc.
    /// <br/>
    /// Required because the on the server the width and height are 100%. Here we have to scale it up to the size of the window
    /// </summary>
    public double Scale
    {
      get => Size / 100;
    }

    /// <summary>
    /// The offset of the game from the top left corner of the window
    /// <br/>
    /// Required because the on the server the game is a square, but on the client it's a rectangle
    /// </summary>
    public Vector2 Offset
    {
      get => new((float)(Width - Size) / 2, (float)(Height - Size) / 2);
    }

    /// <summary>
    /// Create a new instance of a GameClient with the specified width, height and connection
    /// </summary>
    public GameClient(int width, int height, Connection connection)
      : base(width, height)
    {
      Ball = new BallClient(connection, this);

      Connection = connection;

      Start();
    }

    /// <summary>
    /// Unregister event handlers for the connection
    /// <br/>
    /// This is called when the game is closed
    /// </summary>
    public void UnregisterEventHandlers()
    {
      // Unregister event handlers for the connection

      // Required check if the ball is a BallClient (it should always be)
      if (Ball is BallClient ballClient)
        ballClient.UnregisterEventHandlers();
    }

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    /// <param name="_">Unused, this is the delta time. Only the GameServer needs this</param>
    public override void Move(double _)
    {
      if (!IsRunning)
        return;

      // Ball
      BallInstance.Move();
    }
  }
}
