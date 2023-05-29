namespace Pong
{
    public partial class Form1 : Form
    {
        public Game GameInstance { get; set; }

        public Form1()
        {
            InitializeComponent();
            GameInstance = new(pictureBox.Width, pictureBox.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameInstance.Start();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameInstance.Draw(e.Graphics, timer1.Interval / 1000.0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameInstance.KeyDown(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            GameInstance.KeyUp(e.KeyCode);
        }
    }
}