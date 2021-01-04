using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            string exePath = "";
            foreach (string file in files)
            {
                if (file.EndsWith("TempDll"))
                {
                    bool Replaced = false;
                    while (!Replaced)
                    {
                        try
                        {
                            File.Copy(file, file.Substring(0, file.Length - "Tempdll".Length), true);
                            File.Delete(file);
                            Replaced = true;
                        }
                        catch { }
                    }
                }
                if (file.EndsWith("TempExe"))
                {
                     bool Replaced = false;
                     while (!Replaced)
                     {
                         try
                         {
                             File.Copy(file, file.Substring(0, file.Length - "TempExe".Length), true);
                             File.Delete(file);
                             exePath = file.Substring(0, file.Length - "TempExe".Length);
                             Replaced = true;
                         }
                         catch { }
                     }
                }
            }
            if(exePath.Length > 0)
                Process.Start(exePath, "Login");
        }
    }
}
