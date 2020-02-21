using System;

namespace PluginTemplate
{
    public class PluginKeyAttribute:Attribute
    {
        public string Key { get; private set; }
        public PluginKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
