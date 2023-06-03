namespace Pong
{
  public partial class Menu : Form
  {
    private Connection Connection { get; set; }

    public Menu()
    {
      InitializeComponent();

      // Disable the multiplayer button until the connection is established
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
      // Create a new game window
      GameWindow GameWindow = new GameWindow(this);

      // Add an event listener for when the game window is closed so we can show the main menu again
      GameWindow.Closed += (_, __) =>
      {
        // Change the main menu's location to the same as the game window
        Location = GameWindow.Location;

        // Show the main menu again
        Show();
      };

      // Hide the main menu and show the game window
      Hide();
      GameWindow.Show();

      // Set the game window's position to the same as the main menu
      GameWindow.Location = Location;
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

                // GameWindow.Show();
              })
            );
          }
        );
    }
  }
}
