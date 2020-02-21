using Core;
using System.IO;

namespace ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginFolderName = "Plugins";
            if (!Directory.Exists(pluginFolderName))
            {
                throw new DirectoryNotFoundException("Plugins folder is not found");
            }
            PluginHelper.GetPlugins("Plugins");
        }
    }
}
