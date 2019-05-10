using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringOperations
{
    class Program
    {
        static string a = "Cosmo";
        static string b = "Cosmo";

        static void Main(string[] args)

        {

            Stopwatch watch = new Stopwatch();

            watch.Restart();
            StringBuilder name = new StringBuilder("zapato");
            for (int i = 0; i < 1000; i++)
            {               
                a += i;
            }
            watch.Stop();
            Console.WriteLine("Elapsed concat {0}", watch.Elapsed);
            Console.WriteLine("  {0}", a);


            watch.Restart();
            
            for (int i = 0; i < 1000; i++)
            {
                 name.Append(i);
            }
            watch.Stop();
            Console.WriteLine("Elapsed builder append {0}", watch.Elapsed);
            Console.WriteLine("  {0}", a);

        }
    }
}
