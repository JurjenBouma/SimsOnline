using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MessageCodes;

namespace SimsTcpClient
{
    class BufferSocket
    {
        const int longSize = sizeof(ulong);
        public const int BufferSize = 1024;
        public bool isConnected = false;

        public BufferSocket()
        {
            MessageBuffer.Initialize(BufferSize);
        }

        public void Connect()
        {
            while (!IsProcessRunning("TS3W"))
            {

            }
            Thread.Sleep(10000);
            Process process = Process.GetProcessesByName("TS3W")[0];
            while (!MessageBuffer.Connect(process.Id, (ulong)MessageIDs.BufferFindID))
            {
                System.Threading.Thread.Sleep(100);
            }
            isConnected = true;

            RequestHandShake();
            WaitForHandshake();
            ///////////////////////////////////////////Temp start game message
            byte[] buffer = new byte[BufferSize];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 1;
            }
            byte[] senderID = System.BitConverter.GetBytes((ulong)SenderIDs.TcpController);
            byte[] messageID = System.BitConverter.GetBytes((ulong)MessageIDs.StartGame);
            for (int i = 0; i < longSize; i++)
            {
                buffer[i] = senderID[i];//SenderID
            }
            buffer[longSize] = 1;//Num messages
            for (int i = 0; i < longSize; i++)
            {
                buffer[longSize + i + 1] = messageID[i];//Message
            }
            MessageBuffer.WriteMessage(buffer);
            ///////////////////////////////////////////Temp start game message

        }
        public void SendMessages(byte[] messages)
        {
            MessageBuffer.WriteMessage(messages);
        }

        private void RequestHandShake()
        {
            byte[] buffer = new byte[BufferSize];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 1;
            }

            byte[] senderID = System.BitConverter.GetBytes((ulong)SenderIDs.TcpController);
            byte[] messageID = System.BitConverter.GetBytes((ulong)MessageIDs.HandShakeRequest);

            for (int i = 0; i < longSize; i++)
            {
                buffer[i] = senderID[i];//SenderID
            }
            buffer[longSize] = 1;//Num messages
            for (int i = 0; i < longSize; i++)
            {
                buffer[longSize + i + 1] = messageID[i];//Message
            }
            SendMessages(buffer);
        }

        private void WaitForHandshake()
        {
            byte[] buffer = new byte[BufferSize];
            ulong message = 0;
            while (message != (ulong)MessageIDs.HandShakeComfirm)
            {
                MessageBuffer.ReadMessage(buffer, (ulong)SenderIDs.Sims3, false);
                message = BitConverter.ToUInt64(buffer, longSize + 1);
            }
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
    }
}
