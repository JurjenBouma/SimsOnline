using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SimsTcpClient
{
    public class ModFile
    {
        public int numFiles = 0;
        public List<FileInterface> PackageFiles = new List<FileInterface>();

        public ModFile()
        {
        }
        public bool CanLoadMod()
        {
            bool canSave = true;
            if (numFiles <= 0)
                canSave = false;
            if (numFiles != PackageFiles.Count)
                canSave = false;
            for (int i = 0; i < PackageFiles.Count; i++)
            {
                if (!PackageFiles[i].FileReady())
                {
                    canSave = false;
                }
            }
            return canSave;
        }

        public void LoadMod()
        {
            foreach (string file in Directory.GetFiles(SettingsFile.Sims3Folder+"\\Mods"))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, fi.Name + "bup");
            }
            foreach (string folder in Directory.GetDirectories(SettingsFile.Sims3Folder+"\\Mods"))
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                Directory.Move(folder, di.Name + "bup");
            }

            foreach (FileInterface file in PackageFiles)
            {
                Directory.CreateDirectory(SettingsFile.Sims3Folder+"\\Mods" + "\\Packages");
                file.SaveFile(SettingsFile.Sims3Folder + "\\Mods" + "\\Packages\\" + file.Name);
            }
            File.Copy("Resource.cfg", SettingsFile.Sims3Folder + "\\Mods" + "\\Resource.cfg");
        }
        public void UnLoadMod()
        {
            foreach (string file in Directory.GetFiles(SettingsFile.Sims3Folder + "\\Mods"))
            {
                File.Delete(file);
            }
            foreach (string folder in Directory.GetDirectories(SettingsFile.Sims3Folder + "\\Mods"))
            {
                Directory.Delete(folder, true);
            }
            foreach (string file in Directory.GetFiles(Globals.AppStartupPath))
            {
                if (file.EndsWith("bup"))
                {
                    FileInfo fi = new FileInfo(file);
                    File.Move(file, SettingsFile.Sims3Folder + "\\Mods" + "\\" + fi.Name.Substring(0, fi.Name.Length - 3));
                }
            }
            foreach (string folder in Directory.GetDirectories(Globals.AppStartupPath))
            {
                if (folder.EndsWith("bup"))
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    Directory.Move(folder, SettingsFile.Sims3Folder + "\\Mods" + "\\" + di.Name.Substring(0, di.Name.Length - 3));
                }
            }
        }
    }
}
