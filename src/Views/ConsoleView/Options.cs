using CommandLine;

namespace ConsoleView
{
    public class Options
    {
        [Option(shortName:'d')]
        public string PluginDirecotry { get; set; }
    }
}
