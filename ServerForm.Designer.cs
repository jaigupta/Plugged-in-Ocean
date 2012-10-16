namespace ChatServer
{
    partial class ServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.btnListen = new System.Windows.Forms.Button();
            this.lbIp = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(263, 10);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(88, 23);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Start Server";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // lbIp
            // 
            this.lbIp.AutoSize = true;
            this.lbIp.Location = new System.Drawing.Point(15, 17);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(61, 13);
            this.lbIp.TabIndex = 2;
            this.lbIp.Text = "IP Address:";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 39);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(344, 180);
            this.txtLog.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(190, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "PORT: 1986";
            // 
            // txtIp
            // 
            this.txtIp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtIp.FormattingEnabled = true;
            this.txtIp.Location = new System.Drawing.Point(72, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(112, 21);
            this.txtIp.TabIndex = 5;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 231);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.btnListen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.Text = "Server Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label lbIp;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox txtIp;
    }
}

