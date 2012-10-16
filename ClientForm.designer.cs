namespace ChatClient
{
    partial class ClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.onlineUsers = new System.Windows.Forms.ListBox();
            this.transBar = new System.Windows.Forms.ProgressBar();
            this.transText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(353, 35);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(82, 11);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(128, 20);
            this.txtIp.TabIndex = 1;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(13, 14);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(54, 13);
            this.lblServer.TabIndex = 2;
            this.lblServer.Text = "Server IP:";
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(353, 354);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Enabled = false;
            this.txtMessage.Location = new System.Drawing.Point(16, 353);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(331, 20);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(153, 63);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(275, 277);
            this.txtLog.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(13, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(63, 13);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "User Name:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(82, 37);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(265, 20);
            this.txtUser.TabIndex = 7;
            this.txtUser.Text = "Deepak Gupta";
            // 
            // onlineUsers
            // 
            this.onlineUsers.FormattingEnabled = true;
            this.onlineUsers.Items.AddRange(new object[] {
            "Deepak Gupta",
            "Jai Gupta"});
            this.onlineUsers.Location = new System.Drawing.Point(16, 63);
            this.onlineUsers.Name = "onlineUsers";
            this.onlineUsers.Size = new System.Drawing.Size(131, 277);
            this.onlineUsers.TabIndex = 8;
            this.onlineUsers.DoubleClick += new System.EventHandler(this.onlineUsers_DoubleClick);
            // 
            // transBar
            // 
            this.transBar.Location = new System.Drawing.Point(272, 11);
            this.transBar.Name = "transBar";
            this.transBar.Size = new System.Drawing.Size(156, 20);
            this.transBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.transBar.TabIndex = 9;
            this.transBar.Value = 35;
            this.transBar.Visible = false;
            // 
            // transText
            // 
            this.transText.AutoSize = true;
            this.transText.Location = new System.Drawing.Point(218, 14);
            this.transText.Name = "transText";
            this.transText.Size = new System.Drawing.Size(46, 13);
            this.transText.TabIndex = 10;
            this.transText.Text = "Sending";
            this.transText.Visible = false;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 389);
            this.Controls.Add(this.transText);
            this.Controls.Add(this.transBar);
            this.Controls.Add(this.onlineUsers);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "Chat Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.ListBox onlineUsers;
        private System.Windows.Forms.ProgressBar transBar;
        private System.Windows.Forms.Label transText;
    }
}

