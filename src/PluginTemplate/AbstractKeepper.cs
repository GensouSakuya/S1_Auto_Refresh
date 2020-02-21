using System;
using System.Threading;

namespace PluginTemplate
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class AbstractKeepper
    {
        #region KeepOnline

        public AbstractKeepper(string initKey) { }

        public abstract string GetInitKey();
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
                _lastRefreshTime = DateTime.Now;
            }
            catch (Exception e)
            {
                Log($"KeepOnlineAction occured exceptions: {e.Message}");
            }
        }

        protected DateTime _lastRefreshTime;
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
                    Log($"KeepOnlineAction occured exceptions: {e.Message}");
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
                Log($"Process occured exceptions: {e.Message}");
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

        protected string Message;
    }
}
