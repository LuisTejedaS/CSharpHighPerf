using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCompress
{
    /// <summary>
    /// Sync compress vs async compress (read and write)
    /// </summary>
    class Program
    {

        private static void SyncCompress(IEnumerable<string> fileList)
        {
            byte[] buffer = new byte[16384];
            foreach (var file in fileList)
            {
                using (var inputStream = File.OpenRead(file))
                using (var outputStream = File.OpenWrite(file + ".compressed"))
                using (var compressStrem = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    int read = 0;
                    while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        compressStrem.Write(buffer, 0, read);
                    }
                }

            }
        }

        private static async Task AsyncCompress(IEnumerable<string> fileList)
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();

            byte[] buffer = new byte[16384];
            foreach (var file in fileList)
            {
                using (var inputStream = File.OpenRead(file))
                using (var outputStream = File.OpenWrite(file + ".compressed"))
                using (var compressStrem = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    int read = 0;
                    while ((read = await inputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await compressStrem.WriteAsync(buffer, 0, read);
                    }
                }

            }
            watch.Stop();
            Console.WriteLine("finished async, elapsed : {0} ", watch.Elapsed);


        }


        static void Main(string[] args)
        {
            var pathFile = @"C:\Users\Luis.Tejeda\Desktop\1000MB2.txt";
            var pathFile1 = @"C:\Users\Luis.Tejeda\Desktop\1000MB.txt";
            Stopwatch watch = new Stopwatch();
            watch.Restart();

            List<string> fileList = new List<string>();
            fileList.Add(pathFile);
            fileList.Add(pathFile1);
            SyncCompress(fileList);
            watch.Stop();
            Console.WriteLine("finished sync, elapsed : {0} ", watch.Elapsed);
            AsyncCompress(fileList).ConfigureAwait(false);
            Console.WriteLine("Doing stuff");

            Console.ReadKey();
        }
    }
}
