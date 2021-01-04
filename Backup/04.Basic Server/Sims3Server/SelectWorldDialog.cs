using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sims3Server
{
    public partial class SelectWorldDialog : Form
    {
        public string selectedWorldName = "";
        public SelectWorldDialog(string sims3WorldFolder)
        {
            InitializeComponent();
            InitializeListBox(sims3WorldFolder);
        }

        private void InitializeListBox(string sims3WorldFolder)
        {
            string[] worldFolders = Directory.GetDirectories(sims3WorldFolder);
            DirectoryInfo di;
            foreach(string worldFolder in worldFolders)
            {
                if (worldFolder.EndsWith(".sims3"))
                {
                    di = new DirectoryInfo(worldFolder);
                    listBoxWorlds.Items.Add(di.Name);
                }
            }
            listBoxWorlds.SelectedIndex = 0;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void listBoxWorlds_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedWorldName = listBoxWorlds.SelectedItem.ToString();
        }
    }
}
