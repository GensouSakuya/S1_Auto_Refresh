using System.IO;
using System.Net;
using System.Text;

namespace Plugins.S1
{
    internal class HttpHelper
    {
        public static string GetHtml(string url,bool isGet, CookieContainer cookies)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);

            if (cookies.Count > 0)
            {
                req.CookieContainer = cookies;
            }
            else
            {
                req.CookieContainer = new CookieContainer();
            }

            req.Method = isGet ? "GET" : "POST";

            var res = (HttpWebResponse)req.GetResponse();
            string html = "";
            using (var st = res.GetResponseStream())
            {
                var reader = new StreamReader(st, Encoding.UTF8);
                html = reader.ReadToEnd();
            }
            if (cookies.Count == 0)
            {
                foreach (Cookie cookie in res.Cookies)
                {
                    cookies.Add(cookie);
                }
            }

            return html;
        }
    }
}
