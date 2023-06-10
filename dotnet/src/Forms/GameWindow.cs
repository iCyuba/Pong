namespace Pong
{
  public partial class GameWindow : Form
  {
    public Game GameInstance { get; set; }

    public GameWindow(Game? game = null)
    {
      InitializeComponent();

      // If the game is null, then create a new game (server)
      game ??= new GameServer(pictureBox.Width, pictureBox.Height);

      // Set the game instance
      GameInstance = game;
    }

    /// <summary>
    /// This is the method that gets called when the game window is loaded
    /// <br />
    /// It probably shouldn't start the game. But it does...
    /// </summary>
    private void OnLoad(object sender, EventArgs e)
    {
      GameInstance.Start();
    }

    /// <summary>
    /// This is the that renders the game (notice that it isn't updating the game, just rendering it)
    /// </summary>
    private void OnPaint(object sender, PaintEventArgs e)
    {
      GameInstance.Draw(e.Graphics, timer1.Interval / 1000.0);
    }

    /// <summary>
    /// This is the method that gets called every tick (every 50ms [20 times per second])
    /// <br />
    /// It's used to update the game
    /// </summary>
    private void OnTick(object sender, EventArgs e)
    {
      // Update the game
      GameInstance.Move(timer1.Interval / 1000.0);

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
