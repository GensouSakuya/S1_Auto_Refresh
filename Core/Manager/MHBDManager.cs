using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core
{
    public class MHBDManager: BaseForumManager
    {
        public MHBDManager(UserInfo user)
        {
            _user = user;
        }
        private readonly UserInfo _user;
        private static readonly List<string> _attendanceMark = new List<string>{ "study_daily_attendance-daily_attendance.html", "study_daily_attendance:daily_attendance" };

        public override int IntervalSeconds => 240000;

        public override bool EnableAutoDailyCheckIn => true;

        public override bool IsLogin
        {
            get
            {
                var html = HttpHelper.GetHtml("https://www.manhuabudang.com/index.php", true, _user.Cookies);
                return html.Contains(_user.UserName);
                //var doc = new HtmlDocument();
                //doc.LoadHtml(html);
                //return doc.GetElementbyId("um") != null;
            }
        }

        public override void Login()
        {
            var res = HttpHelper.GetHtml($"https://www.manhuabudang.com/login.php?", false, _user.Cookies,
                $"forward=&jumpurl=https%3A%2F%2Fwww.manhuabudang.com%2Findex.php&step=2&lgt=0&pwuser={_user.UserName}&pwpwd={_user.Password}&question=0&customquest=&answer=&hideid=0&cktime=31536000&submit=");

            if (IsLogin)
            {
                _user.Status = "登录成功";
                _user.IsLogin = true;
            }
            else
            {
                _user.IsLogin = false;
                _user.Status = "登录失败";
            }
        }

        public override bool Check()
        {
            try
            {
                var html = HttpHelper.GetHtml("https://www.manhuabudang.com/u.php", true, _user.Cookies);
                if (HasCheck(html))
                {
                    var hashUrl = GetCheckFormHashUrl(html);
                    HttpHelper.GetHtml($"https://www.manhuabudang.com/jobcenter.php?", false, _user.Cookies,$"action=punch&verify={hashUrl}");
                }
                SetCheckTime(_user);
                _user.LastRefreshTime = DateTime.Now;

                return true;
            }
            catch
            {
                return false;
            }
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

        public static bool HasCheck(string html)
        {
            return html.Contains("punchJob(2);");
        }

        public static string GetCheckFormHashUrl(string html)
        {
            var regex = new Regex("var verifyhash = \'[a-z0-9]+\'");
            var splitedHtml = regex.Match(html).Value?.Split('\'');
            var checkMatchedCount = splitedHtml?.Count() ?? 0;
            if (checkMatchedCount >1 )
            {
                return System.Web.HttpUtility.HtmlDecode(splitedHtml[1]);
            }
            return null;
        }

    }
}
