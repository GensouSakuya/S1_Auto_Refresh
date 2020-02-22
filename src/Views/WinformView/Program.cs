using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var _ = typeof(System.Runtime.AssemblyTargetedPatchBandAttribute);
            StreamWriter.Null.WriteLine(_);
            _ = typeof(System.Net.Http.ByteArrayContent);
            StreamWriter.Null.WriteLine(_);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
