using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimsTcpClient
{
    public static class SettingsFile
    {
        public static string Sims3Folder = "";
        public static string UserName = "";
        public static string IPAdress ="";

        public static void Open()
        {
            if(File.Exists("Settings.cfg"))
            {
                string[] fileLines = File.ReadAllLines("Settings.cfg");
                Sims3Folder = fileLines[0];
                UserName = fileLines[1];
                IPAdress = fileLines[2];
            }
        }
        public static void Save()
        {
            string[] fileLines = new string[3];
            fileLines[0] = Sims3Folder;
            fileLines[1] = UserName;
            fileLines[2] = IPAdress;
            File.WriteAllLines("Settings.cfg", fileLines);
        }
    }
}
