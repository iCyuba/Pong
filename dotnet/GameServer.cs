namespace Pong
{
  public class GameServer : Game
  {
    public GameServer(int width, int height)
      : base(width, height) { }

    /// <summary>
    /// Start the game (set IsRunning to true)
    /// </summary>
    public new void Start()
    {
      base.Start();

      Heart.RandomlyStartMoving(600);

      Heart.SetPosToMiddle(Width, Height);
    }

    /// <summary>
    /// Updates the position of objects in the game
    /// </summary>
    public new void Move(double deltaTime)
    {
      if (!IsRunning)
        return;

      // Call the base Move method
      base.Move(deltaTime);

      // Add the bounce logic
      Heart.Bounce(Width, Height, LeftPaddle);
    }

    /// <summary>
    /// This is the method that gets called when a key is pressed
    /// </summary>
    public void KeyDown(Keys key)
    {
      if (key == Keys.W)
        LeftPaddle.MoveUp();
      else if (key == Keys.S)
        LeftPaddle.MoveDown();
    }

    /// <summary>
    /// This is the method that gets called when a key is released
    /// </summary>
    public void KeyUp(Keys key)
    {
      LeftPaddle.Stop();
    }
  }
}
