using Newtonsoft.Json;
using System;
using System.Net;

namespace PluginTemplate
{
    /// <summary>
    /// 针对论坛使用的Keeper基类，增加了一些论坛相关的字段
    /// </summary>
    public abstract class AbstractForumKeeper : AbstractKeeper
    {
        public AbstractForumKeeper(string initKey) : base(initKey)
        {
            _user = JsonConvert.DeserializeObject<UserInfo>(initKey);
        }

        protected readonly UserInfo _user;

        protected abstract bool IsLogin();
        protected abstract void Login();
        protected Func<string, UserInfo, CookieContainer> CookieObtainer;
        protected virtual LoginResponse LoginManually()
        {
            return new LoginResponse();
        }

        public void RegisterCookieObtainer(Func<string, UserInfo, CookieContainer> func)
        {
            CookieObtainer = func;
        }

        public void SetCookie(CookieContainer cookies)
        {
            _user.Cookies = cookies;
        }

        public class UserInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public int QuestionID { get; set; }
            public string Answer { get; set; }
            public bool IsLoginManually { get; set; }

            public UserInfo(UserInfo user)
            {
                UserName = user.UserName;
                Password = user.Password;
                QuestionID = user.QuestionID;
                Answer = user.Answer;
                IsLoginManually = user.IsLoginManually;
            }

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

            [JsonIgnore]
            public CookieContainer Cookies { get; set; } = new CookieContainer();

            public string ToInitKey()
            {
                return JsonConvert.SerializeObject(new UserInfo(this));
            }
        }

        public class LoginResponse
        {
            public bool IsSucceed { get; set; }
            public CookieContainer Cookies { get; set; }
        }
    }
}
