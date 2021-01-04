using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sims3Server
{
    public static class Update
    {
        public static string UpdatePath;
        public static string ExeName;
        public static List<string> DllNames;

        public static void GetUpdateFiles()
        {
            if (SettingsFile.UpdateFolder.Length > 0)
            {
                UpdatePath = SettingsFile.UpdateFolder;
                string[] files = Directory.GetFiles(UpdatePath);
                DllNames = new List<string>();
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Extension == ".exe")
                        ExeName = fi.Name;
                    if (fi.Extension == ".dll")
                        DllNames.Add(fi.Name);
                }
            }
        }
    }
}
