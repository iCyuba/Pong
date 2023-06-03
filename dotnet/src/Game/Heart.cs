using System.Security.Cryptography.X509Certificates;

namespace Pong
{
  public class Heart
  {
    public double PosX { get; set; }
    public double PosY { get; set; }

    // Short for velocity...
    public double VelX { get; set; }
    public double VelY { get; set; }

    public double Diameter { get; set; }

    public Heart()
    {
      PosX = 0;
      PosY = 0;
      VelX = 0;
      VelY = 0;
      Diameter = 25;
    }

    public void Move(double deltaTime)
    {
      PosX += VelX * deltaTime;
      PosY += VelY * deltaTime;
    }

    public void Draw(Graphics g)
    {
      double x = PosX - (Diameter / 2);
      double y = PosY - (Diameter / 2);

      Font f = new("Segoe MDL2 Assets", (float)Diameter);

      g.FillRectangle(Brushes.LightPink, (float)x, (float)y, (float)Diameter, (float)Diameter);
      g.DrawString("", f, Brushes.HotPink, (float)x, (float)y);
    }

    public void SetPosToMiddle(int width, int height)
    {
      PosX = width / 2;
      PosY = height / 2;
    }

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

    private void HeartToCircleOffset()
    {
      // The heart doesn't render from the middle so this hopefully fixes it
    }
  }
}
