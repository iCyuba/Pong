namespace Pong
{
  public partial class GameWindow : Form
  {
    public Game GameInstance { get; set; }

    private Menu MainMenu { get; set; }

    public GameWindow(Menu menu)
    {
      InitializeComponent();

      // Initialize the game instance
      GameInstance = new(pictureBox.Width, pictureBox.Height);

      // Save the main menu for later
      MainMenu = menu;
    }

    private void GameWindow_Load(object sender, EventArgs e)
    {
      GameInstance.Start();
    }

    private void pictureBox_Paint(object sender, PaintEventArgs e)
    {
      GameInstance.Draw(e.Graphics, timer1.Interval / 1000.0);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      pictureBox.Refresh();
    }

    private void GameWindow_KeyDown(object sender, KeyEventArgs e)
    {
      GameInstance.KeyDown(e.KeyCode);
    }

    private void GameWindow_KeyUp(object sender, KeyEventArgs e)
    {
      GameInstance.KeyUp(e.KeyCode);
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);

      // When you close the game window, the main menu should also get closed
      MainMenu.Close();
    }
  }
}
