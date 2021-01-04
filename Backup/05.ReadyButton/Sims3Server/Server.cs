using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TcpConnection;
using TcpMessageCodes;

namespace Sims3Server
{
    public partial class Sims3Server
    {
        TcpListener listener;
        List<Player> playerList;
        Thread serverThread;
        public NewMessageHandler NewMessageEvent = null;
        public string WorldFilePath = "";
        public string WorldFileName = "";
        public ModFile modFile;

        public Sims3Server(int port)
        {
            playerList = new List<Player>();
            listener = new TcpListener(IPAddress.Any, port);
            serverThread = new Thread(LookForClients);
        }

        public void Start()
        {
            listener.Start();
            serverThread.Start();
        }

        public void Stop()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].clientSocket.StopClient();
                
            }
            serverThread.Abort();
            listener.Stop();
        }

        private void LookForClients()
        {
            while (true)
            {
                if (!listener.Pending())
                {
                    PingAllPlayers();
                    Thread.Sleep(1000);
                    continue;
                }
                ConnectClient();
            }
        }

        private void ConnectClient()
        {
            PingAllPlayers();
            int playerId = playerList.Count;
            playerList.Add(new Player(new TcpSocket(listener.AcceptTcpClient(), playerId)));
            playerList[playerId].clientSocket.NewMessageEvent += new NewMessageHandler(NewMessage);
            playerList[playerId].clientSocket.DisconnectClientEvent += new DisconnectClientHandler(RemoveClient);
            playerList[playerId].clientSocket.StartClient();
            SendPlayerData();
            SendShortMessage(playerId,ServerMessages.ServerConnected);
        }

        private void NewMessage(byte[] message, int clientID)
        {
            NewMessageEvent(message, clientID);
            DecodeMessage(message, clientID);
        }

        private void RemoveClient(int clientID)
        {
            playerList[clientID].clientSocket.StopClient();
            playerList.RemoveAt(clientID);
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].clientSocket.SetClientID(i);
            }
            SendPlayerData();
        }

        public bool CanLauch()
        {
            foreach(Player player in playerList)
            {
                if (!player.IsReady)
                    return false;
            }
            return true;
        }
    }
}
