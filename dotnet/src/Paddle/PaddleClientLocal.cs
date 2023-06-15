namespace Pong
{
  public class PaddleClientLocal : PaddleClient
  {
    /// <summary>
    /// Used for the local paddle, sends the appropriate events to the game
    /// </summary>
    /// <param name="game">The game that the paddle is in</param>
    /// <param name="connection">The connection that the paddle is associated with</param>
    public PaddleClientLocal(GameClient game, Connection connection)
      : base(game, connection) { }

    // The Local paddle doesn't need to listen for events from the game
    public override void UnregisterEventHandlers() { }

    /// <summary>
    /// Send the move events when the paddle is changing velocity
    /// </summary>
    public override void MoveDown()
    {
      double previousVelY = VelY;

      base.MoveDown();

      // Send the event if the velocity changed
      if (previousVelY == VelY)
        return;
      var _ = Connection.Move(this);
    }

    /// <summary>
    /// Send the move events when the paddle is changing velocity
    /// </summary>
    public override void MoveUp()
    {
      double previousVelY = VelY;

      base.MoveUp();

      // Send the event if the velocity changed
      if (previousVelY == VelY)
        return;
      var _ = Connection.Move(this);
    }
  }
}
