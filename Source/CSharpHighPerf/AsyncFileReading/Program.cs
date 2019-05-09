using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncFileReading
{
    /// <summary>
    /// Test different ways to read files
    /// </summary>
    class Program
    {
        /// <summary>
        /// Diferent aproaches to read a txt file set the filepath to a file of 1000MB
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var filePath = @"...\1000MB.txt";
            Stopwatch watch = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            Stopwatch watch3 = new Stopwatch();
            Stopwatch watch4 = new Stopwatch();

            watch.Restart();
            int bytesRead = SynchronousRead(filePath);
            watch.Stop();
            Console.WriteLine("SynchronousRead read {0} bytes,                      elapsed time: {1}", bytesRead, watch.Elapsed);

            watch2.Restart();
            bytesRead = EvenBetter(filePath);
            watch2.Stop();
            Console.WriteLine("EvenBetter read thread {0} bytes,                   elapsed time: {1}", bytesRead, watch2.Elapsed);

            watch3.Restart();
            AsynchronousReadSimple(filePath);
            watch3.Stop();
            Console.WriteLine("AsynchronousReadSimple read {0} bytes,               elapsed time: {1}", 0, watch3.Elapsed);
     
            watch4.Restart();
            var task = AsynchronousRead(filePath);
            var read = task.Result;
            watch4.Stop();

            Console.WriteLine("Chingon AsynchronousRead read {0} bytes,             elapsed time: {1}", read, watch4.Elapsed);
            Console.ReadKey();
        }

        static int SynchronousRead(string filename)
        {
            int chunkSize = 524288;
            byte[] buffer = new byte[chunkSize];

            using (var fileContents = new MemoryStream())
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                int bytesRead = 0;
                do
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    fileContents.Write(buffer, 0, bytesRead);

                } while (bytesRead > 0);
                return (int)fileContents.Length;
            }
        }

        static int EvenBetter(string filename)
        {
            int result = 0;
            result = Task.Run(() =>
            {
                using (var inputStream = File.OpenRead(filename))
                {
                    int chunkSize = 524288;
                    byte[] buffer = new byte[chunkSize];

                    int bytesRead = 0;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);

                    } while (bytesRead > 0);
                    return (int)inputStream.Length;
                }
            }).Result;
            return result;
        }

        static void AsynchronousReadSimple(string filename)
        {
            int chunkSize = 4096;
            byte[] buffer = new byte[chunkSize];

            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, chunkSize, useAsync: true);

            var task = fileStream.ReadAsync(buffer, 0, buffer.Length);
            task.ContinueWith((readtask) =>
            {
                int amountRead = readtask.Result;
                fileStream.Dispose();
                Console.WriteLine("Async(simple) read {0} bytes", amountRead);
            });

        }

        private static void ContinueRead(Task<int> task, FileStream stream, MemoryStream fileContents, byte[] buffer,
            TaskCompletionSource<int> tcs)
        {
            if (task.IsCompleted)
            {
                int bytesRead = task.Result;
                fileContents.Write(buffer, 0, bytesRead);
                if (bytesRead > 0)
                {
                    var newtask = stream.ReadAsync(buffer, 0, buffer.Length);
                    newtask.ContinueWith((readtask) => ContinueRead(readtask, stream, fileContents, buffer, tcs));
                }
                else
                {
                    tcs.TrySetResult((int)fileContents.Length);
                    stream.Dispose();
                    fileContents.Dispose();
                }
            }
        }

        private static Task<int> AsynchronousRead(string filename)
        {
            int chunkSize = 524288;
            byte[] buffer = new byte[chunkSize];

            var tcs = new TaskCompletionSource<int>();
            var fileContents = new MemoryStream();
            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, chunkSize, useAsync: true);
            fileContents.Capacity += chunkSize;

            var task = fileStream.ReadAsync(buffer, 0, buffer.Length);
            task.ContinueWith((readTask) => ContinueRead(readTask, fileStream, fileContents, buffer, tcs));
            return tcs.Task;

        }


    }
}
