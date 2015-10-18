using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Domains
{
    class ByRefType : MarshalByRefObject
    {
        public string domainName = AppDomain.CurrentDomain.FriendlyName;

        public int X { get; set; }

        public ByRefType()
        {
            Console.WriteLine("Call contructor ByRefType by domain: {0}", domainName);
        }
        public void SomeMethod()
        {
            Console.WriteLine("Domain ref: " + AppDomain.CurrentDomain.FriendlyName);
            throw new IndexOutOfRangeException("ByRefType");
        }
    }

    [Serializable]
    class ByValueType
    {
        public int X { get; set; }

        public ByValueType()
        {
            Console.WriteLine("Call contructor ByValueType by domain: {0}", AppDomain.CurrentDomain.FriendlyName);
        }

        public void SomeMethod()
        {
            Console.WriteLine("Domain value: " + AppDomain.CurrentDomain.FriendlyName);
            throw new IndexOutOfRangeException("ByValueType");
        }

    }

    class Program
    {
        public static int counter = 1000000;


        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main(string[] args)
        {
            AppDomain.MonitoringIsEnabled = true;
            var domain1 = AppDomain.CreateDomain("second");
            var domain2 = AppDomain.CreateDomain("third");
            var assemblyName = Assembly.GetEntryAssembly().FullName;
            var path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Domains.exe"))))) 
                +  "/NeutralAssembly/bin/Debug/NeutralAssembly.exe";
            domain2.ExecuteAssembly(path);

            AppDomain.Unload(domain2);
            domain2 = AppDomain.CreateDomain("third");
            Console.WriteLine();

            var sw = Stopwatch.StartNew();
            ByRefType byRefType = (ByRefType)domain1.CreateInstanceAndUnwrap(assemblyName, typeof(ByRefType).FullName);
            sw.Stop();
            Console.WriteLine("Time for create ref: {0} ms", sw.ElapsedMilliseconds);
            Console.WriteLine("Is proxy = {0}", RemotingServices.IsTransparentProxy(byRefType));
            domain1.FirstChanceException += domain_FirstChanceException;
            try
            {
                byRefType.SomeMethod();
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Can catch exception");
            }
            catch(Exception exception)
            {
                Console.WriteLine("Catch: {0}", exception.GetType().Name);
            }
            Console.WriteLine();

            sw.Restart();
            for (int i = 0; i < counter; i++)
            {
                byRefType.X++;
            }
            sw.Stop();

            Console.WriteLine("Time for access to refObject: {0} ms\n",sw.ElapsedMilliseconds);

            sw.Restart();
            var byValueType = (ByValueType)domain2.CreateInstanceAndUnwrap(Assembly.GetEntryAssembly().FullName, typeof(ByValueType).FullName);
            sw.Stop();
            domain2.FirstChanceException += domain_FirstChanceException; // не отработает
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(byValueType));
            Console.WriteLine("Time by val: {0} ms", sw.ElapsedMilliseconds);
            try
            {
                byValueType.SomeMethod();
            }
            catch(Exception exception)
            {
                Console.WriteLine("Catch {0}: {1}",exception.GetType().Name, exception.Message);
            }
            Console.WriteLine();

            sw.Restart();
            for (int i = 0; i < counter; i++)
            {
                byValueType.X++;
            }
            sw.Stop();

            Console.WriteLine("Time for access to valObject: {0} ms\n", sw.ElapsedMilliseconds);

            Console.WriteLine("SizeOf domain1: " + domain1.MonitoringTotalAllocatedMemorySize);
            Console.WriteLine("SizeOf domain2: " + domain2.MonitoringTotalAllocatedMemorySize);
        }

        static void domain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine("Exception ({0}):{1}\nBy {2} domain",e.Exception.GetType().Name, e.Exception.Message, AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
