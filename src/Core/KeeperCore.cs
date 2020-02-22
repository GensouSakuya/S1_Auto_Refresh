using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class KeeperCore
    {
        static KeeperCore()
        {
            PluginHelper.Init(Configurations.PluginsPath);
        }

        private List<KeeperModel> _keeperInfos = new List<KeeperModel>();
        private static object _runningLock = new object();
        
        public void AddKeeper(string key,string initKey)
        {
            lock (_runningLock)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Cannot operate during runnning");
                }

                var plugin = PluginHelper.Find(key);
                if (plugin == null)
                {
                    throw new KeyNotFoundException($"plugin [{key}] is not found");
                }

                var newKeeper = new KeeperModel
                {
                    Key = key, InitKey = initKey
                };
                if (_keeperInfos.Any(p => p.Equals(newKeeper)))
                {
                    throw new InvalidOperationException("the keeper is existed");
                }

                _keeperInfos.Add(newKeeper);
            }
        }

        public void RemoveKeeper(string key, string initKey)
        {
            lock (_runningLock)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Cannot operate during runnning");
                }

                _keeperInfos.RemoveAll(p => p.Key == key && p.InitKey == initKey);
            }
        }

        public void CleanKeepers()
        {
            lock (_runningLock)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Cannot operate during runnning");
                }

                _keeperInfos.Clear();
            }
        }

        public bool IsRunning { get; private set; } = false;

        private void PrepareKeeper()
        {
            _keeperInfos.ForEach(p => p.Keeper = null);
            _keeperInfos.ForEach(p =>
            {
                try
                {
                    var keeper = PluginHelper.GetKeepper(PluginHelper.Find(p.Key), p.InitKey);
                    keeper.LogEvent += (s,e)=>
                    {
                        LogHelper.WriteLog(e);
                    };
                    p.Keeper = keeper;
                }
                catch(Exception e)
                {
                    LogHelper.WriteLog($"Keeper[{p.Key}] preparation is failed, message:{e.Message}");
                }
            });
        }

        public void Start()
        {
            lock (_runningLock)
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    PrepareKeeper();
                    _keeperInfos.ForEach(p => p.Keeper?.Start());
                }
            }
        }

        public void Stop()
        {
            lock (_runningLock)
            {
                if (IsRunning)
                {
                    _keeperInfos.ForEach(p => p.Keeper?.Stop());
                    IsRunning = false;
                }
            }
        }
    }
}
