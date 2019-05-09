using System;
using System.Collections.Concurrent; 
using System.Diagnostics; 
using System.Threading;
using System.Threading.Tasks;


namespace Parallelt
{
    class Program
    {
        /// <summary>
        /// Compares parallel for and parallel foreach and partitioner
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Stopwatch watch = new Stopwatch();
            const int MaxValue = 100000000;

            long sum = 0;
            watch.Restart();
            sum = 0;
            Parallel.For(0, MaxValue, (i) =>
            {
                Interlocked.Add(ref sum, (long)Math.Sqrt(i));
            });
            watch.Stop();
            Console.WriteLine("Parallel.For:             {0} and the sum is {1}", watch.Elapsed, sum.ToString());

            var partitioner = Partitioner.Create(0, MaxValue);
            watch.Restart();
            sum = 0;
            Parallel.ForEach(partitioner,
                (range) =>
                {
                    long partialSum = 0;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        partialSum += (long)Math.Sqrt(i);
                    }
                    Interlocked.Add(ref sum, partialSum);
                });
            watch.Stop();
            Console.WriteLine("Partitioned Parallel.For: {0} and the sum is {1}", watch.Elapsed, sum.ToString());
        }
    }
}
