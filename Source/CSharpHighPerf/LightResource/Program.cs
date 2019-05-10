using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LightResource
{
    class Program
    {
        private static readonly ThreadLocal<Random> threadLocalRand = new ThreadLocal<Random>(() => new Random());

        [ThreadStatic]
        static Random threadStaticRand;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();


            watch.Restart();
            int[] results = new int[100];
            Console.WriteLine("ThreadLocal version");

            Parallel.For(0, 500000000,
                i =>
                {
                    var randomNumber = threadLocalRand.Value.Next(100);
                    Interlocked.Increment(ref results[randomNumber]);
                });
            watch.Stop();
            Console.WriteLine("elapsed time: {0}", watch.Elapsed);

            watch.Restart();

            Console.WriteLine("ThreadStatic version");

            Parallel.For(0, 500000000,
                i =>
                {
                    if (threadStaticRand == null)
                    {
                        threadStaticRand = new Random();
                        var randomNumber = threadLocalRand.Value.Next(100);
                        Interlocked.Increment(ref results[randomNumber]);
                    }


                });
            watch.Stop();
            Console.WriteLine("elapsed time: {0}", watch.Elapsed);



        }
    }
}
