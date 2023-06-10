namespace Pong
{
  public class BallClient : Ball
  {
    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// The timestamp of the last time the server sent the ball's position (in milliseconds)
    /// </summary>
    private long LastServerTimestamp { get; set; }

    /// <summary>
    /// Delta time between the last time the server sent the ball's position and the current time (in ticks)
    /// </summary>
    private int DeltaTimeTicks
    {
      get => (int)(DateTime.Now.Ticks - LastServerTimestamp);
    }

    /// <summary>
    /// Delta time between the last time the server sent the ball's position and the current time (in seconds)
    /// </summary>
    // This is split into two properties so it's easier to read
    private double DeltaTime
    {
      get => (double)DeltaTimeTicks / TimeSpan.TicksPerSecond;
    }

    /// <summary>
    /// The position of the center of the ball on the X axis that the server sent
    /// </summary>
    public double ServerPosX { get; set; }

    /// <summary>
    /// The position of the center of the ball on the Y axis that the server sent
    /// </summary>
    public double ServerPosY { get; set; }

    /// <summary>
    /// New instance of a Ball Client..
    /// <br/>
    /// Should be initialized by a Game class (e.g. GameServer or GameClient)
    /// <br/>
    /// A connection is required to initialize a BallClient
    /// </summary>
    /// <param name="connection">The connection to the server</param>
    /// <param name="game">The game that created this ball</param>
    public BallClient(Connection connection, Game game)
      : base(game)
    {
      Connection = connection;

      Connection.OnUpdateHandler += OnUpdate;
    }

    /// <summary>
    /// Moves the ball according to its velocity and the time passed
    /// <br/>
    /// Unlike on the server version, this is calculated against the server's last position instead of the current position.
    /// <br/>
    /// Without this, the ball is desynced from the server's position and the bounces look strange
    /// </summary>
    public void Move()
    {
      PosX = (ServerPosX + Game.Offset.X) + VelX * DeltaTime;
      PosY = (ServerPosY + Game.Offset.Y) + VelY * DeltaTime;
    }

    /// <summary>
    /// Unregisters the event handlers from the connection when the game is over
    /// </summary>
    public void UnregisterEventHandlers()
    {
      Connection.OnUpdateHandler -= OnUpdate;
    }

    /// <summary>
    /// Event handler for the "update" event
    /// <br/>
    /// Updates the ball's position according to the server's position and velocity
    /// </summary>
    private void OnUpdate(object? _, Connection.UpdateEvent updateEvent)
    {
      // TODO: Change this to the timestamp the server sent the update event
      LastServerTimestamp = DateTime.Now.Ticks;

      ServerPosX = updateEvent.PosX * Game.Scale;
      ServerPosY = updateEvent.PosY * Game.Scale;
      VelX = updateEvent.VelX * Game.Scale;
      VelY = updateEvent.VelY * Game.Scale;
    }
  }
}
