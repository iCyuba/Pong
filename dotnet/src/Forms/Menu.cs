namespace Pong
{
  public partial class Menu : Form
  {
    /// <summary>
    /// The connection to the server
    /// </summary>
    private Connection Connection { get; set; }

    /// <summary>
    /// The game that is shown in the background
    /// </summary>
    private Game BackgroundGame { get; set; }

    public Menu()
    {
      InitializeComponent();

      // Create a new game
      BackgroundGame = new GameFake(backgroundGame.Width, backgroundGame.Height);

      // Disable the multiplayer button until the connection is established
      multiplayerOnline.Enabled = false;

      // Create the connection and connect to the server (idk why I made the autoConnect a thing, i'm not using it lmao)
      Connection = new("ws://localhost:3000", false);
      Connection
        .Connect()
        .ContinueWith(
          (t) =>
          {
            // Enable the multiplayer button if the connection is successful
            if (t.Status == TaskStatus.RanToCompletion)
              Invoke(new Action(() => multiplayerOnline.Enabled = true));
          }
        );

      // Register the error event handler
      Connection.OnErrorHandler += OnError;
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

    /// <summary>
    /// Creates a new GameServer. This is used by the 4 methods below
    /// </summary>
    /// <param name="type">The type of game to create</param>
    private void CreateGameServer(GameServer.GameType type)
    {
      // Create a new game window and show it
      GameWindow GameWindow = new(null, type);
      ShowForm(this, GameWindow);
    }

    /// <summary>
    /// Called when the Bot - Easy button is clicked. Creates a new GameServer with the type BotEasy
    /// </summary>
    private void BotEasyClick(object sender, EventArgs e) =>
      CreateGameServer(GameServer.GameType.BotEasy);

    /// <summary>
    /// Called when the Bot - Medium button is clicked. Creates a new GameServer with the type BotMedium
    /// </summary>
    private void BotMediumClick(object sender, EventArgs e) =>
      CreateGameServer(GameServer.GameType.BotMedium);

    /// <summary>
    /// Called when the Bot - Hard button is clicked. Creates a new GameServer with the type BotHard
    /// </summary>
    private void BotHardClick(object sender, EventArgs e) =>
      CreateGameServer(GameServer.GameType.BotHard);

    /// <summary>
    /// Called when the Local Multiplayer button is clicked. Creates a new GameServer with the type Local
    /// </summary>
    private void LocalMultiplayerClick(object sender, EventArgs e) =>
      CreateGameServer(GameServer.GameType.Local);

    /// <summary>
    /// Called when the Online Multiplayer button is clicked. Creates a new GameServer with the type Online
    /// </summary>
    private void OnlineMultiplayerClick(object sender, EventArgs e)
    {
      // Ask for the player's name
      string name = Microsoft.VisualBasic.Interaction.InputBox("What is your name?", "Name");

      // Register the player
      var _ = Connection.Register(name);

      // Once the registration is complete, open the players window
      Connection.OnRegisterHandler += OnRegister;
    }

    /// <summary>
    /// Called when the server throws an error
    /// </summary>
    private void OnError(object? _, Connection.ErrorEvent e)
    {
      MessageBox.Show(e.Message, "Server error");

      // Unregister the registration event handler if it was registered before
      Connection.OnRegisterHandler -= OnRegister;
    }

    /// <summary>
    /// Called when the player is registered, used for opening the players window
    /// </summary>
    private void OnRegister(object? _, Connection.RegisterEvent registerEvent)
    {
      if (registerEvent.Name == Connection.Name)
      {
        Players Players = new(Connection);
        ShowForm(this, Players);

        // Unregister the event handler
        Connection.OnRegisterHandler -= OnRegister;
      }
    }

    /// <summary>
    /// Called when the backgroundGame should be painted
    /// </summary>
    private void BackgroundGamePaint(object sender, PaintEventArgs e) =>
      BackgroundGame.Draw(e.Graphics);

    /// <summary>
    /// Called when the backgroundGame ticks
    /// </summary>
    private void BackgroundGameTick(object sender, EventArgs e)
    {
      // Update the game
      BackgroundGame.Update(backgroundGameTimer.Interval / 1000.0);

      // Render the game
      backgroundGame.Refresh();
    }

    /// <summary>
    /// Activates / deactivates the background game updates. To save resources...
    /// </summary>
    private void MenuVisibilityChanged(object sender, EventArgs e)
    {
      backgroundGameTimer.Enabled = Visible;
    }
  }
}
