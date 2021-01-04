namespace Sims3Server
{
    partial class SelectWorldDialog
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
            this.listBoxWorlds = new System.Windows.Forms.ListBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxWorlds
            // 
            this.listBoxWorlds.FormattingEnabled = true;
            this.listBoxWorlds.Location = new System.Drawing.Point(2, 3);
            this.listBoxWorlds.Name = "listBoxWorlds";
            this.listBoxWorlds.Size = new System.Drawing.Size(255, 264);
            this.listBoxWorlds.TabIndex = 0;
            this.listBoxWorlds.SelectedIndexChanged += new System.EventHandler(this.listBoxWorlds_SelectedIndexChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(182, 273);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // SelectWorldDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 301);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.listBoxWorlds);
            this.Name = "SelectWorldDialog";
            this.Text = "SelectWorldDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxWorlds;
        private System.Windows.Forms.Button buttonOk;
    }
}