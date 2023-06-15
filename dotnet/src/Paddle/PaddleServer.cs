namespace Pong
{
  public class PaddleServer : Paddle
  {
    public PaddleServer(GameServer game)
      : base(game.Scale, game.Offset) { }

    /// <summary>
    /// Handle the movement for a bot player
    /// </summary>
    /// <param name="ball">The ball to copy the velocity from</param>
    /// <param name="type">The type of game that is being played</param>
    public void BotMovement(Ball ball, GameServer.GameType type)
    {
      // Copy the velocity of the ball onto the right paddle and apply the multiplier
      // The multiplier is taken from the GameType enum. It's in percentages so it needs to be divided by 100
      VelY = ball.VelY * ((int)type / 100.0);

      // If the the ball is moving down and the top position of the paddle is below the top position of the ball, move the paddle up
      if (ball.VelY > 0 && Top > ball.Top)
        VelY *= -1;
      // If the the ball is moving up and the bottom position of the paddle is above the bottom position of the ball, move the paddle down
      else if (ball.VelY < 0 && Bottom < ball.Bottom)
        VelY *= -1;
    }
  }
}
