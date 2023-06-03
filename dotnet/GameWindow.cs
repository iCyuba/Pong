namespace Pong
{
  public partial class GameWindow : Form
  {
    public GameServer GameInstance { get; set; }
    private Menu MainMenu { get; set; }

    public GameWindow(Menu menu)
    {
      InitializeComponent();

      // Initialize the game instance
      GameInstance = new(pictureBox.Width, pictureBox.Height);

      // Save the main menu for later
      MainMenu = menu;
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
  }
}
