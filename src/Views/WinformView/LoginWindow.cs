using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.Windows.Forms;

namespace ForumTool.Winform
{
    public partial class LoginWindow : Form
    {
        private string _url;
        private CookieContainer _cookieContainer = new CookieContainer();
        public CookieContainer Cookies => _cookieContainer;
        public ChromiumWebBrowser _chromeBrowser;
        public LoginWindow(string keeperKey, string userName, string url)
        {
            _url = url;
            InitializeComponent();
            this.Text = $"{keeperKey}-{userName}";
            this.Load += LoginWindow_Load;
            this.FormClosed += LoginWindow_FormClosed;
        }

        private void LoginWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            using(var cookiemanager = _chromeBrowser.GetCookieManager())
            using(var visitor = new TaskCookieVisitor())
            {
                cookiemanager.VisitAllCookies(visitor);
                var list = visitor.Task.GetAwaiter().GetResult();
                list.ForEach(c =>
                {
                    _cookieContainer.Add(new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain));
                });
            }
        }

        private void LoginWindow_Load(object sender, System.EventArgs e)
        {
            InitCef();
        }

        public void InitCef()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            _chromeBrowser = new ChromiumWebBrowser(_url);
            this.Controls.Add(_chromeBrowser);
            _chromeBrowser.Dock = DockStyle.Fill;
        }
    }
}
