namespace Pong
{
  public class PaddleClientRemote : PaddleClient
  {
    /// <summary>
    /// Used for the remote paddle, it listens for events from the game
    /// </summary>
    /// <param name="game">The game that the paddle is in</param>
    /// <param name="connection">The connection that the paddle is associated with</param>
    public PaddleClientRemote(GameClient game, Connection connection)
      : base(game, connection)
    {
      // Register the event handlers
      Connection.OnMoveHandler += MoveHandler;
    }

    public override void UnregisterEventHandlers()
    {
      // Unregister the event handlers
      Connection.OnMoveHandler -= MoveHandler;
    }

    /// <summary>
    /// Handle the move event from the game
    /// </summary>
    private void MoveHandler(object? _, Connection.MoveEvent moveEvent)
    {
      // Set the position and velocity
      PosY = moveEvent.Position;
      VelY = moveEvent.Velocity;
    }
  }
}
