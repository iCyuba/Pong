using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class Paddle
    {
        public double PosX { get; set; }
        public double PosY { get; set; }

        public double VelY { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Paddle(double posX, double posY, int width, int height)
        {
            PosX = posX;
            PosY = posY;
            Width = width;
            Height = height;

            VelY = 0;
        }

        public void Move(double deltaTime, int height)
        {
            double newPos = PosY + VelY * deltaTime;
            if (newPos >= height - Height || newPos < 0) return;

            PosY = newPos;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.DeepPink, (float) PosX, (float) PosY, Width, Height);
        }

        public void MoveUp()
        {
            VelY = -500;
        }

        public void MoveDown()
        {
            VelY = 500;
        }

        public void Stop()
        {
            VelY = 0;
        }
    }
}
