namespace Pong
{
  public partial class GameWindow : Form
  {
    /// <summary>
    /// The game that is being played
    /// </summary>
    public Game GameInstance { get; set; }

    /// <summary>
    /// A timeout to show on screen
    /// </summary>
    private DateTime? Timeout { get; set; }

    /// <summary>
    /// Create a new game window
    /// <br/>
    /// If connection is null, then the game is a server. Otherwise, it's a client
    /// </summary>
    /// <param name="connection">The connection to use if the game is multiplayer</param>
    /// <param name="type">The game type to use, note: This is ignored if connection is provided</param>
    public GameWindow(Connection? connection = null, GameServer.GameType? type = null)
    {
      InitializeComponent();

      // Create a new game
      // If the connection is null, then the game is a server. Otherwise, it's a client
      if (connection == null)
        GameInstance = new GameServer(
          pictureBox.Width,
          pictureBox.Height,
          // If the type is null, then use the default type (Local)
          type ?? GameServer.GameType.Local
        );
      else
        GameInstance = new GameClient(pictureBox.Width, pictureBox.Height, connection);

      // The text that is visible when the game isn't running
      message.Text = "Press any key to start!";

      // Register event handlers
      GameInstance.OnStart += OnStart;
      GameInstance.OnStartIn += OnStartIn;
      GameInstance.OnScore += OnScore;
    }

    /// <summary>
    /// This is called when the game starts. It just hides the message
    /// </summary>
    private void OnStart(object? sender, EventArgs e)
    {
      message.Visible = false;
    }

    /// <summary>
    /// This is called when the game will start in some time
    /// </summary>
    private void OnStartIn(object? sender, DateTime time)
    {
      message.Visible = true;

      // Set the timeout
      Timeout = time;

      // Update the message
      UpdateTimeoutMessage();
    }

    /// <summary>
    /// Update the timeout message every second
    /// </summary>
    private void UpdateTimeoutMessage()
    {
      // Return if the timeout is null
      if (Timeout == null)
        return;

      // Update the message
      message.Text =
        $"Starting in {Math.Ceiling((Timeout.Value - DateTime.Now).TotalSeconds)} seconds!";
    }

    /// <summary>
    /// Update the scores text when the score changes
    /// </summary>
    private void OnScore(object? sender, EventArgs e)
    {
      // Left score
      scoreLeft.Text = GameInstance.LeftScore.ToString();

      // Right score
      scoreRight.Text = GameInstance.RightScore.ToString();
    }

    /// <summary>
    /// This is the that renders the game (notice that it isn't updating the game, just rendering it)
    /// </summary>
    private void OnPaint(object sender, PaintEventArgs e)
    {
      GameInstance.Draw(e.Graphics);
    }

    /// <summary>
    /// This is the method that gets called every tick (every 50ms [20 times per second])
    /// <br />
    /// It's used to update the game
    /// </summary>
    private void OnTick(object sender, EventArgs e)
    {
      // Update the timeout message
      UpdateTimeoutMessage();

      // Update the game
      GameInstance.Update(timer1.Interval / 1000.0);

      // Render the game
      pictureBox.Refresh();
    }

    /// <summary>
    /// This is the method that gets called when a key is pressed
    /// <br />
    /// It's used to move the paddles
    /// </summary>
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      GameInstance.KeyDown(e.KeyCode);
    }

    /// <summary>
    /// This is the method that gets called when a key is released
    /// <br />
    /// It's used to stop the paddles
    /// </summary>
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
      GameInstance.KeyUp(e.KeyCode);
    }

    private void OnClose(object sender, FormClosedEventArgs e)
    {
      // If the game is a client, unbind the events
      if (GameInstance is GameClient GameClientInstance)
      {
        GameClientInstance.UnregisterEventHandlers();
      }
    }
  }
}
