namespace SimsTcpClient
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSimsFolder = new System.Windows.Forms.TextBox();
            this.buttonSettingsOk = new System.Windows.Forms.Button();
            this.buttonSettingsCancel = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sims3 Documents Folder :";
            // 
            // textBoxSimsFolder
            // 
            this.textBoxSimsFolder.Location = new System.Drawing.Point(15, 51);
            this.textBoxSimsFolder.Name = "textBoxSimsFolder";
            this.textBoxSimsFolder.Size = new System.Drawing.Size(453, 20);
            this.textBoxSimsFolder.TabIndex = 1;
            // 
            // buttonSettingsOk
            // 
            this.buttonSettingsOk.BackgroundImage = global::SimsTcpClient.Properties.Resources.EmptyButton;
            this.buttonSettingsOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSettingsOk.FlatAppearance.BorderSize = 0;
            this.buttonSettingsOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettingsOk.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettingsOk.Location = new System.Drawing.Point(377, 370);
            this.buttonSettingsOk.Name = "buttonSettingsOk";
            this.buttonSettingsOk.Size = new System.Drawing.Size(91, 40);
            this.buttonSettingsOk.TabIndex = 2;
            this.buttonSettingsOk.Text = "Apply";
            this.buttonSettingsOk.UseVisualStyleBackColor = true;
            this.buttonSettingsOk.Click += new System.EventHandler(this.buttonSettingsOk_Click);
            // 
            // buttonSettingsCancel
            // 
            this.buttonSettingsCancel.BackgroundImage = global::SimsTcpClient.Properties.Resources.EmptyButton;
            this.buttonSettingsCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSettingsCancel.FlatAppearance.BorderSize = 0;
            this.buttonSettingsCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettingsCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettingsCancel.Location = new System.Drawing.Point(280, 370);
            this.buttonSettingsCancel.Name = "buttonSettingsCancel";
            this.buttonSettingsCancel.Size = new System.Drawing.Size(91, 40);
            this.buttonSettingsCancel.TabIndex = 3;
            this.buttonSettingsCancel.Text = "Cancel";
            this.buttonSettingsCancel.UseVisualStyleBackColor = true;
            this.buttonSettingsCancel.Click += new System.EventHandler(this.buttonSettingsCancel_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(492, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseDown);
            this.menuStrip1.MouseLeave += new System.EventHandler(this.menuStrip1_MouseLeave);
            this.menuStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseMove);
            this.menuStrip1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseUp);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SimsTcpClient.Properties.Resources.blue_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(492, 422);
            this.Controls.Add(this.buttonSettingsCancel);
            this.Controls.Add(this.buttonSettingsOk);
            this.Controls.Add(this.textBoxSimsFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormSettings";
            this.Text = "FormSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSimsFolder;
        private System.Windows.Forms.Button buttonSettingsOk;
        private System.Windows.Forms.Button buttonSettingsCancel;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}