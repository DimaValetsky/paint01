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
    class Arrow : Shape
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
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(start.X, start.Y + this.Height / 4);
            pathFigure.IsClosed = true;


            LineSegment ls1 = new LineSegment();
            ls1.Point = new Point(start.X, start.Y + this.Height * 3 / 4);
            LineSegment ls2 = new LineSegment();
            ls2.Point = new Point(ls1.Point.X + this.Width / 2, ls1.Point.Y);
            LineSegment ls3 = new LineSegment();
            ls3.Point = new Point(ls2.Point.X, ls2.Point.Y + this.Height / 4);
            LineSegment ls4 = new LineSegment();
            ls4.Point = new Point(ls3.Point.X + this.Width / 2, ls3.Point.Y - this.Height / 2);
            LineSegment ls5 = new LineSegment();
            ls5.Point = new Point(ls4.Point.X - this.Width / 2, ls4.Point.Y - this.Height / 2);
            LineSegment ls6 = new LineSegment();
            ls6.Point = new Point(ls5.Point.X, ls5.Point.Y + this.Height / 4);

            pathFigure.Segments.Add(ls1);
            pathFigure.Segments.Add(ls2);
            pathFigure.Segments.Add(ls3);
            pathFigure.Segments.Add(ls4);
            pathFigure.Segments.Add(ls5);
            pathFigure.Segments.Add(ls6);

            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }
    }

}
