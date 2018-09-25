using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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

            if (DateTime.Today.Subtract(user.LastCheckTime.Date).Days >= 1)
            {
                html = HttpHelper.GetHtml("https://bbs.saraba1st.com/2b", true, user.Cookies);
                if (HasCheck(html))
                {
                    var hashUrl = GetCheckFormHashUrl(html);
                    Check(user, hashUrl);
                }

                SetCheckTime(user);
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

        public static void Check(UserInfo user,string hashCheckUrl)
        {
            HttpHelper.GetHtml($"https://bbs.saraba1st.com/2b/{hashCheckUrl}", true, user.Cookies);
        }

        public static void SetCheckTime(UserInfo user)
        {
            user.LastCheckTime = DateTime.Now;
            using (var db = new SQLiteDb())
            {
                var model = db.Set<UserInfo>().Where(p => p.UserName == user.UserName).FirstOrDefault();
                model.LastCheckTime = user.LastCheckTime;
                db.SaveChanges();
            }
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

        public static bool HasCheck(string html)
        {
            return html.Contains("打卡签到");
        }

        public static string GetCheckFormHashUrl(string html)
        {
            var checkHtml = html.Split('\n').Where(p => p.Contains("study_daily_attendance-daily_attendance.html")).FirstOrDefault()
                ?.Split('"');
            if (checkHtml.Count() > 2)
            {
                return checkHtml[1];
            }
            return null;
        }

        public static List<UserInfo> GetUsersFromDB()
        {
            using (var db = new SQLiteDb())
            {
                return db.Set<UserInfo>().ToList();
            }
        }

        public static void AddUserToDB(UserInfo user)
        {
            using (var db = new SQLiteDb())
            {
                db.Set<UserInfo>().Add(user);
                db.SaveChanges();
            }
        }

        public static void DelUserFromDB(string userName)
        {
            using (var db = new SQLiteDb())
            {
                var user = db.Set<UserInfo>().Where(p => p.UserName == userName).FirstOrDefault();
                if (user == null)
                    return;
                db.Set<UserInfo>().Remove(user);
                db.SaveChanges();
            }
        }
    }
}
