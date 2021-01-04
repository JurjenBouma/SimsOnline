using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TcpConnection
{
    public delegate void NewMessageHandler(byte[] message, int clientID);
    public delegate void DisconnectClientHandler(int clientID);

    public class TcpSocket
    {
        TcpClient m_clientSocket;
        int m_clientID;
        Thread m_clientInputThread;
        byte[] m_recivedData;
        bool m_connected;

        public NewMessageHandler NewMessageEvent = null;
        public DisconnectClientHandler DisconnectClientEvent = null;

        public TcpSocket(TcpClient clientSocket, int clientID)
        {
            m_clientSocket = clientSocket;
            m_clientInputThread = new Thread(HandleClientInput);
            m_clientID = clientID;
            m_recivedData = new byte[2048];
            m_connected = true;
        }
        public int GetClientID() { return m_clientID; }
        public void SetClientID(int iD) { m_clientID = iD; }

        public void StartClient()
        {
            m_clientInputThread.Start();
        }

        public void StopClient()
        {
            m_clientInputThread.Abort();
            m_clientSocket.Close();
        }

        public void SendMessage(byte[] message)
        {
            //WriteMessages
            if (m_connected)
            {
                try
                {
                    NetworkStream nStream = m_clientSocket.GetStream();
                    byte[] messageLenght = BitConverter.GetBytes(message.Length);
                    List<byte> fullMessage = new List<byte>();
                    fullMessage.AddRange(messageLenght);
                    fullMessage.AddRange(message);
                    nStream.Write(fullMessage.ToArray(), 0, fullMessage.Count);
                    nStream.Flush();
                }
                catch
                {
                    m_connected = false;
                    DisconnectClientEvent(m_clientID);
                }
            }
        }

        void ProcessMessage(int messageLenght)
        {
            if (messageLenght > 0)
            {
                byte[] message = new byte[messageLenght];
                for (int i = 0; i < messageLenght; i++)
                {
                    message[i] = m_recivedData[sizeof(int) + i];
                }
                NewMessageEvent(message, m_clientID);
            }
        }

        void HandleClientInput()
        {
            while (m_connected)
            {
                try
                {
                    if (m_clientSocket.GetStream().DataAvailable)
                    {
                        //ReadMessages
                        NetworkStream nStream = m_clientSocket.GetStream();

                        nStream.Read(m_recivedData, 0, sizeof(int));
                        int messageLenght = BitConverter.ToInt32(m_recivedData, 0);
                        int nRead = nStream.Read(m_recivedData, sizeof(int), messageLenght);
                        while (nRead != messageLenght)
                        {
                            nRead += nStream.Read(m_recivedData, sizeof(int) + nRead, messageLenght - nRead);
                        }
                        ProcessMessage(messageLenght);
                        nStream.Flush();
                        m_recivedData = new byte[2048];
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch
                {

                }
            }
        }
    }
}


