using System.IO;
using System.Net;
using System.Text;

namespace Core
{
    public class HttpHelper
    {
        public static string GetHtml(string url,bool isGet, CookieContainer cookies,string body = null, CustomEncoding encoding =CustomEncoding.UTF8 )
        {
            var req = (HttpWebRequest)WebRequest.Create(url);

            if (cookies.Count>0)
            {
                req.CookieContainer = cookies;
            }
            else
            {
                req.CookieContainer = new CookieContainer();
            }

            req.Method = isGet ? "GET" : "POST";
            if (!isGet && body != null)
            {
                byte[] bs = Encoding.ASCII.GetBytes(body);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
            }

            var res = (HttpWebResponse)req.GetResponse();
            string html = "";
            using (var st = res.GetResponseStream())
            {
                var reader = new StreamReader(st, encoding.GetEncoding());
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

    public enum CustomEncoding
    {
        UTF8=1,
        GBK=2
    }

    public static class CustomEncoudingExtension
    {
        public static Encoding GetEncoding(this CustomEncoding encoding)
        {
            switch (encoding)
            {
                case CustomEncoding.GBK:
                    return Encoding.GetEncoding("GBK");
                case CustomEncoding.UTF8:
                    return Encoding.UTF8;
            }

            throw new System.Exception("不支持指定编码");
        }
    }
}
