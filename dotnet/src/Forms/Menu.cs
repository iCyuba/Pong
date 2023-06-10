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
    /// Replaces one form with another. When the new form is closed, the old form is shown again
    /// </summary>
    /// <param name="hide">The form to hide</param>
    /// <param name="show">The form to show</param>
    public static void ShowForm(Form hide, Form show)
    {
      // Add an event listener for when the form is closed so we can show the main menu again
      show.Closed += (_, __) =>
      {
        // Change the main menu's location to the same as the form
        hide.Location = show.Location;

        // Show the main menu again
        hide.Show();
      };

      // Hide the main menu and show the form
      hide.Hide();
      show.Show();

      // Set the form's position to the same as the main menu
      show.Location = hide.Location;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // Create a new game window and show it
      GameWindow GameWindow = new();
      ShowForm(this, GameWindow);
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
                ShowForm(this, Players);
              })
            );
          }
        );
    }
  }
}
