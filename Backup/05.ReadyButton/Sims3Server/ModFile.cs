using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Sims3Server
{
    public class ModFile
    {
        public string FilePath;
        public string ModName = "Mod";
        public string ExecutableName = "";
        public string Description = "";
        public List<string> PackageFiles = new List<string>();
        public string simsModFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Electronic Arts\\De Sims 3\\Mods";
        public string AppStartUpPath = "";

        public ModFile(string filePath, string appStartupPath)
        {
            FilePath = filePath;
            AppStartUpPath = appStartupPath;
            Read();
        }
        private void Read()
        {
            string[] commands = File.ReadAllLines(FilePath);
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].Substring(0, commands[i].IndexOf("=")) == "Package")
                {
                    string packageName = commands[i].Substring(commands[i].IndexOf("=") + 1);
                    PackageFiles.Add(packageName);
                }
                else if (commands[i].Substring(0, commands[i].IndexOf("=")) == "Name")
                {
                    ModName = commands[i].Substring(commands[i].IndexOf("=") + 1);
                }
                else if (commands[i].Substring(0, commands[i].IndexOf("=")) == "Description")
                {
                    Description = commands[i].Substring(commands[i].IndexOf("=") + 1);
                }
                else if (commands[i].Substring(0, commands[i].IndexOf("=")) == "Exe")
                {
                    ExecutableName = commands[i].Substring(commands[i].IndexOf("=") + 1);
                }
            }
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

            FileInfo fiMod = new FileInfo(FilePath);
            string modPath = fiMod.DirectoryName;
            foreach (string modFile in PackageFiles)
            {
                Directory.CreateDirectory(simsModFolder + "\\Packages");
                File.Copy(modPath + "\\" + modFile, simsModFolder + "\\Packages\\" + modFile);
            }
            File.Copy("Resource.cfg", simsModFolder + "\\Resource.cfg");

            string exePath = modPath + "\\" + ExecutableName;
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
