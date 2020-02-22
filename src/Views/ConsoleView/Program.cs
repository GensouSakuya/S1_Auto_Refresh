using Core;
using System.IO;
using CommandLine;
using System;
using System.Text;

namespace ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.PluginDirecotry != null)
                {
                    var path = Path.GetFullPath(o.PluginDirecotry);
                    if (!Directory.Exists(path))
                    {
                        throw new DirectoryNotFoundException("Plugins folder is not found");
                    }
                    Configurations.PluginsPath = path;
                }
            });

            Console.WriteLine("Loading Plugins");
            var keeperCore = new KeeperCore();
            Console.WriteLine("Plugins Loaded");

            var command = "";
            command = Console.ReadLine();
            var lowerCommand = command?.ToLower();
            while (lowerCommand != "exit" || lowerCommand != "quit")
            {
                try
                {
                    var subCommands = command.Split(' ');
                    var mainCommand = subCommands[0].ToLower();
                    if (mainCommand == "add")
                    {
                        if (subCommands.Length < 3)
                        {
                            throw new ArgumentException("add [key] [initKey]");
                        }

                        var key = subCommands[1];
                        var initKey = new StringBuilder();
                        for (var i = 2; i < subCommands.Length; i++)
                        {
                            initKey.Append(subCommands[i]);
                        }

                        keeperCore.AddKeeper(key, initKey.ToString());
                    }
                    else if (mainCommand == "start")
                    {
                        keeperCore.Start();
                    }
                    else if (mainCommand == "stop")
                    {
                        keeperCore.Stop();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command = Console.ReadLine();
                    lowerCommand = command?.ToLower();
                }
            }
        }
    }
}
