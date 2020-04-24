using System;

namespace Plugins.ManHuaBuDang.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
                return;
            var key = args[0];
            Console.WriteLine($"initKey:{key}");
            
            var keeper = new ManHuaBuDangKeeper(key);
            keeper.Start();


            Console.ReadKey();
        }
    }
}
