using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MessageCodes;
using TcpMessageCodes;
using TcpConnection;
using System.Threading.Tasks;


namespace SimsTcpClient
{
    public delegate void ServerConnectedEvent();
    public delegate void LaunchGameEvent();
    public delegate void FileHeaderEvent(int numPackages,string name , FileTypes type, double id);
    public delegate void FileDataEvent(int packageIndex,byte[] packageData,double id);
    public delegate void WorldInfoEvent(int numFiles);
    public delegate void ModInfoEvent(int numFiles);

    public partial class TcpCommunication
    {
        public EventHandler OnPlayersChanged = null;
        public ServerConnectedEvent OnServerConnected = null;
        public FileHeaderEvent OnFileHeaderReceived = null;
        public FileDataEvent OnFileDataReceived = null;
        public ModInfoEvent OnModInfoReceived = null;
        public WorldInfoEvent OnWorldInfoReceived = null;
        public LaunchGameEvent OnLaunchGame = null;
        //////////////////////////////////Network
        TcpSocket m_tcpSocket;
        Thread serverConnectThread;
        bool m_connectedToServer = false;
        string m_ipAddress;
        int m_networkPort;
        //////////////////////////////////Network

        ///////////////////////////Players
        public List<Player> players = new List<Player>();
        ///////////////////////////Players

        public TcpCommunication()
        {
            
        }

        void NewServerMessage(byte[] message, int clientID)
        {
            DecodeMessage(message);
        }

        public void ConnectToServer(string ip,int port)
        {
            m_ipAddress = ip;
            m_networkPort = port;
            serverConnectThread = new Thread(TryConnectToServer);
            serverConnectThread.Start();
        }

        void TryConnectToServer()
        {
            while (!m_connectedToServer)
            {
                try
                {
                    TcpClient tempClientTcp = new TcpClient();
                    tempClientTcp.Connect(m_ipAddress, m_networkPort);
                    m_tcpSocket = new TcpSocket(tempClientTcp, 0);
                    m_tcpSocket.NewMessageEvent += new NewMessageHandler(NewServerMessage);
                    m_tcpSocket.DisconnectClientEvent += new DisconnectClientHandler(DisconnectFromServer);
                    m_tcpSocket.StartClient();
                    m_connectedToServer = true;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }

        void DisconnectFromServer(int iD)
        {
            if (m_tcpSocket != null)
            {
                m_tcpSocket.StopClient();
                m_tcpSocket = null;
                m_connectedToServer = false;
                TryConnectToServer();
            }
        }

        public void Stop()
        {
            serverConnectThread.Abort();
            if (m_tcpSocket != null)
            {
                m_tcpSocket.StopClient();
            }
           
        }
    }
}
