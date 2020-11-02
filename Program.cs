using System;
using ClassLibrary;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var serwerTCP = new ServerTCP();
            try
            {
                serwerTCP.Start();
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
