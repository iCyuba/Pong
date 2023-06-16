namespace Pong
{
  public partial class GameClientWindow : GameWindow<GameClient, BallClient, PaddleClient>
  {
    /// <summary>
    /// The connection associated with this window
    /// </summary>
    private Connection Connection { get; set; }

    public override GameClient GameInstance { get; set; }

    /// <summary>
    /// Create a new game (client) window
    /// </summary>
    /// <param name="connection">The connection to use</param>
    public GameClientWindow(Connection connection)
      : base()
    {
      Connection = connection;

      // Initialize the game instance with the connection
      GameInstance = new GameClient(pictureBox.Width, pictureBox.Height, connection);

      // Register the events
      RegisterEventHandlers();
    }

    public override void RegisterEventHandlers()
    {
      base.RegisterEventHandlers();

      FormClosed += OnClosed;
    }

    public override void UnregisterEventHandlers(object? sender = null, EventArgs? e = null)
    {
      base.UnregisterEventHandlers(sender, e);

      FormClosed -= OnClosed;
    }

    /// <summary>
    /// Send an end event when the window is closed
    /// </summary>
    private void OnClosed(object? sender, FormClosedEventArgs e)
    {
      // idc about any response
      var _ = Connection.End();
    }
  }
}
