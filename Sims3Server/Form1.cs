using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sims3Server
{
    public partial class FormMain : Form
    {
        Sims3Server server;
        SelectWorldDialog worldSelectDialog;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SettingsFile.Open();
        }

        private void NewMessage(byte[] message,int clientId)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new TcpConnection.NewMessageHandler(NewMessage), message, clientId);
                return;
            }
            richTextBox1.Text += "Send by: " + clientId.ToString() + " - ";
            richTextBox1.Text += Encoding.ASCII.GetString(message);
            richTextBox1.Text += "\n";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsFile.Save();
            if(server != null)
                server.Stop();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            if (SettingsFile.ModPath.Length > 0 && server == null)
            {
                worldSelectDialog = new SelectWorldDialog(SettingsFile.Sims3Folder + "\\Saves");
                if (worldSelectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    server = new Sims3Server(34019);
                    server.modFile = new ModFile(SettingsFile.ModPath, Application.StartupPath);
                    server.WorldFilePath = SettingsFile.Sims3Folder +"\\Saves\\" + worldSelectDialog.selectedWorldName;
                    server.WorldFileName = worldSelectDialog.selectedWorldName;
                    server.NewMessageEvent += new TcpConnection.NewMessageHandler(NewMessage);
                    server.Start();
                }
            }
        }

        private void selectModFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            formSettings.Show();
        }
    }
}
