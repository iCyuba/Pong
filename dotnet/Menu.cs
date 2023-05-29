using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class Menu : Form
    {
        GameWindow GameWindow;

        public Menu()
        {
            InitializeComponent();

            // Prepare the game window
            GameWindow = new GameWindow(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Hide the menu window and show the game window
            Hide();

            GameWindow.Show();
        }
    }
}
