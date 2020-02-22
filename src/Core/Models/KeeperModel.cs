using PluginTemplate;
using System.Collections.Generic;

namespace Core.Models
{
    public class KeeperModel
    {
        public string Key { get; internal set; }
        public string InitKey { get; internal set; }
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
            var hashCode = 6905750;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InitKey);
            return hashCode;
        }
    }
}
