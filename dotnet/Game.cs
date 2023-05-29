namespace Pong
{
    public class Game
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsRunning { get; set; }

        public Heart Heart { get; set; }

        public Paddle LeftPaddle { get; set; }

        public Game(int width, int height)
        {
            Width = width;
            Height = height;

            IsRunning = false;

            Heart = new();
            LeftPaddle = new(Width / 8, Height / 2, 20, 100);
        }

        public void Start()
        {
            IsRunning = true;

            Heart.RandomlyStartMoving(600);

            Heart.SetPosToMiddle(Width, Height);
        }

        public void Draw(Graphics g, double deltaTime)
        {
            if (!IsRunning) return;

            Heart.Move(deltaTime);
            Heart.Bounce(Width, Height, LeftPaddle);
            Heart.Draw(g);

            LeftPaddle.Move(deltaTime, Height);
            LeftPaddle.Draw(g);
        }

        public void KeyDown(Keys key)
        {
            if (key == Keys.W) LeftPaddle.MoveUp();
            else if (key == Keys.S) LeftPaddle.MoveDown();
        }

        public void KeyUp(Keys key)
        {
            LeftPaddle.Stop();
        }
    }
}
