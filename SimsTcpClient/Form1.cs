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
using TcpConnection;
using System.IO;

namespace SimsTcpClient
{
    public partial class Form1 : Form
    {
        OnlineMod onlineMod = new OnlineMod();
        bool menuBarClicked = false;
        Point menuBarClickLocation;
        Point formOriginalLocation;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.AppStartupPath = Application.StartupPath;
            Globals.AppName = Application.ProductName;
            Globals.player.Name = textBoxName.Text;
            SettingsFile.Open();
            textBoxIPAdress.Text = SettingsFile.IPAdress;
            textBoxName.Text = SettingsFile.UserName;
            if (Environment.GetCommandLineArgs().Length > 0)
            {
                if (Environment.GetCommandLineArgs()[0] == "Login")
                {
                    onlineMod.Start(SettingsFile.UserName, SettingsFile.IPAdress, new EventHandler(OnPlayersChanged));
                    onlineMod.MessageEvent += new ModMessageEvent(OnMessage);
                    onlineMod.OnUpdateReady += new UpDateReadyEvent(OnUpdate);
                }
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
            SettingsFile.Save();
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
           Process.Start("Updater.exe");
           Application.Exit();
       }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            SettingsFile.UserName = textBoxName.Text;
        }

        private void textBoxIPAdress_TextChanged(object sender, EventArgs e)
        {
            SettingsFile.IPAdress = textBoxIPAdress.Text;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            formSettings.Show();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            menuBarClicked = true;
            menuBarClickLocation = System.Windows.Forms.Control.MousePosition;
            formOriginalLocation = this.Location;
        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (menuBarClicked == true && this.WindowState == FormWindowState.Normal)
            {
                Point dragLocation = System.Windows.Forms.Control.MousePosition;
                int xDifference = formOriginalLocation.X - menuBarClickLocation.X;
                int yDifference = formOriginalLocation.Y - menuBarClickLocation.Y;
                Point location = new Point(dragLocation.X + xDifference, dragLocation.Y + yDifference);
                if (location.Y > 0 && location.Y < Screen.PrimaryScreen.Bounds.Height - 5)
                {
                    this.Location = location;
                }
            }
        }

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            menuBarClicked = false;
        }

        private void menuStrip1_MouseLeave(object sender, EventArgs e)
        {
            menuBarClicked = false;
        }

        private void buttonReady_Click(object sender, EventArgs e)
        {
            if (onlineMod.canLaunch)
            {
                Globals.player.Name = textBoxName.Text;
                Globals.player.IsReady = !Globals.player.IsReady;
                onlineMod.UpdatePlayerData();
            }
        }
    }
}
