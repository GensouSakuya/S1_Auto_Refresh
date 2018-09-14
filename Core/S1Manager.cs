using HtmlAgilityPack;

namespace Core
{
    public class S1Manager
    {
        public static void Refresh(UserInfo user)
        {
            var html = HttpHelper.GetHtml("https://bbs.saraba1st.com/2b", true, user.Cookies);
            if (!IsLogin(html))
            {
                Login(user);
            }
        }

        public static void Login(UserInfo loginModel)
        {
            HttpHelper.GetHtml(
                $"http://bbs.saraba1st.com/2b/api/mobile/index.php?mobile=no&version=1&module=login&loginsubmit=yes&loginfield=auto&submodule=checkpost&username={loginModel.UserName}&password={loginModel.Password}&questionID={loginModel.QuestionID}&answer={loginModel.Answer}",
                false, loginModel.Cookies);
        }

        public static bool IsLogin(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.GetElementbyId("um") != null;
        }
    }
}
