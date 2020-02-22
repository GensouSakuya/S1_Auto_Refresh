using Newtonsoft.Json;
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

        public class UserInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public int QuestionID { get; set; }
            public string Answer { get; set; }

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
