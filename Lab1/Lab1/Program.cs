using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{

    class Program
    {
        public delegate void MyDelegat<in T>(T first, T second);

        static void Main(string[] args)
        {
            Child c1 = new Child { A = 1, B = 1 };
            Child c2 = new Child { A = 2, B = 2 };

            var del = new MyDelegat<Child>(Parent.DoubleSum); //контрвариативность
            del(c1, c2);
            
            Separate();
            del = c1.SuperSum;
            del.Invoke(c1, c2);
            
            Separate();
            del += c1.Sum;
            del += c2.SuperSum;
            del(c1, c2);

            Separate();
            del = (MyDelegat<Child>)Delegate.Remove(del, new MyDelegat<Child>(c1.Sum));
            
            Separate();
            del(c1, c2);
        }

        static void Separate()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }
}
