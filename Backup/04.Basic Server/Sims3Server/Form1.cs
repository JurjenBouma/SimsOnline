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
        string simsWorldFolder = "D:\\Users\\Jurjen\\Documents\\Electronic Arts\\De Sims 3\\Saves";
        string modFilePath = "";
        public FormMain()
        {
            InitializeComponent();
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
            if(server != null)
                server.Stop();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            if (modFilePath.Length > 0)
            {
                worldSelectDialog = new SelectWorldDialog(simsWorldFolder);
                if (worldSelectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    server = new Sims3Server(34019);
                    server.modFile = new ModFile(modFilePath, Application.StartupPath);
                    server.WorldFilePath = simsWorldFolder + "\\" + worldSelectDialog.selectedWorldName;
                    server.WorldFileName = worldSelectDialog.selectedWorldName;
                    server.NewMessageEvent += new TcpConnection.NewMessageHandler(NewMessage);
                    server.Start();
                }
            }
        }

        private void selectModFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogSelectModFile.ShowDialog();
        }

        private void openFileDialogSelectModFile_FileOk(object sender, CancelEventArgs e)
        {
            modFilePath = openFileDialogSelectModFile.FileName;
        }
    }
}
