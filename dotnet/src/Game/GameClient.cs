namespace Pong
{
  public class GameClient : Game
  {
    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// The ball in the game (client version)
    /// </summary>
    public new BallClient Ball { get; set; }

    /// <summary>
    /// Create a new instance of a GameClient with the specified width, height and connection
    /// </summary>
    public GameClient(int width, int height, Connection connection)
      : base(width, height, new BallClient(connection))
    {
      Ball = (BallClient)base.Ball;

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
      Ball.Move();
    }
  }
}
