using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimsTcpClient
{
    public partial class FormSettings : Form
    {
        bool menuBarClicked = false;
        Point menuBarClickLocation;
        Point formOriginalLocation;

        public FormSettings()
        {
            InitializeComponent();
            textBoxSimsFolder.Text = SettingsFile.Sims3Folder;
        }

        private void buttonSettingsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSettingsOk_Click(object sender, EventArgs e)
        {
            SettingsFile.Sims3Folder = textBoxSimsFolder.Text;
            this.Close();
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

        private void menuStrip1_MouseLeave(object sender, EventArgs e)
        {
            menuBarClicked = false;
        }

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            menuBarClicked = false;
        }
    }
}
