using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsOOP3
{
    class MyShape
    {
        GraphicsPath gp = new GraphicsPath();
        Pen pen = new Pen(Color.Black);
        public MyShape(RectangleF rectangle, Color color)
        {
            pen.Color = color;

            PointF a = new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y);
            PointF b = new PointF(rectangle.X,rectangle.Y + rectangle.Height);
            PointF c = new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);

            gp.AddPolygon(new PointF[] { a, b, c });
        }

        public void Draw(Graphics g)
        {
            g.DrawPath(pen,gp);
        }
    }
}
