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
    class Triangle : Shape
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
            pathFigure.StartPoint = start;
            pathFigure.IsClosed = true;

            LineSegment ls1 = new LineSegment();
            ls1.Point = new Point(start.X, start.Y + this.Height);
            pathFigure.Segments.Add(ls1);
            LineSegment ls2 = new LineSegment();
            ls2.Point = new Point(start.X + this.Width, start.Y + this.Height);
            pathFigure.Segments.Add(ls2);
            pathGeometry.Figures.Add(pathFigure);

            //LineSegment ls1 = new LineSegment();
            //ls1.Point = new Point(100, 100);
            //pathFigure.Segments.Add(ls1);
            //LineSegment ls2 = new LineSegment();
            //ls2.Point = new Point(100, 50);
            //pathFigure.Segments.Add(ls2);
            //pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }
    }
}
