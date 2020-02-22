using PluginTemplate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core
{
    internal class PluginHelper
    {
        private static List<PluginModel> _plugins;
        public static void Init(string path)
        {
            _plugins = GetPlugins(path);
        }

        public static List<PluginModel> GetPlugins(string path)
        {
            var list = new List<PluginModel>();
            if (!Directory.Exists(path))
            {
                return list;
            }
            var files = Directory.GetFiles(path, "*.dll");
            files.ToList().ForEach(p =>
            {
                try
                {
                    var assembly = Assembly.LoadFile(Path.GetFullPath(p));
                    var keeperTypes = assembly.DefinedTypes.Where(p => p.IsSubclassOf(typeof(AbstractKeeper))).ToList();
                    if (keeperTypes.Any())
                    {
                        keeperTypes.ForEach(kT =>
                        {
                            var keyAttr = kT.GetCustomAttribute<PluginKeyAttribute>();
                            var nameAttr = kT.GetCustomAttribute<PluginNameAttribute>();
                            list.Add(new PluginModel
                            {
                                Type = kT, Key = keyAttr?.Key ?? kT.FullName, Name = nameAttr?.Name ?? kT.Name
                            });
                        });
                    }
                }
                catch
                {
                    LogHelper.WriteLog($"{Path.GetFileName(p)}中未找到可用类");
                }
            });
            return list;
        }

        public static PluginModel Find(string key)
        {
            return _plugins?.Find(p => p.Key == key);
        }

        public static AbstractKeeper GetKeepper(PluginModel plugin, string initKey)
        {
            return (AbstractKeeper)Activator.CreateInstance(plugin.Type, new object[]
            {
                initKey
            });
        }
    }

    public class PluginModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
