﻿using System;
using System.Net;

namespace Core
{
    public class UserInfo
    {
        public string UserName { get; internal set; }
        public string Password { get; internal set; }
        public int QuestionID { get; internal set; }
        public string Answer { get; internal set; }

        public UserInfo()
        {

        }

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
        internal CookieContainer Cookies { get; set; } = new CookieContainer();
    }
}
