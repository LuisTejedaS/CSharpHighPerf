using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesIL
{
    class Program
    {
        private static int loopCount = 10000;
        private delegate int MathOp(int x, int y);
        private static int Add(int x, int y) { return x + y; }
        private static int DoOperation(MathOp op, int x, int y) { return op(x, y); }

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            watch.Restart();
            for (int i = 0; i < loopCount; i++)
            {
                Console.Write(DoOperation(Add, 1, 2));
            }

            watch.Stop();


            MathOp op = Add;
            watch2.Restart();

            for (int i = 0; i < loopCount; i++)
            {
                Console.Write(DoOperation(op, 1, 2));
            }
            watch2.Stop();
            Console.WriteLine("Elapsed: {0}", watch.Elapsed);

            Console.WriteLine("Elapsed: {0}", watch2.Elapsed);
            Console.ReadKey();

        }
    }
}
