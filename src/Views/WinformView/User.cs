using Core.Models;
using PluginTemplate;
using System;

namespace SimpleForm
{
    public class User: AbstractForumKeeper.UserInfo
    {
        public User() { }
        public User(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public User(string username, string password, int questionID, string answer,string keeperKey) : this(username, password)
        {
            QuestionID = questionID;
            Answer = answer;
            KeeperKey = keeperKey;
        }

        public string Status { get; internal set; }
        public DateTime LastRefreshTime { get; internal set; }
        internal bool IsLogin { get; set; } = false;

        public KeeperModel KeeperModel { get; set; }
        public string KeeperKey { get; set; }
        public string KeeperInitKey { get; set; }
        public string KeeperName { get; set; }
    }
}
