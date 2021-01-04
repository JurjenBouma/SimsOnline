namespace Sims3Server
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
            this.textBoxModFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSettingsOk = new System.Windows.Forms.Button();
            this.buttonSettingsCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sims Documents Folder :";
            // 
            // textBoxSimsFolder
            // 
            this.textBoxSimsFolder.Location = new System.Drawing.Point(12, 25);
            this.textBoxSimsFolder.Name = "textBoxSimsFolder";
            this.textBoxSimsFolder.Size = new System.Drawing.Size(465, 20);
            this.textBoxSimsFolder.TabIndex = 1;
            // 
            // textBoxModFile
            // 
            this.textBoxModFile.Location = new System.Drawing.Point(12, 65);
            this.textBoxModFile.Name = "textBoxModFile";
            this.textBoxModFile.Size = new System.Drawing.Size(465, 20);
            this.textBoxModFile.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ModFile :";
            // 
            // buttonSettingsOk
            // 
            this.buttonSettingsOk.Location = new System.Drawing.Point(402, 134);
            this.buttonSettingsOk.Name = "buttonSettingsOk";
            this.buttonSettingsOk.Size = new System.Drawing.Size(75, 23);
            this.buttonSettingsOk.TabIndex = 4;
            this.buttonSettingsOk.Text = "Apply";
            this.buttonSettingsOk.UseVisualStyleBackColor = true;
            this.buttonSettingsOk.Click += new System.EventHandler(this.buttonSettingsOk_Click);
            // 
            // buttonSettingsCancel
            // 
            this.buttonSettingsCancel.Location = new System.Drawing.Point(321, 134);
            this.buttonSettingsCancel.Name = "buttonSettingsCancel";
            this.buttonSettingsCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonSettingsCancel.TabIndex = 5;
            this.buttonSettingsCancel.Text = "Cancel";
            this.buttonSettingsCancel.UseVisualStyleBackColor = true;
            this.buttonSettingsCancel.Click += new System.EventHandler(this.buttonSettingsCancel_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 169);
            this.Controls.Add(this.buttonSettingsCancel);
            this.Controls.Add(this.buttonSettingsOk);
            this.Controls.Add(this.textBoxModFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSimsFolder);
            this.Controls.Add(this.label1);
            this.Name = "FormSettings";
            this.Text = "FormSettings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSimsFolder;
        private System.Windows.Forms.TextBox textBoxModFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSettingsOk;
        private System.Windows.Forms.Button buttonSettingsCancel;
    }
}