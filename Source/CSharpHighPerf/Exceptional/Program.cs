using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptional
{
    public class Program
    {
        static void Main(string[] args)
        {
        }

        public static void ValidateLogin(string username, string pass)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new ArgumentNullException("use parameter is null");
            }
            catch (Exception)
            {


            }

        }
    }
}
