using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace XiWrap_Idle
{
    class Program
    {
        static void Main(string[] args)
        {
            var statusfile = @"C:\Program Files (x86)\Xibo Player\XiWrap\0.txt";
            var configfile = @"C:\Program Files (x86)\Xibo Player\XiWrap\time.txt";
            int timeout = 900000;
            string configtime = "15";

            //First run cleanup
            if (File.Exists(statusfile))
            {
                File.Delete(statusfile);
            }

            //Check if config file exists
            if (File.Exists(configfile))
            {
                //If blank delete and recreate.
                if (File.ReadAllLines(configfile).Skip(1).Take(1).FirstOrDefault() == null)
                {
                    File.Delete(configfile);
                    File.Create(configfile).Close();
                    TextWriter tw = new StreamWriter(configfile);
                    tw.WriteLine(@"//Enter the number of minutes before timeout on the second line of this file");
                    tw.WriteLine(configtime);
                    tw.Close();
                }
                //Else value exists.  Convert to miliseconds.
                else
                {
                    configtime = File.ReadAllLines(configfile).Skip(1).Take(1).First();
                    timeout = Convert.ToInt32(configtime) * 60000;
                }
            }
            //If config file is missing, create default one.
            else
            {
                File.Create(configfile).Close();
                TextWriter tw = new StreamWriter(configfile);
                tw.WriteLine(@"//Enter the number of minutes before timeout on the second line of this file");
                tw.WriteLine(configtime);
                tw.Close();
            }

            //Loop
            while (true)
            {
                var idle = IdleTimeFinder.GetIdleTime();

                //If high idle, create status file
                if (idle > timeout)
                {
                    File.Create(statusfile).Close();
                }

                //If low idle, delete status file
                if (idle < timeout)
                {
                    File.Delete(statusfile);
                }
                //Sleep
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
