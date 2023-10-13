using Core.Models;
using PluginTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace SimpleForm
{
    public class User: AbstractForumKeeper.UserInfo
    {
        public User() { }

        public User(string username, string password, int questionID, string answer,string keeperKey) : this(username, keeperKey, false)
        {
            QuestionID = questionID;
            Answer = answer;
            Password = password;
        }

        public User(string username, string keeperKey, bool loginManually)
        {
            UserName = username;
            KeeperKey = keeperKey;
            IsLoginManually = loginManually;
        }

        public string Status { get; internal set; }
        public DateTime LastRefreshTime { get; internal set; }
        internal bool IsLogin { get; set; } = false;

        public KeeperModel KeeperModel { get; set; }
        public string KeeperKey { get; set; }
        public string KeeperInitKey { get; set; }
        public string KeeperName { get; set; }
        public string RawCookies { get; set; }

        public void LoadCookies(CookieContainer cc)
        {
            List<CookieModel> lstCookies = new List<CookieModel>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                foreach (Cookie c in colCookies) lstCookies.Add(new CookieModel
                {
                    Domain=c.Domain,
                    Name=c.Name,
                    Value=c.Value,
                    Path=c.Path,
                });
            }
            RawCookies = Newtonsoft.Json.JsonConvert.SerializeObject(lstCookies);
        }

        public CookieContainer GetCookieContainer()
        {
            var cookies= Newtonsoft.Json.JsonConvert.DeserializeObject<List<CookieModel>>(RawCookies);
            var cc = new CookieContainer();
            foreach(var cookie in cookies)
            {
                cc.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
            }
            return cc;
        }

        internal class CookieModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Path { get; set; }
            public string Domain { get; set; }
        }
    }
}
