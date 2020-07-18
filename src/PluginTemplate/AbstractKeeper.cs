using System;
using System.Linq;
using System.Threading;

namespace PluginTemplate
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class AbstractKeeper
    {
        private object _nameLock = new object();
        private string _keeperName = null;

        private string KeeperName
        {
            get
            {
                if (_keeperName == null)
                {
                    lock (_nameLock)
                    {
                        if (_keeperName == null)
                        {
                            _keeperName = ((PluginNameAttribute) this.GetType().GetCustomAttributes(typeof(PluginNameAttribute), true)
                                .FirstOrDefault())?.Name;
                        }

                        if (_keeperName == null)
                        {
                            _keeperName = this.GetType().FullName;
                        }
                    }
                }
                return _keeperName;
            }
        }

        #region KeepOnline

        public string InitKey { get; private set; }

        public AbstractKeeper(string initKey)
        {
            InitKey = initKey;
        }

        //因为设计上是在保持在线的同时检查每日签到情况
        //所以不增加用来控制是否维持在线状态的配置

        /// <summary>
        /// 维持在线刷新时间间隔
        /// </summary>
        protected virtual int KeepOnlineIntervalSeconds => 60 * 5;

        protected abstract void KeepOnline();

        private void _keepOnline()
        {
            try
            {
                KeepOnline();
                LastRefreshTime = DateTime.Now;
            }
            catch (Exception e)
            {
                Log($"[{KeeperName}]KeepOnlineAction occured exceptions: {e.Message}\n{e.StackTrace}");
            }
        }

        public DateTime LastRefreshTime { get; protected set; }
        #endregion

        #region CheckIn

        /// <summary>
        /// 是否启用每日签到
        /// </summary>
        protected virtual bool IsDailyCheckInEnabled => false;
        private DateTime LastCheckDate { get; set; }
        private bool _isNeedCheck => DateTime.Today > LastCheckDate;

        protected virtual void DailyCheck() { }
        private void _dailyCheckAction()
        {
            if (_isNeedCheck)
            {
                try
                {
                    DailyCheck();
                    LastCheckDate = DateTime.Today;
                }
                catch (Exception e)
                {
                    Log($"[{KeeperName}]DailyCheck occured exceptions: {e.Message}\n{e.StackTrace}");
                }
            }
        }
        #endregion

        private void Process()
        {
            try
            {
                _keepOnline();

                if (IsDailyCheckInEnabled)
                {
                    _dailyCheckAction();
                }
            }
            catch (Exception e)
            {
                Log($"[{KeeperName}]Process occured exceptions: {e.Message}");
            }
        }

        public EventHandler<string> LogEvent;

        protected void Log(string message)
        {
            LogEvent?.Invoke(this, message);
        }

        private Thread _thread;
        private bool _isStopping = false;

        protected virtual void Starting() { }
        public void Start()
        {
            Starting();
            _isStopping = false;
            if (_thread != null)
            {
                if (_thread.IsAlive)
                {
                    _thread.Abort();
                }
                _thread = null;
            }
            _thread = new Thread(new ThreadStart(() =>
            {
                while (!_isStopping)
                {
                    Process();
                    Thread.Sleep(KeepOnlineIntervalSeconds * 1000);
                }
            }));
            _thread.Start();
        }

        protected virtual void Stopping() { }

        public void Stop()
        {
            Stopping();
            _isStopping = true;
        }

        public string Message{ get; protected set; }
    }
}
