using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintPencil

{
    public partial class Form1 : Form
    {
        Color prevColor = Color.Red;// сначала красный

        Point prevPoint;
        Point currentPoint;

        Shapes currentShape = Shapes.Free;// по умолчанию карандаш

        GraphicsPath gp = new GraphicsPath();
        Graphics g;
        Bitmap bmp;

        Color colorOrigin;
        Color colorFill;

        public GraphicsPath gpp;

        public Pen pen { get; private set; }

        public Form1()
        {
            InitializeComponent();
            Initalization();
        }
        
        private void Initalization(Bitmap tmpBmp = null)
        {
            pen = new Pen(prevColor, 3);
            if (tmpBmp == null)
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            else
                bmp = tmpBmp;
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(pictureBox1.Image);
        }

        Queue<Point> q = new Queue<Point>();

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prevPoint = e.Location;
            switch (currentShape)
            {
                case Shapes.Fill:
                    F1(e.Location);
                    break;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (currentShape)
                {
                    case Shapes.Free:
                        currentPoint = e.Location;
                        g.DrawLine(pen, prevPoint, currentPoint);
                        //gp.AddLine(prevPoint, currentPoint);

                        prevPoint = currentPoint;
                        break;
                    case Shapes.Line:
                        currentPoint = e.Location;
                        gp.Reset();
                        gp.AddLine(prevPoint, currentPoint);
                        break;
                    case Shapes.Ellipse:
                        currentPoint = e.Location;
                        gp.Reset();
                        gp.AddEllipse(prevPoint.X, prevPoint.Y, 
                                      currentPoint.X - prevPoint.X, 
                                      currentPoint.Y - prevPoint.Y);

                        break;
                    case Shapes.Rectangle:
                        currentPoint = e.Location;
                        gp.Reset();
                        Point[] pt2 = { new Point(prevPoint.X, prevPoint.Y),
                                       new Point(prevPoint.X, currentPoint.Y),
                                new Point(currentPoint.X, currentPoint.Y),
                                new Point(currentPoint.X, prevPoint.Y)};
                        gp.AddPolygon(pt2);
                        //g.DrawPolygon(pen, pt2);
                        break;
                    case Shapes.Triangle:
                        currentPoint = e.Location;
                        gp.Reset();
                        Point[] pt = { new Point(prevPoint.X, prevPoint.Y),
                                       new Point(currentPoint.X, currentPoint.Y),
                                       new Point(currentPoint.X-2*(currentPoint.X-prevPoint.X), currentPoint.Y) };
                        gp.AddPolygon(pt);
                        break;
                    case Shapes.Fill:
                        break;
                    default:
                        break;
                }
            }
            mouseLocationLabel.Text = string.Format("X = {0}; Y = {1}", e.X, e.Y);
            pictureBox1.Refresh();
        }

        private void F1(Point point)
        {
            q.Enqueue(point);//в очередь добавляем точку
            colorOrigin = bmp.GetPixel(point.X, point.Y);//берет цвет пикселя этой точки
            colorFill = pen.Color;//цвет заливки это цвет ручки
            Fill();
        }
        
        private void Fill()
        {
            while (q.Count > 0)
            {
                Point curPoint = q.Dequeue();//берем ее из очереди
                Step(curPoint.X + 1, curPoint.Y);//
                Step(curPoint.X - 1, curPoint.Y);//
                Step(curPoint.X, curPoint.Y + 1);//     берем точки со всех сторон
                Step(curPoint.X, curPoint.Y - 1);//
            }
            pictureBox1.Refresh();
        }

        private void Step(int x, int y)
        {
            if (x < 0) return;
            if (y < 0) return;
            if (x >= bmp.Width) return;
            if (y >= bmp.Height) return;
            if (bmp.GetPixel(x, y) != colorOrigin) return;  
            bmp.SetPixel(x, y, colorFill); //закрашивает этот пиксель цветом заливки
            q.Enqueue(new Point(x, y));// добавляет в очередь эту точку
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            g.DrawPath(pen, gp);
            gp.Reset();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                Bitmap tmpBmp = new Bitmap(Image.FromFile(file.FileName), new Size(300, 300));// битмап с файла
                bmp = new Bitmap(Width, Height);// наш битмап - пустой
                
                using (Graphics g = Graphics.FromImage(bmp)) 
                // создаем графикс на нашем битмапе, чтобы на нем нарисовать
                {
                    g.DrawImage(tmpBmp, new Point(0, 0));// рисуем на нашем битмапе тот битмап с 0,0 
                }
            }
            Initalization(bmp);
            

            /*
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap newimage = new Bitmap(ofd.FileName);
                Bitmap cloneimage = newimage.Clone() as Bitmap;
                newimage.Dispose();// не используем
                pictureBox1.Image = cloneimage;
                g = Graphics.FromImage(pictureBox1.Image);
            }
            */
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {   
            /*
            Bitmap btm = bmp;
            SaveFileDialog safe = new SaveFileDialog();
            if (safe.ShowDialog() == DialogResult.OK)
                btm.Save(safe.FileName);
            */
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(sfd.FileName);
            }
        }

        /*
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pen = new Pen(prevColor, 3 * comboBox1.SelectedIndex);
            //Console.WriteLine(sender.ToString());
            //Console.WriteLine(e.ToString());
        }
        */

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pen = new Pen(prevColor, 2 * trackBar1.Value);
        }

        private void colorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                prevColor = c.Color;
                pen = new Pen(prevColor, pen.Width);
            }
        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            currentShape = Shapes.Line;
        }

        private void freeBtn_Click(object sender, EventArgs e)
        {
            currentShape = Shapes.Free;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawPath(pen, gp);
        }

        private void triangleBtn_click(object sender, EventArgs e)
        {
            currentShape = Shapes.Triangle;
        }

        private void rectBtn_Click(object sender, EventArgs e)
        {
            currentShape = Shapes.Rectangle;

        }

        private void ellipseBtn_Click(object sender, EventArgs e)
        {
            currentShape = Shapes.Ellipse;
        }

        private void Fill_Click(object sender, EventArgs e)
        {
            currentShape = Shapes.Fill;
        }
    }


    public enum Shapes
    {
        Free,
        Line,
        Ellipse,
        Rectangle,
        Triangle,
        Fill
    }
}
