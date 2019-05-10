using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockVariable
{
    /// <summary>
    /// Lock variable demos
    /// </summary>
    class Program
    {
        static void LockDemo()
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            object syncObj = new object();
            var masterList = new List<long>();
            const int numTasks = 8;
            Task[] tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 10000000; j++)
                    {
                        lock (syncObj)
                        {
                            masterList.Add(j);
                        }

                    }
                });
            }
            Task.WaitAll(tasks);
            watch.Stop();
            Console.WriteLine("Elapsed locked {0}: ", watch.Elapsed);
        }


        static void BetterLockDemo()
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            object syncObj = new object();
            var masterList = new List<long>();
            const int numTasks = 8;
            Task[] tasks = new Task[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var localList = new List<long>();

                    for (int j = 0; j < 10000000; j++)
                    {
                        localList.Add(j);

                    }
                    lock (syncObj)
                    {
                        masterList.AddRange(localList);
                    }
                });
            }
            Task.WaitAll(tasks);
            watch.Stop();
            Console.WriteLine("Elapsed beter locked {0}: ", watch.Elapsed);
        }

        static void Main(string[] args)
        {
            LockDemo();
            BetterLockDemo();
        }
    }
}
