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
      get => (int)(DateTime.UtcNow.Ticks - LastServerTimestamp);
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

      // Apparently this needs to be called again. I don't know why
      SetPosToMiddle();

      // Register the event handlers
      Connection.OnStartHandler += OnStart;
      Connection.OnUpdateHandler += OnUpdate;
    }

    public override void SetPosToMiddle()
    {
      // This should only be called by the Start event handler!

      ServerPosX = 50;
      ServerPosY = 50;
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
      PosX = ServerPosX + VelX * DeltaTime;
      PosY = ServerPosY + VelY * DeltaTime;
    }

    /// <summary>
    /// Unregisters the event handlers from the connection when the game is over
    /// </summary>
    public void UnregisterEventHandlers()
    {
      Connection.OnStartHandler -= OnStart;
      Connection.OnUpdateHandler -= OnUpdate;
    }

    /// <summary>
    /// Event handler for the "start" event
    /// <br/>
    /// Starts the ball's movement
    /// </summary>
    private void OnStart(object? _, Connection.StartEvent startEvent)
    {
      // Convert the timestamp to a DateTimeOffset so we can get the ticks from it
      DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(startEvent.Timestamp);
      LastServerTimestamp = dateTimeOffset.Ticks;

      // When this event is sent. The ball is in the middle of the screen
      SetPosToMiddle();

      VelX = startEvent.VelX;
      VelY = startEvent.VelY;
    }

    /// <summary>
    /// Event handler for the "update" event
    /// <br/>
    /// Updates the ball's position according to the server's position and velocity
    /// </summary>
    private void OnUpdate(object? _, Connection.UpdateEvent updateEvent)
    {
      // Convert the timestamp to a DateTimeOffset so we can get the ticks from it
      DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(
        updateEvent.Timestamp
      );
      LastServerTimestamp = dateTimeOffset.Ticks;

      ServerPosX = updateEvent.PosX;
      ServerPosY = updateEvent.PosY;
      VelX = updateEvent.VelX;
      VelY = updateEvent.VelY;
    }

    public override void Bounce()
    {
      // If the ball is inside the playable area, don't do anything as that will be handled by the server
      // We only want these bounces to happen when the ball is outside the playable area (cuz the game won't be running on the server)
      if (Right < 0 || Left > 100)
        base.Bounce();
    }
  }
}
