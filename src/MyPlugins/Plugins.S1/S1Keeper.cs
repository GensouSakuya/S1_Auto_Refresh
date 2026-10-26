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
        private const string HOST = "https://stage1st.com";
        private const string CHECKIN_PAGE = HOST + "/2b/muanyun_053-muanyun_053.html";
        private const string CHECKIN_API = HOST + "/2b/plugin.php?id=muanyun_053&op=checkin";

        public S1Keeper(string initKey) : base(initKey)
        {
        }

        protected override int KeepOnlineIntervalSeconds => 240;

        protected override bool IsLogin()
        {
            return IsLoginHtml(GetCheckInPage());
        }

        private string GetCheckInPage()
        {
            using (var client = new RestClient())
            {
                var req = new RestRequest(CHECKIN_PAGE);
                req.CookieContainer = _user.Cookies;
                return client.Get(req).Content;
            }
        }

        private static bool IsLoginHtml(string html)
        {
            return ParseHtml(html).GetElementbyId("um") != null;
        }

        private static HtmlDocument ParseHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html ?? "");
            return doc;
        }

        protected override LoginResponse LoginManually()
        {
            if (CookieObtainer == null)
                throw new InvalidOperationException("LoginAndObtainCookiesManual not registered");
            var cookies = CookieObtainer(CHECKIN_PAGE, _user);
            return new LoginResponse
            {
                IsSucceed = true,
                Cookies = cookies
            };
        }

        protected override bool IsDailyCheckInEnabled => true;

        protected override void DailyCheck()
        {
            var html = GetCheckInPage();
            if (HasCheck(html))
            {
                var formhash = ParseHtml(html).DocumentNode.SelectSingleNode("//input[@name='formhash']")?.GetAttributeValue("value", null);
                var body = string.IsNullOrEmpty(formhash)
                    ? "checkinsubmit=1"
                    : $"formhash={Uri.EscapeDataString(formhash)}&checkinsubmit=1";
                HttpHelper.GetHtml(CHECKIN_API, false, _user.Cookies, body);
            }
        }

        private static bool HasCheck(string html)
        {
            return ParseHtml(html).DocumentNode.SelectSingleNode("//*[@name='checkinsubmit']") != null;
        }

        protected override void KeepOnline()
        {
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
                $"{HOST}/2b/api/mobile/index.php?mobile=no&version=1&module=login&loginsubmit=yes&loginfield=auto&submodule=checkpost&username={_user.UserName}&password={_user.Password}&questionid={_user.QuestionID}&answer={_user.Answer}",
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
