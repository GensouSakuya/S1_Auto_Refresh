using System;
using System.Threading;

namespace Core
{
    public abstract class BaseForumManager
    {
        public virtual int IntervalSeconds { get; } = 5;
        //public virtual void Refresh()
        //{

        //}
        #region Login
        public abstract bool IsLogin { get; }
        public virtual void Login()
        {

        }

        private void _login()
        {
            if(!IsLogin)
            {
                Login();
                if(!IsLogin)
                {
                    throw new Exception("登录方法失效");
                }
            }
        }
        #endregion

        #region CheckIn
        public abstract bool EnableAutoDailyCheckIn { get; }
        public virtual bool Check()
        {
            return true;
        }

        private DateTime LastCheckDate { get; set; }
        private bool IsNeedCheck => DateTime.Today > LastCheckDate;
        private void _check()
        {
            if (IsNeedCheck)
            {
                if (Check())
                {
                    LastCheckDate = DateTime.Today;
                }
            }
        }
        #endregion

        public virtual void Process()
        {
            _login();
            _check();
            Thread.Sleep(IntervalSeconds);
        }
    }

    public enum ForumType
    {
        //S1
        S1 = 1,
        //漫画补档
        漫画补档 = 2
    }
}
