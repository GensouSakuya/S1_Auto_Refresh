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
        private UserInfo _user;

        public UserInfo User
        {
            get
            {
                if (_user == null)
                {
                    throw new Exception("user is empty");
                }
                return _user;
            }
        }

        private volatile object lockObj = new object();
        private Thread _thread { get; set; }
        private bool _isStarted { get; set; } = false;

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

        public Refresher(string username, string password, int questionID = 0, string answer = null)
        {
            _user = new UserInfo(username, password, questionID, answer);
        }

        public void Start()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                _thread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        S1Manager.Refresh(_user);
                        Thread.Sleep(240000);
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
