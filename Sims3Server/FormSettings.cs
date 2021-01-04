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
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBoxModFile.Text = SettingsFile.ModPath;
            textBoxSimsFolder.Text = SettingsFile.Sims3Folder;
            textBoxUpdateFolder.Text = SettingsFile.UpdateFolder;
        }

        private void buttonSettingsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSettingsOk_Click(object sender, EventArgs e)
        {
            SettingsFile.Sims3Folder = textBoxSimsFolder.Text;
            SettingsFile.ModPath = textBoxModFile.Text;
            SettingsFile.UpdateFolder = textBoxUpdateFolder.Text;
            this.Close();
        }
    }
}
