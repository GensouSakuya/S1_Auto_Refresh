using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Core
{
    public class S1Manager
    {
        public static void Refresh(UserInfo user)
        {
            var html = HttpHelper.GetHtml("https://bbs.saraba1st.com/2b", true, user.Cookies);
            if (!user.IsLogin||!IsLogin(html))
            {
                Login(user);
            }

            user.LastRefreshTime = DateTime.Now;
        }

        public static void Login(UserInfo user)
        {
            var res = HttpHelper.GetHtml(
                $"http://bbs.saraba1st.com/2b/api/mobile/index.php?mobile=no&version=1&module=login&loginsubmit=yes&loginfield=auto&submodule=checkpost&username={user.UserName}&password={user.Password}&questionid={user.QuestionID}&answer={user.Answer}",
                false, user.Cookies);
            var result = (JObject)JsonConvert.DeserializeObject(res);
            SetStatus(result["Message"]["messageval"].ToString(), user);
        }

        public static void SetStatus(string message, UserInfo user)
        {
            switch (message)
            {
                case "login_question_empty":
                    user.Status = "需要输入登录问题";
                    user.IsLogin = false;
                    break;
                case "profile_passwd_illegal":
                    user.Status = "需要输入密码";
                    user.IsLogin = false;
                    break;
                case "login_invalid":
                    user.Status = "账号或密码错误";
                    user.IsLogin = false;
                    break;
                case "login_succeed":
                    user.Status = "登录成功";
                    user.IsLogin = true;
                    break;
                default:
                    user.Status = "出现异常";
                    user.IsLogin = false;
                    break;
            }
        }

        public static bool IsLogin(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.GetElementbyId("um") != null;
        }
    }
}
