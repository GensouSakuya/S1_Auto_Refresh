using PluginTemplate;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plugins.ManHuaBuDang
{
    [PluginKey("Plugins.ManHuaBuDang")]
    [PluginName("漫画补档")]
    public class ManHuaBuDangKeeper : AbstractForumKeepper
    {
        public ManHuaBuDangKeeper(string initKey) : base(initKey)
        {
        }

        protected override int KeepOnlineIntervalSeconds => 240;
        protected override bool IsDailyCheckInEnabled => true; 

        protected override bool IsLogin()
        {
            var html = HttpHelper.GetHtml("https://www.manhuabudang.com/index.php", true, _user.Cookies);
            return html.Contains(_user.UserName);
        }

        protected override void KeepOnline()
        {
            if (!IsLogin())
            {
                Login();
            }
        }

        protected override void DailyCheck()
        {
            var html = HttpHelper.GetHtml("https://www.manhuabudang.com/u.php", true, _user.Cookies);
            if (HasCheck(html))
            {
                var hashUrl = GetCheckFormHashUrl(html);
                HttpHelper.GetHtml($"https://www.manhuabudang.com/jobcenter.php?action=punch&verify={hashUrl}&step=2", false, _user.Cookies);
            }
        }
        private readonly static Regex CheckRegex = new Regex(@"punchJob\([0-9]+\)");


        protected static bool HasCheck(string html)
        {
            return CheckRegex.IsMatch(html);// html.Contains("punchJob(2);");
        }
        protected static string GetCheckFormHashUrl(string html)
        {
            var regex = new Regex("var verifyhash = \'[a-z0-9]+\'");
            var splitedHtml = regex.Match(html).Value?.Split('\'');
            var checkMatchedCount = splitedHtml?.Count() ?? 0;
            if (checkMatchedCount > 1)
            {
                return System.Web.HttpUtility.HtmlDecode(splitedHtml[1]);
            }
            return null;
        }

        protected override void Login()
        {
            HttpHelper.GetHtml($"https://www.manhuabudang.com/login.php?", false, _user.Cookies,
                $"forward=&jumpurl=https%3A%2F%2Fwww.manhuabudang.com%2Findex.php&step=2&lgt=0&pwuser={_user.UserName}&pwpwd={_user.Password}&question=0&customquest=&answer=&hideid=0&cktime=31536000&submit=");
        }
    }
}
