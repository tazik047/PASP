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
            del += c1.Sum;
            del += c2.SuperSum;
            del(c1, c2);

            Separate();
            del -= c1.Sum;
            del(c1, c2);

            Separate();
            var m1 = new My(1);
            var m2 = new My();
            var m3 = new My(3);
            m1.MyEvent +=Action_MyEvent;
            m2.MyEvent +=Action_MyEvent;
            m3.MyEvent +=Action_MyEvent;
            Func<int> myDel = m1.Print;
            myDel += m2.Print;
            myDel += m3.Print;
            try
            {
                Console.WriteLine(myDel());
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Произошла ошибка.");
            }

            Separate();
            foreach (var i in myDel.GetInvocationList().Cast<Func<int>>())
            {
                try
                {
                    Console.WriteLine(i());
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Произошла ошибка.");
                }
            }
        }

        static void Action_MyEvent(object sender, int e)
        {
            Console.WriteLine("MyEvent - {0}", e);
        }

        static void Separate()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

    }

    class My
    {
        private readonly int _a;

        public event EventHandler<int> MyEvent;

        public int Print()
        {
            if (_a == 0)
                throw new ArgumentException();
            Action();
            return _a;
        }

        public My() { }

        public My(int a)
        {
            _a = a;
        }

        private void Action()
        {
            if (MyEvent != null)
                MyEvent(this, _a);
        }
    }
}
