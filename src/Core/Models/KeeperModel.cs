using PluginTemplate;
using System;
using System.Collections.Generic;
using System.Net;
using static PluginTemplate.AbstractForumKeeper;

namespace Core.Models
{
    public class KeeperModel
    {
        public string Key { get; internal set; }
        public string InitKey { get; internal set; }
        public AbstractKeeper Keeper { get; internal set; }
        public string Message => Keeper?.Message;
        public Func<string, UserInfo, CookieContainer> LoginManuallyFunc { get; internal set; }
        public CookieContainer CookieContainer { get; internal set; }

        public void RegisterLoginManuallyFunc(Func<string, string, UserInfo, CookieContainer> func)
        {
            LoginManuallyFunc = (string a,UserInfo b) => func?.Invoke(this.Key, a, b); 
        }

        public void SetCookies(CookieContainer cookies)
        {
            CookieContainer = cookies;
        }

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
