namespace Pong
{
  public abstract class PaddleClient : Paddle
  {
    /// <summary>
    /// The connection that the paddle is associated with
    /// </summary>
    public Connection Connection { get; set; }

    /// <summary>
    /// Create a new instance of a PaddleClient in the middle of the screen and scale it to an appropriate size
    /// (It's created at X=0 intentionally.. I use the setters <see cref="Box.Left"/> and <see cref="Box.Right"/> to set the position)
    /// </summary>
    /// <param name="game">The game that the paddle is in</param>
    /// <param name="connection">The connection that the paddle is associated with</param>
    public PaddleClient(GameClient game, Connection connection)
      : base(game.Scale, game.Offset)
    {
      Connection = connection;
    }

    /// <summary>
    /// Unregister any registered event handlers
    /// </summary>
    public abstract void UnregisterEventHandlers();
  }
}
