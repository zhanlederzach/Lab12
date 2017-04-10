using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsOOP3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue };

            Graphics g = pictureBox1.CreateGraphics();
            for (int i = 0; i < 5; ++i)
            {
                int x = r.Next(0, pictureBox1.Width);
                int y = r.Next(0, pictureBox1.Height);
                int ci = r.Next(0, colors.Length);
                MyShape ms = new MyShape(new RectangleF(x, y, 30, 30), colors[ci]);
                ms.Draw(g);
            }
        }
    }
}
