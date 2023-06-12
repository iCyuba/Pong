namespace Pong
{
  public class GameClient : Game
  {
    public override Ball Ball { get; set; }

    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// The ball in the game (Client) [i don't like this but it works for now]
    /// </summary>
    private BallClient BallInstance
    {
      get => (BallClient)Ball;
    }

    /// <summary>
    /// Create a new instance of a GameClient with the specified width, height and connection
    /// </summary>
    public GameClient(int width, int height, Connection connection)
      : base(width, height)
    {
      Connection = connection;

      // Initialize the ball with the provided connection
      Ball = new BallClient(connection, this);

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
      BallInstance.UnregisterEventHandlers();
    }

    public override void Move(double deltaTime)
    {
      if (!IsRunning)
        return;

      // Paddles
      LeftPaddle.Move(deltaTime);
      RightPaddle.Move(deltaTime);

      // Ball
      BallInstance.Move();
      BallInstance.Bounce();
    }
  }
}
