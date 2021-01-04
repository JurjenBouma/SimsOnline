using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MessageCodes;

namespace SimsTcpClient
{
    public class BufferSocket
    {
        public delegate void GameLoadedEventHandler();
        public delegate void GameMessage(byte[] message);

        public GameLoadedEventHandler OnGameLoaded = null;
        public GameMessage OnGameMessage = null;

        const int longSize = sizeof(ulong);
        const int intSize = sizeof(uint);
        public const int BufferSize = 1024;
        public bool isConnected = false;
        public List<byte[]> sendBuffer = new List<byte[]>();
        public Thread BufferThread;

        public BufferSocket()
        {
            MessageBuffer.Initialize(BufferSize);
            BufferThread = new Thread(RunThread);
        }

        public void Start()
        {
            BufferThread.Start();
        }
        public void Stop()
        {
            BufferThread.Abort();
        }

        private void RunThread()
        {
            Connect();
            ProcessMessages();
        }

        private void Connect()
        {
            while (!IsProcessRunning("TS3W"))
            {

            }
            Thread.Sleep(10000);
            Process process = Process.GetProcessesByName("TS3W")[0];
            while (!MessageBuffer.Connect(process.Id, (ulong)SenderIDs.BufferFindID))
            {
                System.Threading.Thread.Sleep(100);
            }
            RequestHandShake();
            SendMessages();
        }
       
        private void RequestHandShake()
        {
            byte[] message = System.BitConverter.GetBytes((uint)MessageIDs.HandShakeRequest);
            sendBuffer.Add(message);
        }

        public bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == processName)
                    return true;
            }
            return false;
        }

        public void SendGameMessages(byte[] message)
        {
            List<byte> messages = new List<byte>();
            
            byte[] senderBytes = BitConverter.GetBytes((ulong)SenderIDs.TcpController);
            messages.AddRange(senderBytes);
            messages.AddRange(message);
            MessageBuffer.WriteMessage(messages.ToArray());
        }

        private void ProcessMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[BufferSize];
                MessageBuffer.ReadMessage(buffer, (ulong)0, true);
                ulong sender = System.BitConverter.ToUInt64(buffer, 0);
                if (sender == (ulong)SenderIDs.TcpController)
                {
                    Thread.Sleep(10);
                    continue;
                }
                ReadAllMessages(buffer);
                SendMessages();
            }
        }
        private void ReadAllMessages(byte[] buffer)
        {
            int numMessages = (int)buffer[longSize]; 
            int messageIndex = longSize + 1;
            for (int i = 0; i < numMessages; i++)
            {
                messageIndex = ProcessMessage(messageIndex, buffer);
                if (messageIndex == -1)
                    return;
            }
        }
        private int ProcessMessage(int messageIndex,byte[] buffer)
        {
            int processPosition = messageIndex;
            uint message = BitConverter.ToUInt32(buffer, messageIndex);
            processPosition += intSize;
            List<byte> gameMassage = new List<byte>(buffer);

            if (message == (uint)MessageIDs.GameLoaded)
            {
                OnGameLoaded();
                return processPosition;
            }
            else if(message == (uint)MessageIDs.HandShakeComfirm)
            {
                isConnected = true;
                return processPosition;
            }
            else
            {
                OnGameMessage(gameMassage.GetRange(messageIndex-1,buffer.Length - messageIndex-1).ToArray());
                return -1;
            }
        }

        private void SendMessages()
        {
            byte[] messages = new byte[BufferSize];
            int messageIndex = longSize + 1;
            for (int i = 0; i < sendBuffer.Count; i++)
            {
                for (int b = 0; b < sendBuffer[i].Length; b++)
                {
                    messages[messageIndex + b] = sendBuffer[i][b];
                }
                messageIndex += sendBuffer[i].Length;
            }
            messages[longSize] = (byte)sendBuffer.Count;
            byte[] senderBytes = BitConverter.GetBytes((ulong)SenderIDs.TcpController);
            for (int i = 0; i < senderBytes.Length; i++)
            {
                messages[i] = senderBytes[i];
            }
            sendBuffer.Clear();
            MessageBuffer.WriteMessage(messages);
        }

    }
}
