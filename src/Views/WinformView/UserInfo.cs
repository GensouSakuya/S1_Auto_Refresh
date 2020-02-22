using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleForm
{
    public class UserInfo
    {
        public string UserName { get; internal set; }
        public string Password { get; internal set; }
        public int QuestionID { get; internal set; }
        public string Answer { get; internal set; }

        public UserInfo(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public UserInfo(string username, string password, int questionID, string answer) : this(username, password)
        {
            QuestionID = questionID;
            Answer = answer;
        }

        public string Status { get; internal set; }
        public DateTime LastRefreshTime { get; internal set; }
        internal bool IsLogin { get; set; } = false;
        public DateTime LastCheckTime { get; internal set; }

        public KeeperModel KeeperModel { get; set; }
        public string KeeperKey { get; set; }
        public string KeeperInitKey { get; set; }
        public string KeeperName { get; set; }
    }
}
