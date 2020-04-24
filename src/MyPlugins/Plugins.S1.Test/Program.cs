using System;

namespace Plugins.S1.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;
            var key = args[0];
            Console.WriteLine($"initKey:{key}");

            var keeper = new S1Keeper(key);
            keeper.Start();


            Console.ReadKey();
        }
    }
}
