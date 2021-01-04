using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using TcpMessageCodes;

namespace SimsTcpClient
{
    public partial class OnlineMod
    {
        private void OnServerConnected()
        {
            tcpManager.SendExeInfo(AppStartupPath + "\\" + AppName + ".exe");
            MessageEvent("Connected to Server.");
        }
        private void OnModInfoReceived(int numFiles)
        {
            modFile.numFiles = numFiles;
        }
        private void OnWorldInfoReceived(int numFiles)
        {
            world.numWorldFiles = numFiles;
            world.WorldFileName = "SimsOnlineModTempWorld";
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
        private void HandleCompletedFile(int index)
        {
            if (ReceivedFiles[index].type == FileTypes.ExeFile)
            {
                MessageEvent("Restarting Program.");
                ReceivedFiles[index].SaveFile(AppStartupPath + "\\" + AppName + "Temp.exe");
                Thread.Sleep(1000);
                Process.Start(AppStartupPath + "\\" + AppName + "Temp.exe");
                OnUpdateReady();
            }
            if (ReceivedFiles[index].type == FileTypes.ModFile)
            {
                modFile.PackageFiles.Add(ReceivedFiles[index]);
                ReceivedFiles.RemoveAt(index);
                if (modFile.CanLoadMod())
                {
                    MessageEvent("Mod Download Completed!");
                }
            }
            if (ReceivedFiles[index].type == FileTypes.WorldFile)
            {
                world.WorldFiles.Add(ReceivedFiles[index]);
                ReceivedFiles.RemoveAt(index);
                if (world.CanSaveWorld())
                {
                    MessageEvent("World Download Completed!");
                }
            }
        }
    }
}
