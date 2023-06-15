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
    private long DeltaTimeTicks
    {
      get => DateTime.UtcNow.Ticks - LastServerTimestamp;
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
    public BallClient(Connection connection, GameClient game)
      : base(game.Scale, game.Offset)
    {
      Connection = connection;

      // Apparently this needs to be called again. I don't know why
      SetPosToMiddle();

      // Register the event handlers
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
      // If delta time is negative, the game probably hasn't started yet
      if (DeltaTime < 0)
        return;

      PosX = ServerPosX + VelX * DeltaTime;
      PosY = ServerPosY + VelY * DeltaTime;
    }

    /// <summary>
    /// Unregisters the event handlers from the connection when the game is over
    /// </summary>
    public void UnregisterEventHandlers()
    {
      Connection.OnUpdateHandler -= OnUpdate;
    }

    /// <summary>
    /// Unlike OnUpdate. this isn't called by the connection
    /// <br/>
    /// It's called by game 3 seconds after it receives the "start" event
    /// </summary>
    public void Start(double velX, double velY)
    {
      // Set the timestamp to the current time
      // This is so the ball doesn't start moving before the game starts
      LastServerTimestamp = DateTime.UtcNow.Ticks;

      VelX = velX;
      VelY = velY;
    }

    /// <summary>
    /// Event handler for the "update" event
    /// <br/>
    /// Updates the ball's position according to the server's position and velocity
    /// </summary>
    private void OnUpdate(object? _, Connection.UpdateEvent updateEvent)
    {
      // Set the timestamp to the current time
      LastServerTimestamp = DateTime.UtcNow.Ticks;

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
