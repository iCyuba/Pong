namespace Pong
{
  public class PaddleClient : Paddle
  {
    public PaddleClient(GameClient game)
      : base(game.Scale, game.Offset) { }
  }
}
