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

namespace Sims3Server
{
    public partial class Sims3Server
    {
        TcpListener listener;
        List<Player> clientList;
        Thread serverThread;
        public NewMessageHandler NewMessageEvent = null;
        public string WorldFilePath = "";
        public string WorldFileName = "";
        public ModFile modFile;

        public Sims3Server(int port)
        {
            clientList = new List<Player>();
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
            for (int i = 0; i < clientList.Count; i++)
            {
                clientList[i].clientSocket.StopClient();
                
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
            int playerId = clientList.Count;
            clientList.Add(new Player(new TcpSocket(listener.AcceptTcpClient(), playerId)));
            clientList[playerId].clientSocket.NewMessageEvent += new NewMessageHandler(NewMessage);
            clientList[playerId].clientSocket.DisconnectClientEvent += new DisconnectClientHandler(RemoveClient);
            clientList[playerId].clientSocket.StartClient();
            SendPlayerData();
            SendServerConnectedMessage(playerId);
        }

        private void NewMessage(byte[] message, int clientID)
        {
            NewMessageEvent(message, clientID);
            DecodeMessage(message, clientID);
        }

        private void RemoveClient(int clientID)
        {
            clientList[clientID].clientSocket.StopClient();
            clientList.RemoveAt(clientID);
            for (int i = 0; i < clientList.Count; i++)
            {
                clientList[i].clientSocket.SetClientID(i);
            }
            SendPlayerData();
        }
    }
}
