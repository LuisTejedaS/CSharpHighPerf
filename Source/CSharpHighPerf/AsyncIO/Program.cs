using System;
using System.Diagnostics; 
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsyncIO
{
    /// <summary>
    /// reach a web page and returns string content
    /// </summary>
    class Program
    {
        static Regex regex = new Regex("<title>(.*)</title>", RegexOptions.Compiled);

        private static async Task<string> GetWebPageTitle(string url)
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            Task<string> task = client.GetStringAsync(url);
            string contents = await task;

            Match match = regex.Match(contents);

            if (match.Success)
            {
                watch.Stop();
                return match.Groups[1].Captures[0].Value + " elapsed: " + watch.Elapsed;
            }
            return string.Empty;

        }

        static void Main(string[] args)
        {
            GetWebPageTitle(@"http://www.bing.com").ContinueWith(task => Console.WriteLine(task.Result));
            Console.WriteLine("Continue work without blocking, press any key to exit... ");
            Console.ReadKey();
        }
    }
}
