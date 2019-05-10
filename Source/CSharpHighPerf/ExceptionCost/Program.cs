using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ExceptionCost
{
    class Program
    {
        const int NumIterations = 1000;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void EmptyMethod()
        {

        }
        static void ExceptionMethod(int depth)
        {
            if (depth > 1)
            {
                ExceptionMethod(depth - 1);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        static void Main(string[] args)
        {
            EmptyMethod();
            try
            {
                ExceptionMethod(1);
            }
            catch (InvalidOperationException)
            {

            }

            Stopwatch watch = new Stopwatch();

            watch.Restart();
            for (int i = 0; i < NumIterations; i++)
            {
                EmptyMethod();
            }
            watch.Stop();

            long baselineTime = watch.ElapsedTicks;
            Console.WriteLine("Empty method 1x" + "elapsed: " + baselineTime);

            for (int depth = 1; depth <= 10; depth++)
            {
                watch.Restart();
                for (int i = 0; i < NumIterations; i++)
                {
                    try
                    {
                        ExceptionMethod(depth);
                    }
                    catch (InvalidOperationException)
                    {

                    }
                }
                watch.Stop();
                Console.WriteLine("exception (depth = {0}): {1:f1}x ", depth, (double)watch.ElapsedTicks / baselineTime);

            }



        }
    }
}
