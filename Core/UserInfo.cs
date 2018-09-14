using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }

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

        internal CookieContainer Cookies { get; set; } = new CookieContainer();
    }
}
