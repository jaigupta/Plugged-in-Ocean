namespace GeoCommunication
{
    partial class objSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(objSelect));
            this.sendBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.selectCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // sendBtn
            // 
            this.sendBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.sendBtn.Location = new System.Drawing.Point(199, 67);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(73, 23);
            this.sendBtn.TabIndex = 7;
            this.sendBtn.Text = "Select";
            this.sendBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(282, 67);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(73, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // selectCombo
            // 
            this.selectCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectCombo.FormattingEnabled = true;
            this.selectCombo.Location = new System.Drawing.Point(12, 24);
            this.selectCombo.Name = "selectCombo";
            this.selectCombo.Size = new System.Drawing.Size(343, 21);
            this.selectCombo.TabIndex = 9;
            this.selectCombo.SelectedIndexChanged += new System.EventHandler(this.selectCombo_SelectedIndexChanged);
            // 
            // objSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 109);
            this.Controls.Add(this.selectCombo);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.sendBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "objSelect";
            this.Text = "Slect an object to send";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ComboBox selectCombo;
    }
}