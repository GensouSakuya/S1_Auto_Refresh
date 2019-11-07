namespace Core
{
    public abstract class BaseForumManager
    {
        public abstract bool EnableAutoRefresh { get; }
        public virtual int AutoRefreshTimeInterval { get; } = 5;
        public virtual void Refresh()
        {

        }

        public virtual void Login()
        {

        }

        public abstract bool EnableAutoDailyCheckIn { get; set; }
        public virtual void Check()
        {

        }
    }
}
