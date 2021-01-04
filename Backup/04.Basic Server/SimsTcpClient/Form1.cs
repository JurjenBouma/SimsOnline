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
using TcpConnection;
using System.IO;

namespace SimsTcpClient
{
    public partial class Form1 : Form
    {
        OnlineMod onlineMod = new OnlineMod(Application.StartupPath, Application.ProductName);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(Application.ExecutablePath.EndsWith("Temp.exe"))
            {
                string destPath = Application.ExecutablePath.Substring(0,Application.ExecutablePath.Length - "Temp.exe".Length) + ".exe";
                File.Copy(Application.ExecutablePath, destPath,true);
                richTextBoxInfo.Text += "Program has been Updated.\nPlease Relog.";
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            onlineMod.Start(textBoxName.Text, textBoxIPAdress.Text, new EventHandler(OnPlayersChanged));
            onlineMod.MessageEvent += new ModMessageEvent(OnMessage);
            onlineMod.OnUpdateReady += new UpDateReadyEvent(OnUpdate);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            onlineMod.Stop();
        }

        private void OnMessage(string message)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new ModMessageEvent(OnMessage), message);
                return;
            }
            richTextBoxInfo.Text += message + "\n";
        }

       private void OnPlayersChanged(object obj,EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnPlayersChanged), obj, e);
                return;
            }
            listBoxPlayers.Items.Clear();
           foreach(Player player in onlineMod.tcpManager.players)
           {
               listBoxPlayers.Items.Add(player.ToString());
           }
        }
        private void OnUpdate()
       {
           if (InvokeRequired)
           {
               this.BeginInvoke(new UpDateReadyEvent(OnUpdate));
               return;
           }
           Application.Exit();
       }
    }
}
