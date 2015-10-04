using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Hosting;
using System.Text;
using System.Threading.Tasks;

namespace PZ1
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(10, 0);
            var math = new Math();
            Console.WriteLine("Length = {0}", math.Length(p1, p2));
            Console.WriteLine("Middle = {0}", math.Middle(p1,p2));
        }
    }
}
