using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using TcpMessageCodes;
using MessageCodes;

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
        private void OnUpdateInfoReceived(int numDlls)
        {
            Update.nDlls = numDlls;
        }
        private void OnWorldInfoReceived(int numFiles)
        {
            world.numWorldFiles = numFiles;
            world.WorldFileName = "SimsOnlineModTempWorld.sims3";
        }
        private void OnStartGamePlay()
        {
            byte[] message  = System.BitConverter.GetBytes((uint)MessageIDs.StartGame);
            bufferManager.sendBuffer.Add(message);
        }
        private void OnGameMessageServer(byte[] messages)
        {
            bufferManager.SendGameMessages(messages);
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
                Update.ExeFile = ReceivedFiles[index];
                ReceivedFiles.RemoveAt(index);
                if(Update.SaveFiles())
                {
                    OnUpdateReady();
                }
            }
            if (ReceivedFiles[index].type == FileTypes.Dll)
            {
                Update.Dlls.Add(ReceivedFiles[index]);
                ReceivedFiles.RemoveAt(index);
                if (Update.SaveFiles())
                {
                    OnUpdateReady();
                }
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
