namespace Pong
{
  public partial class GameServerWindow : GameWindow<GameServer, BallServer, PaddleServer>
  {
    public override GameServer GameInstance { get; set; }

    /// <summary>
    /// Create a new game (server) window with the specified type
    /// </summary>
    /// <param name="type">The game type to use</param>
    public GameServerWindow(GameServer.GameType type)
      : base()
    {
      // Initialize the game instance with the specified type
      GameInstance = new GameServer(pictureBox.Width, pictureBox.Height, type);

      // Register the events
      RegisterEventHandlers();
    }
  }
}
