namespace ChatClient
{
    partial class PrivateChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrivateChat));
            this.label1 = new System.Windows.Forms.Label();
            this.addLabel = new System.Windows.Forms.Label();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seismicCubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seismicInterpretationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tryclassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recieveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "To:";
            // 
            // addLabel
            // 
            this.addLabel.AutoSize = true;
            this.addLabel.ForeColor = System.Drawing.Color.Blue;
            this.addLabel.Location = new System.Drawing.Point(41, 33);
            this.addLabel.Name = "addLabel";
            this.addLabel.Size = new System.Drawing.Size(35, 13);
            this.addLabel.TabIndex = 1;
            this.addLabel.Text = "label2";
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(12, 404);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(236, 20);
            this.messageBox.TabIndex = 3;
            this.messageBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.messageBox_KeyPress);
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(255, 401);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(47, 23);
            this.sendBtn.TabIndex = 4;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToolStripMenuItem,
            this.controlsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(314, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendFileToolStripMenuItem,
            this.sendObjectToolStripMenuItem,
            this.recieveToolStripMenuItem});
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.sendToolStripMenuItem.Text = "Send";
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sendFileToolStripMenuItem.Text = "Send File";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem_Click);
            // 
            // sendObjectToolStripMenuItem
            // 
            this.sendObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seismicCubeToolStripMenuItem,
            this.seismicInterpretationToolStripMenuItem,
            this.tryclassToolStripMenuItem});
            this.sendObjectToolStripMenuItem.Name = "sendObjectToolStripMenuItem";
            this.sendObjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sendObjectToolStripMenuItem.Text = "Send Object";
            // 
            // seismicCubeToolStripMenuItem
            // 
            this.seismicCubeToolStripMenuItem.Name = "seismicCubeToolStripMenuItem";
            this.seismicCubeToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.seismicCubeToolStripMenuItem.Text = "Seismic Cube";
            this.seismicCubeToolStripMenuItem.Click += new System.EventHandler(this.seismicCubeToolStripMenuItem_Click);
            // 
            // seismicInterpretationToolStripMenuItem
            // 
            this.seismicInterpretationToolStripMenuItem.Name = "seismicInterpretationToolStripMenuItem";
            this.seismicInterpretationToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.seismicInterpretationToolStripMenuItem.Text = "Seismic Interpretation";
            this.seismicInterpretationToolStripMenuItem.Visible = false;
            this.seismicInterpretationToolStripMenuItem.Click += new System.EventHandler(this.seismicInterpretationToolStripMenuItem_Click);
            // 
            // tryclassToolStripMenuItem
            // 
            this.tryclassToolStripMenuItem.Name = "tryclassToolStripMenuItem";
            this.tryclassToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.tryclassToolStripMenuItem.Text = "tryclass";
            this.tryclassToolStripMenuItem.Visible = false;
            this.tryclassToolStripMenuItem.Click += new System.EventHandler(this.tryclassToolStripMenuItem_Click);
            // 
            // recieveToolStripMenuItem
            // 
            this.recieveToolStripMenuItem.Name = "recieveToolStripMenuItem";
            this.recieveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recieveToolStripMenuItem.Text = "Recieve";
            this.recieveToolStripMenuItem.Visible = false;
            this.recieveToolStripMenuItem.Click += new System.EventHandler(this.recieveToolStripMenuItem_Click);
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem});
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.controlsToolStripMenuItem.Text = "Controls";
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 50);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(290, 348);
            this.logBox.TabIndex = 6;
            // 
            // PrivateChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 436);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.addLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PrivateChat";
            this.Text = "PrivateChat";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label addLabel;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seismicCubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seismicInterpretationToolStripMenuItem;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tryclassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recieveToolStripMenuItem;
    }
}