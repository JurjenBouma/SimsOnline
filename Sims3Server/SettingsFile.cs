using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sims3Server
{
    public static class SettingsFile
    {
        public static string ModPath = "";
        public static string Sims3Folder = "";
        public static string UpdateFolder = "";
        public static void Open()
        {
            if (File.Exists("Settings.cfg"))
            {
                string[] fileLines = File.ReadAllLines("Settings.cfg");
                ModPath = fileLines[0];
                Sims3Folder = fileLines[1];
                UpdateFolder = fileLines[2];
            }
        }
        public static void Save()
        {
            string[] fileLines = new string[3];
            fileLines[0] = ModPath;
            fileLines[1] = Sims3Folder;
            fileLines[2] = UpdateFolder;
            File.WriteAllLines("Settings.cfg", fileLines);
        }
    }
}
