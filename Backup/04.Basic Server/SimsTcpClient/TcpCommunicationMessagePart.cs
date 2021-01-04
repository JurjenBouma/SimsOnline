using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (messageID == (int)ServerMessages.WorldInfo)
            {
                int numFiles = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                OnWorldInfoReceived(numFiles);
            }
            if (messageID == (int)ServerMessages.ModInfo)
            {
                int numFiles = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                OnModInfoReceived(numFiles);
            }
            if (messageID == (int)ServerMessages.ServerConnected)
            {
                OnServerConnected();
                SendPlayerInfo(m_nickName);
            }
            if (messageID == (int)ServerMessages.FileHeader)
            {
                FileTypes type = (FileTypes)BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int nameLenght = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                string name = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(nameLenght));
                int numPackages = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                double fileID = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)), 0);
                OnFileHeaderReceived(numPackages,name,type,fileID);
            }
            if (messageID == (int)ServerMessages.FileData)
            {
                double fileID = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)), 0);
                int packageIndex = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                int packageSize = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
                byte[] data = reader.ReadBytes(packageSize);
                OnFileDataReceived(packageIndex, data,fileID);
            }
        }


        public void SendPlayerInfo(string PlayerName)
        {
            List<byte> message = new List<byte>();
            message.AddRange(BitConverter.GetBytes((int)ClientMessages.PlayerInfo));
            message.AddRange(BitConverter.GetBytes(PlayerName.Length));
            message.AddRange(ASCIIEncoding.ASCII.GetBytes(PlayerName));
            message.AddRange(BitConverter.GetBytes(false));//notReady
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
      
    }
}
