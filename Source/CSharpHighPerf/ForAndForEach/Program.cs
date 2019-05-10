using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForAndForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[10000000];
            int sum = 0;

            Stopwatch watch = new Stopwatch();
            watch.Restart();
            for (int i = 0; i < arr.Length; i++)
            {
                sum += i;
            }

            watch.Stop();
            Console.WriteLine("Finished for, elapsed {0}", watch.Elapsed);

            watch.Restart();

            foreach (int val in arr)
            {
                sum += val;
            }
            watch.Stop();
            Console.WriteLine("Finished for each, elapsed {0}", watch.Elapsed);


            IEnumerable<int> arreNum = arr;
            watch.Restart();

            foreach (int val in arreNum)
            {
                sum += val;
            }

            watch.Stop();
            Console.WriteLine("Finished for each ienum, elapsed {0}", watch.Elapsed);
            Console.ReadKey();
        }
    }
}
