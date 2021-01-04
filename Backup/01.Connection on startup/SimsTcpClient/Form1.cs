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
            MessageBuffer.isConnected = MessageBuffer.Connect(process.Id, (ulong)MessageIDs.BufferFindID);
            byte[] buffer = new byte[MessageBuffer.BufferSize];

            byte[] ID = System.BitConverter.GetBytes((ulong)MessageIDs.HandShakeRequest);
            int longSize = sizeof(ulong);
            for (int i = 0; i < longSize; i++)
            {
                buffer[i] = ID[i];
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

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
                byte[] buffer = new byte[MessageBuffer.BufferSize];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0;
                }
                ulong bufferID = (ulong)MessageIDs.BufferFindID;
                byte[] idBytes = System.BitConverter.GetBytes(bufferID);
                int longSize = sizeof(ulong);
                for (int i = 0; i < longSize; i++)
                {
                    buffer[i] = idBytes[longSize - i - 1];
                }
                string ms = "";
                for (int i = 0; i < buffer.Length; i++)
                {
                    ms += buffer[i].ToString() + ",";
                }
                System.Windows.Forms.Clipboard.SetText(ms);
        }
    }
}
