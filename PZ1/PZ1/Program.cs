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
            AppDomain domain1 = AppDomain.CreateDomain("first", null);
            var t = domain1.CreateInstance("ClassLibrary", "Point");
            
        }
    }
}
