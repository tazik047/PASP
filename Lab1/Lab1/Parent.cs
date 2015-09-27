using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Parent
    {
        public int A { get; set; }

        public void Sum(Parent p1, Parent p2)
        {
            Console.WriteLine(p1.A + p2.A);
        }

        public static void DoubleSum(Parent p1, Parent p2)
        {
            Console.WriteLine((p1.A + p2.A)*2);
        }
    }
}
