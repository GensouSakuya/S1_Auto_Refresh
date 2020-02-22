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

        public List<KeeperInfoModel> LoadedKeepers =>
            PluginHelper.LoadedPlugins.Select(p => new KeeperInfoModel
            {
                Key = p.Key, Name = p.Name
            }).ToList();

        public List<KeeperModel> KeeperInfos { get; } = new List<KeeperModel>();
        private static object _runningLock = new object();
        
        public KeeperModel AddKeeper(string key,string initKey)
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
                if (KeeperInfos.Any(p => p.Equals(newKeeper)))
                {
                    throw new InvalidOperationException("the keeper is existed");
                }

                KeeperInfos.Add(newKeeper);

                return newKeeper;
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

                KeeperInfos.RemoveAll(p => p.Key == key && p.InitKey == initKey);
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

                KeeperInfos.Clear();
            }
        }

        public bool IsRunning { get; private set; } = false;

        private void PrepareKeeper()
        {
            KeeperInfos.ForEach(p => p.Keeper = null);
            KeeperInfos.ForEach(p =>
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
                    KeeperInfos.ForEach(p => p.Keeper?.Start());
                }
            }
        }

        public void Stop()
        {
            lock (_runningLock)
            {
                if (IsRunning)
                {
                    KeeperInfos.ForEach(p => p.Keeper?.Stop());
                    IsRunning = false;
                }
            }
        }
    }
}
