using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1
{
    public class Math
    {
        public double Length(Point p1, Point p2)
        {
            return System.Math.Sqrt(System.Math.Pow(p1.X - p2.X, 2) + System.Math.Pow(p1.Y - p2.Y, 2));
        }

        public Point Middle(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X)/2, (p1.Y + p2.Y)/2);
        }
    }
}
