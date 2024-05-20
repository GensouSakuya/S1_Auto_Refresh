using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginTemplate;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.S1
{
    [PluginKey("Plugins.S1")]
    [PluginName("S1")]
    public class S1Keeper : AbstractForumKeeper
    {
        private static readonly List<string> _attendanceMark = new List<string> { "study_daily_attendance-daily_attendance.html", "study_daily_attendance:daily_attendance" };

        public S1Keeper(string initKey) : base(initKey)
        {
        }

        protected override int KeepOnlineIntervalSeconds => 240;

        protected override bool IsLogin()
        {
            using(var client = new RestClient())
            {
                var req = new RestRequest("https://bbs.saraba1st.com/2b/");
                req.CookieContainer = _user.Cookies;
                var res = client.Get(req);
                var html = res.Content;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc.GetElementbyId("um") != null;
            }
        }

        protected override LoginResponse LoginManually()
        {
            if (CookieObtainer == null)
                throw new InvalidOperationException("LoginAndObtainCookiesManual not registered");
            var cookies = CookieObtainer("https://bbs.saraba1st.com/2b/", _user);
            return new LoginResponse
            {
                IsSucceed = true,
                Cookies = cookies
            };
        }

        protected override bool IsDailyCheckInEnabled => true;

        protected override void DailyCheck()
        {
            var html = HttpHelper.GetHtml("https://bbs.saraba1st.com/2b/", true, _user.Cookies);
            if (HasCheck(html))
            {
                var hashUrl = GetCheckFormHashUrl(html);
                HttpHelper.GetHtml($"https://bbs.saraba1st.com/2b/{hashUrl}", true, _user.Cookies);
            }
        }

        private static bool HasCheck(string html)
        {
            return html.Contains("打卡签到");
        }

        private static string GetCheckFormHashUrl(string html)
        {
            List<string> checkedHtml = null;
            var splitedHtml = html.Split('\n');
            foreach (var mark in _attendanceMark)
            {
                if (splitedHtml.Any(p => p.Contains(mark)))
                {
                    checkedHtml = splitedHtml.Where(p => p.Contains(mark)).FirstOrDefault()
                        ?.Split('"').ToList();
                    break;
                }
            }
            var checkMatchedCount = checkedHtml?.Count() ?? 0;
            if (checkMatchedCount > 2)
            {
                return System.Web.HttpUtility.HtmlDecode(checkedHtml[1]); ;
            }
            return null;
        }

        protected override void KeepOnline()
        {
            //因为检查登录状态本身就是一种访问，所以不需要进行额外操作
            if (!IsLogin())
            {
                if(_user.IsLoginManually)
                {
                    var res = LoginManually();
                    _user.Cookies = res.Cookies;
                }
                else
                {
                    Login();
                }
            }
        }

        protected override void Login()
        {
            var res = HttpHelper.GetHtml(
                $"http://bbs.saraba1st.com/2b/api/mobile/index.php?mobile=no&version=1&module=login&loginsubmit=yes&loginfield=auto&submodule=checkpost&username={_user.UserName}&password={_user.Password}&questionid={_user.QuestionID}&answer={_user.Answer}",
                false, _user.Cookies);
            var result = (JObject)JsonConvert.DeserializeObject(res);
            SetStatus(result["Message"]["messageval"].ToString());
        }

        private void SetStatus(string message)
        {
            switch (message)
            {
                case "login_question_empty":
                    Message = "需要输入登录问题";
                    break;
                case "profile_passwd_illegal":
                    Message = "需要输入密码";
                    break;
                case "login_invalid":
                    Message = "账号或密码错误";
                    break;
                case "login_succeed":
                    Message = "登录成功";
                    break;
                default:
                    Message = "出现异常";
                    break;
            }
        }
    }
}
