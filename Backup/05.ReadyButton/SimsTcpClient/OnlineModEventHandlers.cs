using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using TcpMessageCodes;

namespace SimsTcpClient
{
    public partial class OnlineMod
    {
        private void OnServerConnected()
        {
            UpdatePlayerData();
            tcpManager.SendExeInfo(Globals.AppStartupPath + "\\" + Globals.AppName + ".exe");
            MessageEvent("Connected to Server.");
        }
        private void OnModInfoReceived(int numFiles)
        {
            modFile.numFiles = numFiles;
        }
        private void OnWorldInfoReceived(int numFiles)
        {
            world.numWorldFiles = numFiles;
            world.WorldFileName = "SimsOnlineModTempWorld.sims3";
        }
        private void OnFileHeaderReceived(int numPackages, string name, FileTypes type, double id)
        {
            FileInterface file = new FileInterface();
            file.Name = name;
            file.NumPackages = numPackages;
            file.id = id;
            file.type = type;
            ReceivedFiles.Add(file);
            if(type == FileTypes.ExeFile)
                MessageEvent("Updating Program.");
            else
                MessageEvent("Downloading file " + file.Name);
        }
        private void OnFileDataReceived(int packageIndex,byte[] data,double id)
        {
            for (int i = 0; i < ReceivedFiles.Count; i++)
            {
                if (ReceivedFiles[i].id == id)
                {
                    ReceivedFiles[i].fileData.InsertRange(packageIndex, data);
                    ReceivedFiles[i].PackagesReceived++;
                    if (ReceivedFiles[i].FileReady())
                    {
                        HandleCompletedFile(i);
                    }
                }
            }
        }
        private void OnLaunchGame()
        {
            LoadMod();
        }
        private void HandleCompletedFile(int index)
        {
            if (ReceivedFiles[index].type == FileTypes.ExeFile)
            {
                MessageEvent("Restarting Program.");
                ReceivedFiles[index].SaveFile(Globals.AppStartupPath + "\\" + Globals.AppName + "Temp.exe");
                Thread.Sleep(1000);
                Process.Start(Globals.AppStartupPath + "\\" + Globals.AppName + "Temp.exe");
                OnUpdateReady();
            }
            if (ReceivedFiles[index].type == FileTypes.ModFile)
            {
                modFile.PackageFiles.Add(ReceivedFiles[index]);
                ReceivedFiles.RemoveAt(index);
                if (modFile.CanLoadMod())
                {
                    MessageEvent("Mod Download Completed!");
                    if (world.CanSaveWorld())
                    {
                        canLaunch = true;
                        MessageEvent("Press the READY button when ready.");
                    }
                }
            }
            if (ReceivedFiles[index].type == FileTypes.WorldFile)
            {
                world.WorldFiles.Add(ReceivedFiles[index]);
                ReceivedFiles.RemoveAt(index);
                if (world.CanSaveWorld())
                {
                    MessageEvent("World Download Completed!");
                    if (modFile.CanLoadMod())
                    {
                        canLaunch = true;
                        MessageEvent("Press the READY button when ready.");
                    }
                }
            }
        }
    }
}
