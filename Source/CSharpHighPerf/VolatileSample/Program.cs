using System;
using System.Threading; 

namespace VolatileSample
{
    /// <summary>
    /// demonstrate how to read a variable value with volatile
    /// </summary>
    class Program
    {
        volatile bool _loop = true;

        private static void SomeThread(object o1)
        {
            Program o = (Program)o1;
            Console.WriteLine("loop starting ...");
            while (o._loop)
            {

            }
            Console.WriteLine("loop stoping ..");
        }

        static void Main(string[] args)
        {
            Program o1 = new Program();
            Thread t1 = new Thread(SomeThread);
            t1.Start(o1);
            Thread.Sleep(2000);
            o1._loop = false;
            Console.WriteLine("value set to false");
        }
    }
}
