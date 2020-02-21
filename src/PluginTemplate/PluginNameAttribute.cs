using System;

namespace PluginTemplate
{
    public class PluginNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public PluginNameAttribute(string name)
        {
            Name = name;
        }
    }
}
