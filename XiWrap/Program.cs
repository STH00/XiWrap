using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace XiWrap
{
    class Program
    {
        static void Main(string[] args)
        {
            // Status file creator
            ProcessAsUser.Launch(@"C:\Program Files (x86)\Xibo Player\XiWrap\XiWrap_Idle.exe");

            while (true)
            {
                // Idle process watchdog
                var idleWatch = Process.GetProcesses().Where(pr => pr.ProcessName.ToLower().Contains("xiwrap_idle"));
                if (idleWatch.Count() > 1)
                {
                    foreach (var process in idleWatch)
                    {
                        process.Kill();
                    }
                }
                else if (idleWatch.Count() < 1)
                {
                    ProcessAsUser.Launch(@"C:\Program Files (x86)\Xibo Player\XiWrap\XiWrap_Idle.exe");
                }

                // Variable containing process containing Xibo
                var xiboProcess = Process.GetProcesses().Where(pr => pr.ProcessName.ToLower().Contains("xibo"));
                // Status file path
                var statusfile = @"C:\Program Files (x86)\Xibo Player\XiWrap\0.txt";

                // If there is a Xibo process
                if (xiboProcess.Count() > 0)
                {
                    // If not idle
                    if (!File.Exists(statusfile))
                    {
                        // Kill every Xibo process
                        foreach (var process in xiboProcess)
                        {
                            process.Kill();
                        }
                    }
                }

                // If there is not a Xibo process
                if (xiboProcess.Count() < 1)
                {
                    // If idle
                    if (File.Exists(statusfile))
                    {
                        ProcessAsUser.Launch(@"C:\Program Files (x86)\Xibo Player\Xibo.exe");
                        //Process.Start(@"C:\Program Files (x86)\Xibo Player\Xibo.exe");
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
