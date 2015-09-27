using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Child : Parent
    {
        public int B { get; set; }

        public void SuperSum(Child c1, Child c2)
        {
            Console.WriteLine(c1.A + c2.A + c1.B + c2.B);
        }
    }
}
