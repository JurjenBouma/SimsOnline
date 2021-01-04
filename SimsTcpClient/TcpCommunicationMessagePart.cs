using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpMessageCodes;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace SimsTcpClient
{
    public partial class TcpCommunication
    {
        private void DecodeMessage(byte[] message)
        {
            MemoryStream messageStream = new MemoryStream(message);
            BinaryReader reader = new BinaryReader(messageStream);

            int messageID = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
            if (messageID == (int)ServerMessages.PlayerData)
            {
                int playerCount = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                players.Clear();
                for (int i = 0; i < playerCount; i++)
                {
                    int nameLenght = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                    string name = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(nameLenght));
                    bool ready = BitConverter.ToBoolean(reader.ReadBytes(sizeof(bool)), 0);
                    int id = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);

                    players.Add(new Player(name, ready, id));
                }
                OnPlayersChanged(this, null);
            } 
            else if (messageID == (int)ServerMessages.WorldInfo)
            {
                int numFiles = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                OnWorldInfoReceived(numFiles);
            }
            else if (messageID == (int)ServerMessages.ModInfo)
            {
                int numFiles = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                OnModInfoReceived(numFiles);
            }
            else if (messageID == (int)ServerMessages.UpdateInfo)
            {
                int numDlls = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                OnUpdateInfoReceived(numDlls);
            }
            else if (messageID == (int)ServerMessages.ServerConnected)
            {
                OnServerConnected();
            }
            else if (messageID == (int)ServerMessages.LaunchGame)
            {
                OnLaunchGame();
            }
            else if (messageID == (int)ServerMessages.StartGamePlay)
            {
                OnStartGamePlay();
            }
            else if (messageID == (int)ServerMessages.GameMessage)
            {
                OnGameMessage(reader.ReadBytes(message.Length - sizeof(int)));
            }
            else if (messageID == (int)ServerMessages.FileHeader)
            {
                FileTypes type = (FileTypes)BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int nameLenght = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                string name = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(nameLenght));
                int numPackages = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                double fileID = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)), 0);
                OnFileHeaderReceived(numPackages,name,type,fileID);
            }
            else if (messageID == (int)ServerMessages.FileData)
            {
                double fileID = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)), 0);
                int packageIndex = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int packageSize = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                byte[] data = reader.ReadBytes(packageSize);
                OnFileDataReceived(packageIndex, data,fileID);
            }
        }

        public void SendGameMessages(byte[] messages)
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ClientMessages.GameMessage));
            message.AddRange(messages);
            m_tcpSocket.SendMessage(message.ToArray());
        }

        public void SendPlayerInfo()
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ClientMessages.PlayerInfo));
            message.AddRange(BitConverter.GetBytes(Globals.player.Name.Length));
            message.AddRange(ASCIIEncoding.ASCII.GetBytes(Globals.player.Name));
            message.AddRange(BitConverter.GetBytes(Globals.player.IsReady));
            m_tcpSocket.SendMessage(message.ToArray());
        }

        public void SendExeInfo(string exePath)
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ClientMessages.ExeInfo));
            FileVersionInfo fVI = FileVersionInfo.GetVersionInfo(exePath);
            message.AddRange(BitConverter.GetBytes(fVI.ProductMajorPart));
            message.AddRange(BitConverter.GetBytes(fVI.ProductMinorPart));
            message.AddRange(BitConverter.GetBytes(fVI.ProductBuildPart));
            message.AddRange(BitConverter.GetBytes(fVI.ProductPrivatePart));
            m_tcpSocket.SendMessage(message.ToArray());
        }
        
        public void SendGameLoaded()
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ClientMessages.GameLoaded));
            m_tcpSocket.SendMessage(message.ToArray());
        }
    }
}
