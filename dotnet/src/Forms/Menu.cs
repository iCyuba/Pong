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
          (t) =>
          {
            // Enable the multiplayer button if the connection is successful
            if (t.Status == TaskStatus.RanToCompletion)
              Invoke(new Action(() => button3.Enabled = true));
          }
        );
    }

    /// <summary>
    /// Shows a form and hides the main menu
    /// </summary>
    /// <param name="form">The form to show</param>
    public void ShowForm(Form form)
    {
      // Add an event listener for when the form is closed so we can show the main menu again
      form.Closed += (_, __) =>
      {
        // Change the main menu's location to the same as the form
        Location = form.Location;

        // Show the main menu again
        Show();
      };

      // Hide the main menu and show the form
      Hide();
      form.Show();

      // Set the form's position to the same as the main menu
      form.Location = Location;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // Create a new game window and show it
      GameWindow GameWindow = new();
      ShowForm(GameWindow);
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
            // Hide the menu window and show the players window
            Invoke(
              new Action(() =>
              {
                // Show the players window
                Players Players = new(Connection);
                ShowForm(Players);
              })
            );
          }
        );
    }
  }
}
