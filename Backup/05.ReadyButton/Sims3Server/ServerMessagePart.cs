using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TcpMessageCodes;
using System.Diagnostics;

namespace Sims3Server
{
    public partial class Sims3Server
    {
        public void DecodeMessage(byte[] message,int clientID)
        {
            MemoryStream messageStream = new MemoryStream(message);
            BinaryReader reader = new BinaryReader(messageStream);

            int messageID = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
            if(messageID == (int)ClientMessages.PlayerInfo)
            {         
                int nameLenght = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                string name = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(nameLenght));
                bool ready = BitConverter.ToBoolean(reader.ReadBytes(sizeof(bool)),0);

                playerList[clientID].Name = name;
                playerList[clientID].IsReady = ready;
                SendPlayerData();
                LaunchIfReady();
            }
            else if (messageID == (int)ClientMessages.ExeInfo)
            {
                int majorVersion = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int minorVersion = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int buildVersion = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int privateVersion = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);

                if (!SendExe(clientID, majorVersion, minorVersion, buildVersion, privateVersion))
                {
                    SendWorldFiles(clientID);
                    SendPackageFiles(clientID);
                }
            }
        }
        public void SendPlayerData()
        {
            PingAllPlayers();
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ServerMessages.PlayerData));
            message.AddRange(BitConverter.GetBytes(playerList.Count));
            for (int i = 0; i < playerList.Count; i++)
            {
                message.AddRange(BitConverter.GetBytes(playerList[i].Name.Length));
                message.AddRange(ASCIIEncoding.ASCII.GetBytes(playerList[i].Name));
                message.AddRange(BitConverter.GetBytes(playerList[i].IsReady));
                message.AddRange(BitConverter.GetBytes(playerList[i].clientSocket.GetClientID()));
            }
            foreach(Player player in playerList)
            {
                player.clientSocket.SendMessage(message.ToArray());
            }
        }
        private bool SendExe(int receiverID ,int majorVersion,int minorVersion,int buildVersion, int privateVersion)
        {
            FileInfo fi = new FileInfo(modFile.FilePath);
            string exePath = fi.DirectoryName + "\\" + modFile.ExecutableName;
            FileVersionInfo fVI = FileVersionInfo.GetVersionInfo(exePath);
            if(fVI.ProductMajorPart != majorVersion || fVI.ProductMinorPart != minorVersion || 
                fVI.ProductBuildPart != buildVersion ||  fVI.ProductPrivatePart != privateVersion)
            {
                Stream fileStream = new FileStream(exePath, FileMode.Open);
                BinaryReader fileReader = new BinaryReader(fileStream);
                byte[] fileBytes = fileReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                int PackageSize = 50;
                int numPackages = (fileBytes.Length + PackageSize - 1) / PackageSize;
                SendFile(receiverID, PackageSize, fileBytes,"ExeFile", FileTypes.ExeFile);
                fileStream.Close();
                return true;
            }
            return false;
        }
        private void SendFile(int receiverID, int PackageSize, byte[] fileData,string fileName, FileTypes fileType)
        {
            Random rng = new Random();
            double fileID = rng.NextDouble();
            int numPackages = (fileData.Length + PackageSize - 1) / PackageSize;
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ServerMessages.FileHeader));
            message.AddRange(BitConverter.GetBytes((int)fileType));
            message.AddRange(BitConverter.GetBytes(fileName.Length));
            message.AddRange(ASCIIEncoding.ASCII.GetBytes(fileName));
            message.AddRange(BitConverter.GetBytes(numPackages));
            message.AddRange(BitConverter.GetBytes(fileID));
            playerList[receiverID].clientSocket.SendMessage(message.ToArray());

            for (int i = 0; i < numPackages; i++)
            {
                int packageSize = PackageSize;
                if (i == numPackages - 1)
                    packageSize = fileData.Length % PackageSize;

                message = new List<byte>();
                message.AddRange(BitConverter.GetBytes((int)ServerMessages.FileData));
                message.AddRange(BitConverter.GetBytes(fileID));
                message.AddRange(BitConverter.GetBytes(i * PackageSize));
                message.AddRange(BitConverter.GetBytes(packageSize));
                for (int b = 0; b < packageSize; b++)
                    message.Add(fileData[i * PackageSize + b]);
                playerList[receiverID].clientSocket.SendMessage(message.ToArray());
            }
        }

        public void LaunchIfReady()
        {
            if(CanLauch())
            {
                foreach (Player player in playerList)
                {
                    SendShortMessage(player.clientSocket.GetClientID(), ServerMessages.LaunchGame);
                }
            }
        }
       
        public void SendPackageFiles(int receiverID)
        {
            FileInfo fi = new FileInfo(modFile.FilePath);
            string modPath = fi.DirectoryName;
            SendModInfo(receiverID,modFile.PackageFiles.Count);
            foreach(string packageFile in modFile.PackageFiles)
            {
                Stream fileStream = new FileStream(modPath + "\\" + packageFile, FileMode.Open);
                BinaryReader fileReader = new BinaryReader(fileStream);
                byte[] fileBytes = fileReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                int PackageSize = 50;
                int numPackages = (fileBytes.Length + PackageSize - 1) / PackageSize;
                SendFile(receiverID, PackageSize, fileBytes,packageFile,FileTypes.ModFile);
                fileStream.Close();
            }
        }
        public void SendModInfo(int receiverID,int numFiles)
        {
            List<byte> message = new List<byte>(); 
            message.AddRange(BitConverter.GetBytes((int)ServerMessages.ModInfo));
            message.AddRange(BitConverter.GetBytes(numFiles));
            playerList[receiverID].clientSocket.SendMessage(message.ToArray());
        }
        public void SendWorldInfo(int receiverID, int numFiles)
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ServerMessages.WorldInfo));
            message.AddRange(BitConverter.GetBytes(numFiles));
            playerList[receiverID].clientSocket.SendMessage(message.ToArray());
        }
        

        public void SendShortMessage(int receiverID, ServerMessages messageID)
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)messageID));
            playerList[receiverID].clientSocket.SendMessage(message.ToArray());
        }

        public void SendWorldFiles(int receiverID)
        {
            string[] worldFiles = Directory.GetFiles(WorldFilePath);
            SendWorldInfo(receiverID,worldFiles.Length);
            for (int i = 0; i < worldFiles.Length; i++)
            {
                FileInfo fi = new FileInfo(worldFiles[i]);
                Stream fileStream = new FileStream(worldFiles[i], FileMode.Open);
                BinaryReader fileReader = new BinaryReader(fileStream);
                byte[] fileBytes = fileReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                int PackageSize = 50;
                int numPackages = (fileBytes.Length + PackageSize - 1) / PackageSize;
                SendFile(receiverID, PackageSize, fileBytes, fi.Name, FileTypes.WorldFile);
                fileStream.Close();
            }
        }

        public void PingAllPlayers()
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ServerMessages.Ping));
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].clientSocket.SendMessage(message.ToArray());
            }
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].clientSocket.SendMessage(message.ToArray());
            }
        }
        
    }
}
