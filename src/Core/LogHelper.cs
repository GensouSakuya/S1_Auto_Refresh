﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace Core
{
    internal class LogHelper
    {
        private static string _logFileName => $"corelog{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
        private static volatile object fileLock = new object();

        public static void WriteLog(string log)
        {
            lock (fileLock)
            {
                var path = Directory.GetCurrentDirectory();
                var fullFileName = Path.Combine(path, _logFileName);
                if (!File.Exists(fullFileName))
                {
                    var f = File.Create(fullFileName);
                    f.Close();
                }

                using (var st = new FileStream(fullFileName, FileMode.Open))
                {
                    st.Position = st.Length;
                    using (var sw = new StreamWriter(st))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff："));
                        sw.WriteLine(log);
                        sw.WriteLine("===============================================================");
                    }
                }
            }
        }

        public static void WriteLog(Exception e)
        {
            WriteLog(JsonConvert.SerializeObject(e));
        }
    }
}
