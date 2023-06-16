namespace Pong
{
  public class GameClient : Game<BallClient, PaddleClient>
  {
    public override BallClient BallInstance { get; set; }
    public override PaddleClient LeftPaddle { get; set; }
    public override PaddleClient RightPaddle { get; set; }

    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// Indicates whether a ready event has been sent already
    /// <br/>
    /// This is to prevent spamming the server with ready events (although it doesn't really matter)
    /// </summary>
    private bool IsReady { get; set; } = false;

    /// <summary>
    /// Whether the game is invalid (the other player left)
    /// </summary>
    public bool IsInvalid { get; set; } = false;

    /// <summary>
    /// Create a new instance of a GameClient with the specified width, height and connection
    /// </summary>
    public GameClient(int width, int height, Connection connection)
      : base(width, height)
    {
      Connection = connection;

      // Initialize the ball with the provided connection
      BallInstance = new BallClient(connection, this);

      // Create the paddles
      LeftPaddle = new PaddleClientLocal(this, connection);
      RightPaddle = new PaddleClientRemote(this, connection);
      PositionPaddles();

      // Custom event handlers
      Connection.OnReadyHandler += OnReadyHandler;
      Connection.OnStartHandler += OnStartHandler;
      Connection.OnScoreHandler += OnScoreHandler;
      Connection.OnWinHandler += OnWinHandler;
      Connection.OnEndHandler += OnEndHandler;
    }

    public override void Start()
    {
      base.Start();

      if (LastStartEvent == null)
        throw new InvalidOperationException("LastStartEvent is null");

      // Start the ball
      BallInstance.Start(LastStartEvent.VelX, LastStartEvent.VelY);

      // Set last start event to null
      LastStartEvent = null;
    }

    /// <summary>
    /// Shows a message "waiting for the other player" when the we already sent a ready event
    /// </summary>
    private void OnReadyHandler(object? _, Connection.ReadyEvent e)
    {
      // If the game is already running, don't do anything
      if (IsRunning)
        return;

      // Check if the ready event was sent by us
      if (e.Name == Connection.Name)
      {
        OnShowTopMessage?.Invoke(this, "Ready!");
        OnShowBottomMessage?.Invoke(this, "Waiting for the other player...");
      }
      else if (!IsReady)
      {
        OnShowTopMessage?.Invoke(this, "The other player is ready!");
        OnShowBottomMessage?.Invoke(this, "Press any key to start!");
      }
    }

    /// <summary>
    /// This is the event message for the last "start" event
    /// <br/>
    /// I literally just need to store it somewhere so I can use it in the Start method
    /// </summary>
    private Connection.StartEvent? LastStartEvent { get; set; } = null;

    /// <summary>
    /// This is called when a server sends us a game start event
    /// <br/>
    /// This is the equivalent of <see cref="GameServer.StartIn3Seconds"/> but for the client. It can only be called by the server
    /// </summary>
    private void OnStartHandler(object? _, Connection.StartEvent e)
    {
      // Game will start at now + 3 seconds
      StartAt = DateTime.Now.AddSeconds(3);

      // Invoke the event
      OnStartIn?.Invoke(this, StartAt.Value);

      // Store the event
      LastStartEvent = e;
    }

    /// <summary>
    /// This is called when the ball goes out of bounds and the server sends us the new scores
    /// </summary>
    private void OnScoreHandler(object? _, Connection.ScoreEvent e)
    {
      // Update the score stored locally
      LeftScore = e.You;
      RightScore = e.Opponent;

      // Invoke the event
      OnScore?.Invoke(this, EventArgs.Empty);

      // Also stop the game
      IsRunning = false;
    }

    /// <summary>
    /// Called when a player scores 10 points
    /// </summary>
    private void OnWinHandler(object? _, Connection.WinEvent winEvent)
    {
      // Stop the game and set ready to false
      IsRunning = false;
      IsReady = false;

      // Set the messages
      OnShowTopMessage?.Invoke(this, $"{winEvent.Player} won!");
      OnShowBottomMessage?.Invoke(this, "Press any key to start!");

      // Set the scores to 0
      LeftScore = 0;
      RightScore = 0;

      // Invoke the event
      OnScore?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when a player leaves the game
    /// </summary>
    private void OnEndHandler(object? _, Connection.EndEvent endEvent)
    {
      // Stop the game and set ready to false
      IsRunning = false;
      IsInvalid = true;

      // Stop the timers
      OnStartIn?.Invoke(this, null);
      StartAt = null;

      // Set the messages
      OnShowTopMessage?.Invoke(this, $"The other player left the game!");
      OnShowBottomMessage?.Invoke(this, "Close the window to go back to the menu");
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

      LeftPaddle.UnregisterEventHandlers(); // this does nothing. but whatever. it looks better
      RightPaddle.UnregisterEventHandlers();

      Connection.OnReadyHandler -= OnReadyHandler;
      Connection.OnStartHandler -= OnStartHandler;
      Connection.OnScoreHandler -= OnScoreHandler;
      Connection.OnWinHandler -= OnWinHandler;
      Connection.OnEndHandler -= OnEndHandler;
    }

    public override void Update(double deltaTime)
    {
      // Run the base method
      base.Update(deltaTime);

      // Note: this can happen even with isRunning = false

      // Paddles
      LeftPaddle.Move(deltaTime);
      RightPaddle.Move(deltaTime);

      // Ball
      BallInstance.Move();
      BallInstance.Bounce();
    }

    public override void KeyDown(Keys key)
    {
      // Return if the game is invalid
      if (IsInvalid)
        return;

      // Send a ready event if the game is not running or about to start
      // don't do this if one has been sent already tho
      if (!IsRunning && StartAt == null && !IsReady)
      {
        _ = Connection.Ready();
        IsReady = true;
      }

      base.KeyDown(key);
    }

    public override void KeyUp(Keys key)
    {
      // Return if the game is invalid
      if (IsInvalid)
        return;

      base.KeyUp(key);
    }
  }
}
