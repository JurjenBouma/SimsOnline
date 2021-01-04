using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MessageCodes;

namespace SimsTcpClient
{
    public partial class Form1 : Form
    {
        int longSize = sizeof(ulong);

        public Form1()
        {
            InitializeComponent();
            MessageBuffer.Initialize(MessageBuffer.BufferSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while(!IsProcessRunning("TS3W"))
            {

            }
            System.Threading.Thread.Sleep(10000);
            Process process = Process.GetProcessesByName("TS3W")[0];
            while(!MessageBuffer.Connect(process.Id, (ulong)MessageIDs.BufferFindID))
            {
                System.Threading.Thread.Sleep(100);
            }
            byte[] buffer = new byte[MessageBuffer.BufferSize];
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
            MessageBuffer.WriteMessage(buffer);

            ulong message = 0;
            while(message != (ulong)MessageIDs.HandShakeComfirm)
            {
                MessageBuffer.ReadMessage(buffer, (ulong)SenderIDs.Sims3, false);
                message = BitConverter.ToUInt64(buffer, longSize+1);
            }

            messageID = System.BitConverter.GetBytes((ulong)MessageIDs.StartGame);
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

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[MessageBuffer.BufferSize];
            byte[] senderID = System.BitConverter.GetBytes((ulong)SenderIDs.TcpController);
            byte[] messageID = System.BitConverter.GetBytes((ulong)MessageIDs.GameFlowPause);

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
            richTextBox1.Text = MessageBuffer.GetAddress().ToString();
        }
    }
}
