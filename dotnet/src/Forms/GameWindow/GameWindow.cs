namespace Pong
{
  public abstract partial class GameWindow<GameType, BallType, PaddleType> : Form
    // Look, i don't know what this is. I give up the generics. Typescript is better with this. Why can't I do <T extends Game = Game>?????
    where GameType : Game<BallType, PaddleType>
    where BallType : Ball
    where PaddleType : Paddle
  {
    /// <summary>
    /// The game that is being played
    /// </summary>
    public abstract GameType GameInstance { get; set; }

    /// <summary>
    /// A timeout to show on screen
    /// </summary>
    private DateTime? Timeout { get; set; }

    /// <summary>
    /// Create a new game window (abstract)
    /// </summary>
    public GameWindow()
    {
      InitializeComponent();

      // The text that is visible when the game isn't running
      topMessage.Text = "Press any key to start!";
      bottomMessage.Visible = false;

      // Don't forget to register events in the constructor in the extended classes
      // Can't do it here cuz GameInstance is null here
    }

    protected void RegisterEvents()
    {
      // Register event handlers
      GameInstance.OnShowTopMessage += OnShowTopMessage;
      GameInstance.OnHideTopMessage += OnHideTopMessage;
      GameInstance.OnShowBottomMessage += OnShowBottomMessage;
      GameInstance.OnHideBottomMessage += OnHideBottomMessage;
      GameInstance.OnStartIn += OnStartIn;
      GameInstance.OnStart += OnStart;
      GameInstance.OnScore += OnScore;
    }

    /// <summary>
    /// This is called to show some message on the top
    /// </summary>
    private void OnShowTopMessage(object? sender, string message)
    {
      this.topMessage.Text = message;
      this.topMessage.Visible = true;
    }

    /// <summary>
    /// This is called to hide the message on the bottom
    /// </summary>
    private void OnHideTopMessage(object? sender, EventArgs e)
    {
      topMessage.Visible = false;
    }

    /// <summary>
    /// This is called to show some message on the top
    /// </summary>
    private void OnShowBottomMessage(object? sender, string message)
    {
      this.bottomMessage.Text = message;
      this.bottomMessage.Visible = true;
    }

    /// <summary>
    /// This is called to hide the message on the bottom
    /// </summary>
    private void OnHideBottomMessage(object? sender, EventArgs e)
    {
      bottomMessage.Visible = false;
    }

    /// <summary>
    /// This is called when the game will start in some time
    /// </summary>
    private void OnStartIn(object? sender, DateTime? time)
    {
      // Set the timeout
      Timeout = time;

      // Update the message
      UpdateTimeoutMessage();
    }

    /// <summary>
    /// This is called when the game will starts
    /// </summary>
    private void OnStart(object? sender, EventArgs e)
    {
      // Set the timeout to null
      Timeout = null;

      // Remove all messages
      topMessage.Visible = false;
      bottomMessage.Visible = false;
    }

    /// <summary>
    /// Update the timeout message every second
    /// </summary>
    private void UpdateTimeoutMessage()
    {
      // Return if the timeout is null
      if (Timeout == null)
        return;

      topMessage.Visible = true;
      bottomMessage.Visible = false;

      // Update the message
      topMessage.Text =
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
