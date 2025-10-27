using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.Windows.Forms;

namespace ForumTool.Winform
{
    public partial class LoginWindow : Form
    {
        private string _url;
        private CookieContainer _cookieContainer;
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
            if (_pageChangedCount>=1)
            {
                _cookieContainer = new CookieContainer();
                using (var cookiemanager = _chromeBrowser.GetCookieManager())
                using (var visitor = new TaskCookieVisitor())
                {
                    cookiemanager.VisitAllCookies(visitor);
                    var list = visitor.Task.GetAwaiter().GetResult();
                    list.ForEach(c =>
                    {
                        _cookieContainer.Add(new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain));
                    });
                }
            }
        }

        private void LoginWindow_Load(object sender, System.EventArgs e)
        {
            InitCef();
        }

        public void InitCef()
        {
            CefSettings settings = new CefSettings()
            {
                //todo send ua to keeper
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36 Edg/118.0.2088.44"
            };
            if(Cef.IsInitialized == false)
                Cef.Initialize(settings);
            _chromeBrowser = new ChromiumWebBrowser(_url);
            this.Controls.Add(_chromeBrowser);
            _chromeBrowser.Dock = DockStyle.Fill;
            _chromeBrowser.AddressChanged += _chromeBrowser_AddressChanged;
        }

        private int _pageChangedCount = -1;

        private void _chromeBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            _pageChangedCount++;
        }
    }
}
