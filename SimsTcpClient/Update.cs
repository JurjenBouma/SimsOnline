using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SimsTcpClient
{
    public static class Update
    {
        public static int nDlls = -1;
        public static FileInterface ExeFile = new FileInterface();
        public static List<FileInterface> Dlls = new List<FileInterface>();

        public static bool IsUpdateReady()
        {
            if(!ExeFile.FileReady())
                return false;
            foreach(FileInterface dll in Dlls)
                if(!dll.FileReady())
                    return false;
            if(Dlls.Count != nDlls)
                return false;
            return true;
        }

        public static bool SaveFiles()
        {
            if (IsUpdateReady())
            {
                ExeFile.SaveFile(Globals.AppStartupPath + "\\" + ExeFile.Name + "TempExe");
                for (int i = 0; i < Dlls.Count;i++ )
                {
                    Dlls[i].SaveFile(Globals.AppStartupPath + "\\" + Dlls[i].Name + "TempDll");
                }
                return true;
            }
            return false;
        }
    }
}
