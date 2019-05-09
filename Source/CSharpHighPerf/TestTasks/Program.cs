using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContpaqiTasks
{
    /// <summary>
    /// Use tasks to run activities vs sequential
    /// </summary>
    class Program
    {
        static Stopwatch watch = new Stopwatch();
        static int pendingTasks;

        static void Main(string[] args)
        {
            const int MaxValue = 1000000000;
            long sum = 0;
            watch.Start();
            for (int i = 0; i <= MaxValue; i++)
            {
                sum += (long)Math.Sqrt(i);
            }
            watch.Stop();

            Console.WriteLine("Sequential: {0}", watch.Elapsed);

            watch.Restart();
            int numTasks = Environment.ProcessorCount;
            pendingTasks = numTasks;
            int perThreadCount = MaxValue / numTasks;
            int perThreadLeftOver = MaxValue % numTasks;

            Task<long>[] tasks = new Task<long>[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                int start = i * perThreadCount;
                int end = (i + 1) + perThreadCount;
                if (i == numTasks - 1)
                {
                    end += perThreadLeftOver;
                }
                tasks[i] = Task<long>.Run(() =>
                {
                    long threadSum = 0;
                    for (int j = start; j <= end; j++)
                    {
                        threadSum += (long)Math.Sqrt(j);
                    }
                    return threadSum;
                });
                //tasks[i].ContinueWith(OnTaskEnd, TaskContinuationOptions.ExecuteSynchronously);
                tasks[i].ContinueWith(OnTaskEnd);

            }
            Task.WaitAll(tasks);
            Console.WriteLine("Total Tasks: {0}", watch.Elapsed);
            Console.ReadKey();
        }



        private static void OnTaskEnd(Task<long> task)
        {
            Console.WriteLine("Tasks: {0}", watch.Elapsed);
        }
    }
}
