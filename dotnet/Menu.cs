namespace Pong
{
  public partial class Menu : Form
  {
    private GameWindow GameWindow { get; set; }

    private Connection.Connection Connection { get; set; }

    public Menu()
    {
      InitializeComponent();

      // Prepare the game window
      GameWindow = new GameWindow(this);

      button3.Enabled = false;

      // Create the connection and connect to the server (idk why I made the autoConnect a thing, i'm not using it lmao)
      Connection = new("ws://localhost:3000", false);
      Connection
        .Connect()
        .ContinueWith(
          (task) =>
          {
            // Handle an exception if there is one
            if (task.Exception != null)
            {
              Console.WriteLine(task.Exception);
              return;
            }

            // Enable the multiplayer button
            Invoke(new Action(() => button3.Enabled = true));
          }
        );
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // Hide the menu window and show the game window
      Hide();

      GameWindow.Show();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      // Ask for the player's name
      string name = Microsoft.VisualBasic.Interaction.InputBox("What is your name?", "Name");

      // Register the player
      Connection
        .Register(name)
        .ContinueWith(
          (t) =>
          {
            // An error is thrown here when the connection fails so just ignore it
            if (t.Exception != null)
              return;

            // Hide the menu window and show the game window
            Invoke(
              new Action(() =>
              {
                Hide();

                GameWindow.Show();
              })
            );
          }
        );
    }
  }
}
