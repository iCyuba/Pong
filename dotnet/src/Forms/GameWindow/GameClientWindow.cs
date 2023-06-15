namespace Pong
{
  public partial class GameClientWindow : GameWindow<GameClient, BallClient, PaddleClient>
  {
    public override GameClient GameInstance { get; set; }

    /// <summary>
    /// Create a new game (client) window
    /// </summary>
    /// <param name="connection">The connection to use</param>
    public GameClientWindow(Connection connection)
      : base()
    {
      // Initialize the game instance with the connection
      GameInstance = new GameClient(pictureBox.Width, pictureBox.Height, connection);

      // Register the events
      RegisterEvents();
    }
  }
}
