namespace Pong
{
  public class BallServer : Ball
  {
    /// <summary>
    /// Move the ball according to its velocity and the time passed
    /// </summary>
    public void Move(double deltaTime)
    {
      PosX += VelX * deltaTime;
      PosY += VelY * deltaTime;
    }

    /// <summary>
    /// Set the position of the ball to the middle of the screen
    /// </summary>
    public void SetPosToMiddle(int width, int height)
    {
      PosX = width / 2;
      PosY = height / 2;
    }

    /// <summary>
    /// Randomly start moving the ball in a random direction
    /// </summary>
    public void RandomlyStartMoving(int vel)
    {
      Random r = new();

      int angleInDegrees = r.Next(0, 360);
      double angleInRadians = (angleInDegrees / 180.0) * Math.PI;

      VelX = Math.Cos(angleInRadians) * vel;
      VelY = Math.Sin(angleInRadians) * vel;
    }

    public bool CheckHColission(int width)
    {
      return PosX <= Diameter / 2 || PosX >= width - (Diameter / 2 * 3);
    }

    public bool CheckVColission(int height)
    {
      return PosY <= Diameter / 2 || PosY >= height - (Diameter / 2 * 3);
    }

    public bool CheckPaddleHColission(Paddle paddle)
    {
      double x1 = paddle.PosX;
      double x2 = x1 + paddle.Width;

      double y1 = paddle.PosY;
      double y2 = y1 + paddle.Height;

      // if we're under it / above it. just return false now
      if (PosY > y2 || PosY < y1)
        return false;

      return PosX <= x2 && PosX >= x1;
    }

    public void Bounce(int width, int height, Paddle paddle)
    {
      if (CheckHColission(width))
        VelX *= -1;
      if (CheckVColission(height))
        VelY *= -1;
      if (CheckPaddleHColission(paddle))
        VelX *= -1;
    }
  }
}
