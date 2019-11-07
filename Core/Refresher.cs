using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class Refresher
    {
        private const int _maxRetryCount = 3;

        private BaseForumManager Manager;

        public readonly UserInfo User;

        private volatile object lockObj = new object();
        private Thread _thread { get; set; }
        private bool _isStarted { get; set; } = false;
        private DateTime _lastCollectTime { get; set; }

        private Thread MyThread
        {
            get
            {
                return _thread;
            }
            set
            {
                if (_thread != null)
                {
                    _thread.Abort();
                }

                _thread = value;
            }
        }

        public Refresher(string username, string password, int questionID = 0, string answer = null,ForumType type = ForumType.S1)
        {
            User = new UserInfo(username, password, questionID, answer, type);
            switch (type)
            {
                case ForumType.S1:
                    Manager = new S1Manager(User);
                    break;
                default:
                    throw new Exception("不支持的论坛类型");
            }
        }

        public void Start()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                _lastCollectTime = DateTime.Now;
                _thread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        try
                        {
                            for (int retryCount = 0; retryCount < _maxRetryCount; retryCount++)
                            {
                                try
                                {
                                    Manager.Process();
                                    break;
                                }
                                catch (Exception e)
                                {
                                    FileLogHelper.WriteLog(e);
                                }
                            }
                            if (DateTime.Now.Subtract(_lastCollectTime).Hours > 2)
                            {
                                _lastCollectTime = DateTime.Now;
                                GC.Collect();
                            }
                            Thread.Sleep(240000);
                        }
                        catch (Exception e)
                        {
                            FileLogHelper.WriteLog(e);
                        }
                    }
                }));
                _thread.Start();
            }
        }

        public void Stop()
        {
            if (_isStarted)
            {
                _isStarted = false;
                _thread.Abort();
                _thread = null;
            }
        }
    }
}
