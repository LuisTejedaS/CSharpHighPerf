using System;
using System.Diagnostics;

namespace BoxUnbox
{
    class Program
    {
        static void Main(string[] args)
        {
            int val = 13;
            object boxedVal = val;
            val = 14;
            string.Format("val: {0}, boxedVal: {1}", val, boxedVal);
            string.Format("Number of processes on machin: {0}", Process.GetProcesses().Length);
        }
    }
}
