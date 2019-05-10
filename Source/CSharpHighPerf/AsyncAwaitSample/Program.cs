using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitSample
{
    /// <summary>
    /// Compares async func vs not async func
    /// </summary>
    class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        const int WaitTimeMS = 1000;

        static void Func1()
        {

            while (true)
            {
                semaphore.Wait();
                Console.WriteLine("Func1");
                semaphore.Release();
                Thread.Sleep(WaitTimeMS);

            }
        }

        static void Func2()
        {
            while (true)
            {
                semaphore.Wait();
                Console.WriteLine("Func2");
                semaphore.Release();
                Thread.Sleep(WaitTimeMS);
            }
        }

        static void AsyncFunc1()
        {
            semaphore.WaitAsync().ContinueWith(_ =>
            {
                Console.WriteLine("asyncfunc1");
                semaphore.Release();
                Thread.Sleep(WaitTimeMS);
            }).ContinueWith(_ => AsyncFunc1());

        }

        static void AsyncFunc2()
        {
            semaphore.WaitAsync().ContinueWith(_ =>
            {
                Console.WriteLine("asyncfunc2");
                semaphore.Release();
                Thread.Sleep(WaitTimeMS);
            }).ContinueWith(_ => AsyncFunc2());

        }

        static void Main(string[] args)
        {
            Console.WriteLine("starting ...");
            Task.Run((Action)AsyncFunc1);
            Task.Run((Action)AsyncFunc2);
            Console.WriteLine("Finished ...");
            Console.ReadKey();
        }
    }
}
