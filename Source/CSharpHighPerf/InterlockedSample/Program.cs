using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InterlockedSample
{
    class Program
    {
        private static int ThreadCount = 0;
        private static int Targetcount = 10000000;
        private static readonly object syncObject = new object();
        private static int acumulatorLock = 0;

        private static void TestPerf(Action action)
        {
            var tasks = new Task[ThreadCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(action);
            }
            Task.WaitAll(tasks);

        }

        private static void LockPerf()
        {
            var action = new Action(() =>
            {
                bool quit = false;
                while (!quit)
                {
                    lock (syncObject)
                    {
                        if (++acumulatorLock >= Targetcount)
                        {
                            quit = true;
                        }
                    }

                }
            });
            TestPerf(action);
        }

        private static void InterLockedPerf()
        {

            var action = new Action(() =>
            {
                bool quit = false;
                while (!quit)
                {
                    if (System.Threading.Interlocked.Increment(ref acumulatorLock) > Targetcount)
                    {
                        quit = true;
                    }

                }
            });
            TestPerf(action);

        }

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            InterLockedPerf();
            watch.Stop();

            Console.WriteLine("Fnished, elapsed: {0}", watch.Elapsed);


            watch.Restart();
            LockPerf();
            watch.Stop();
            Console.WriteLine("Fnished, elapsed: {0}", watch.Elapsed);

        }
    }
}
