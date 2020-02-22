using PluginTemplate;
using System;

namespace Core.Models
{
    internal class KeeperModel
    {
        public string Key { get; set; }
        public string InitKey { get; set; }
        public AbstractKeeper Keeper { get; internal set; }
        public string Message => Keeper?.Message;

        public override bool Equals(object obj)
        {
            return obj is KeeperModel model &&
                   Key == model.Key &&
                   InitKey == model.InitKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, InitKey);
        }
    }
}
