using Newtonsoft.Json;
using System.Net;

namespace PluginTemplate
{
    /// <summary>
    /// 针对论坛使用的Keeper基类，增加了一些论坛相关的字段
    /// </summary>
    public abstract class AbstractForumKeepper:AbstractKeepper
    {
        public AbstractForumKeepper(string initKey):base(initKey)
        {
            _user = JsonConvert.DeserializeObject<UserInfo>(initKey);
        }

        public override string GetInitKey()
        {
            return JsonConvert.SerializeObject(_user);
        }

        protected readonly UserInfo _user;

        protected abstract bool IsLogin();
        protected abstract void Login();

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

            public CookieContainer Cookies { get; set; } = new CookieContainer();
        }

    }
}
