using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FromAsyncIO
{
    /// <summary>
    /// asyn reading from file
    /// </summary>
    class Program
    {
        const int TotalLength = 1024;
        const int ReadSize = TotalLength / 4;

        static Task<string> GetStringFromFile(string path)
        {
            byte[] buffer = new byte[TotalLength];
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, buffer.Length, FileOptions.DeleteOnClose | FileOptions.Asynchronous);

            Task<int> task = Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, 0, buffer.Length, null);

            return task.ContinueWith(readTask =>
            {
                stream.Close();
                int bytesRead = readTask.Result;
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                ;
            });
        }

        static void OnReadBuffer(Task<int> readTask, Stream stream, byte[] buffer, int offset, TaskCompletionSource<string> tcs)
        {
            int bytesRead = readTask.Result;
            if (bytesRead > 0)
            {
                Task<int> task = Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, offset + bytesRead, Math.Min(buffer.Length - (offset + bytesRead),
                    ReadSize), null);

                task.ContinueWith(callbackTask => OnReadBuffer(callbackTask, stream, buffer, offset + bytesRead, tcs));
            }
            else
            {
                tcs.TrySetResult(Encoding.UTF8.GetString(buffer, 0, offset));
            }
        }


        static Task<string> GetStringFromFileBetter(string path)
        {
            byte[] buffer = new byte[TotalLength];
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, buffer.Length, FileOptions.DeleteOnClose | FileOptions.Asynchronous);

            Task<int> task = Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, 0, buffer.Length, null);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            task.ContinueWith(readTask => OnReadBuffer(readTask, stream, buffer, 0, tcs));
            return tcs.Task;
        }


        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            watch.Restart();
            string tempFile = Path.GetTempFileName();
            Console.WriteLine("the file is: {0}", tempFile);
            string contents = new string('a', TotalLength);
            Console.WriteLine("writing to file");
            File.WriteAllText(tempFile, contents);

            Console.WriteLine("Reading from file");
            GetStringFromFileBetter(tempFile).ConfigureAwait(false);
            watch.Stop();
            Console.WriteLine("elapsed {0}: ", watch.Elapsed);
            Console.ReadLine();



        }
    }
}
