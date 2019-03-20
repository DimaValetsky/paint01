using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace MyPaint01
{
    class Heart : Shape
    {

        private Point start;
        public Point Start { get; set; }
        protected override Geometry DefiningGeometry
        {
            get { return GenerateTriangleGeometry(); }
        }

        private Geometry GenerateTriangleGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure1 = new PathFigure();
            pathFigure1.StartPoint = new Point(start.X + this.Width / 2, start.Y + this.Height / 3);
            pathFigure1.IsClosed = false;

            BezierSegment bs1 = new BezierSegment();
            bs1.Point1 = new Point(start.X + this.Width * 3 / 4, start.Y);
            bs1.Point2 = new Point(start.X + this.Width, start.Y + this.Height / 3);
            bs1.Point3 = new Point(start.X + this.Width / 2, start.Y + this.Height);
            pathFigure1.Segments.Add(bs1);

            PathFigure pathFigure2 = new PathFigure();
            pathFigure2.StartPoint = new Point(start.X + this.Width / 2, start.Y + this.Height / 3);
            pathFigure1.IsClosed = false;

            BezierSegment bs2 = new BezierSegment();
            bs2.Point1 = new Point(start.X + this.Width * 1 / 4, start.Y);
            bs2.Point2 = new Point(start.X, start.Y + this.Height / 3);
            bs2.Point3 = new Point(start.X + this.Width / 2, start.Y + this.Height);
            pathFigure2.Segments.Add(bs2);

            pathGeometry.Figures.Add(pathFigure1);
            pathGeometry.Figures.Add(pathFigure2);
            return pathGeometry;
        }
    }
}
