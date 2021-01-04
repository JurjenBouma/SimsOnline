using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SimsTcpClient
{
    public class ModFile
    {
        public int numFiles = 0;
        public List<FileInterface> PackageFiles = new List<FileInterface>();
        public string simsModFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Electronic Arts\\De Sims 3\\Mods";
        public string AppStartUpPath = "";

        public ModFile(string appStartupPath)
        {
            AppStartUpPath = appStartupPath;
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
            foreach (string file in Directory.GetFiles(simsModFolder))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, fi.Name + "bup");
            }
            foreach (string folder in Directory.GetDirectories(simsModFolder))
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                Directory.Move(folder, di.Name + "bup");
            }

            foreach (FileInterface file in PackageFiles)
            {
                Directory.CreateDirectory(simsModFolder + "\\Packages");
                file.SaveFile(simsModFolder + "\\Packages\\" + file.Name);
            }
            File.Copy("Resource.cfg", simsModFolder + "\\Resource.cfg");
        }
        public void UnLoadMod()
        {
            foreach (string file in Directory.GetFiles(simsModFolder))
            {
                File.Delete(file);
            }
            foreach (string folder in Directory.GetDirectories(simsModFolder))
            {
                Directory.Delete(folder, true);
            }
            foreach (string file in Directory.GetFiles(AppStartUpPath))
            {
                if (file.EndsWith("bup"))
                {
                    FileInfo fi = new FileInfo(file);
                    File.Move(file, simsModFolder + "\\" + fi.Name.Substring(0, fi.Name.Length - 3));
                }
            }
            foreach (string folder in Directory.GetDirectories(AppStartUpPath))
            {
                if (folder.EndsWith("bup"))
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    Directory.Move(folder, simsModFolder + "\\" + di.Name.Substring(0, di.Name.Length - 3));
                }
            }
        }
    }
}
