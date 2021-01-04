using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimsTcpClient
{
    public class World
    {
        public string WorldFileName;
        public int numWorldFiles = 0;
        public List<FileInterface> WorldFiles = new List<FileInterface>();
        public bool WorldSaved = false;

        public bool CanSaveWorld()
        {
            bool canSave = true;
            if (numWorldFiles <= 0)
                canSave = false;
            if (numWorldFiles != WorldFiles.Count)
                canSave = false;
            for(int i =0;i < WorldFiles.Count;i++)
            {
                if(!WorldFiles[i].FileReady())
                {
                    canSave = false;
                }
            }
            return canSave;
        }

        public void SaveWorld()
        {
            if (CanSaveWorld())
            {
                string savePath = SettingsFile.Sims3Folder + "\\Saves\\" + WorldFileName;
                if (!WorldSaved)
                {
                    if (Directory.Exists(savePath))
                        Directory.Delete(savePath, true);
                    Directory.CreateDirectory(savePath);
                    for (int i = 0; i < WorldFiles.Count; i++)
                    {
                        WorldFiles[i].SaveFile(savePath + "\\" + WorldFiles[i].Name);
                    }
                    WorldSaved = true;
                }
            }
        }
    }
}
