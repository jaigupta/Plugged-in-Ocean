namespace Plot3D
{
    partial class Plot3DMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plot3DMainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bw = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.zNum = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.yNum = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.xNum = new System.Windows.Forms.NumericUpDown();
            this.zoomOutBtn = new System.Windows.Forms.Button();
            this.zoomInBtn = new System.Windows.Forms.Button();
            this.maxNum = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.minNum = new System.Windows.Forms.NumericUpDown();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cubename = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.recalBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.bw);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.zNum);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.yNum);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.xNum);
            this.panel1.Controls.Add(this.zoomOutBtn);
            this.panel1.Controls.Add(this.zoomInBtn);
            this.panel1.Controls.Add(this.maxNum);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.minNum);
            this.panel1.Controls.Add(this.trackBar2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.ForeColor = System.Drawing.Color.Snow;
            this.panel1.Location = new System.Drawing.Point(0, 518);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1034, 29);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.Location = new System.Drawing.Point(108, -1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(122, 27);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(108, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(122, 26);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // bw
            // 
            this.bw.AutoSize = true;
            this.bw.Location = new System.Drawing.Point(793, 6);
            this.bw.Name = "bw";
            this.bw.Size = new System.Drawing.Size(73, 17);
            this.bw.TabIndex = 18;
            this.bw.Text = "Grayscale";
            this.bw.UseVisualStyleBackColor = true;
            this.bw.CheckedChanged += new System.EventHandler(this.bw_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Snow;
            this.label7.Location = new System.Drawing.Point(711, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Z:";
            // 
            // zNum
            // 
            this.zNum.Location = new System.Drawing.Point(727, 4);
            this.zNum.Name = "zNum";
            this.zNum.Size = new System.Drawing.Size(56, 20);
            this.zNum.TabIndex = 16;
            this.zNum.ValueChanged += new System.EventHandler(this.zNum_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Snow;
            this.label6.Location = new System.Drawing.Point(628, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Y:";
            // 
            // yNum
            // 
            this.yNum.Location = new System.Drawing.Point(644, 3);
            this.yNum.Name = "yNum";
            this.yNum.Size = new System.Drawing.Size(61, 20);
            this.yNum.TabIndex = 14;
            this.yNum.ValueChanged += new System.EventHandler(this.yNum_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Snow;
            this.label5.Location = new System.Drawing.Point(540, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "X:";
            // 
            // xNum
            // 
            this.xNum.Location = new System.Drawing.Point(560, 3);
            this.xNum.Name = "xNum";
            this.xNum.Size = new System.Drawing.Size(62, 20);
            this.xNum.TabIndex = 12;
            this.xNum.ValueChanged += new System.EventHandler(this.xNum_ValueChanged);
            // 
            // zoomOutBtn
            // 
            this.zoomOutBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomOutBtn.ForeColor = System.Drawing.Color.Black;
            this.zoomOutBtn.Location = new System.Drawing.Point(511, 0);
            this.zoomOutBtn.Name = "zoomOutBtn";
            this.zoomOutBtn.Size = new System.Drawing.Size(22, 24);
            this.zoomOutBtn.TabIndex = 11;
            this.zoomOutBtn.Text = "-";
            this.zoomOutBtn.UseVisualStyleBackColor = true;
            this.zoomOutBtn.Click += new System.EventHandler(this.zoomOutBtn_Click);
            // 
            // zoomInBtn
            // 
            this.zoomInBtn.ForeColor = System.Drawing.Color.Black;
            this.zoomInBtn.Location = new System.Drawing.Point(484, 0);
            this.zoomInBtn.Name = "zoomInBtn";
            this.zoomInBtn.Size = new System.Drawing.Size(23, 24);
            this.zoomInBtn.TabIndex = 10;
            this.zoomInBtn.Text = "+";
            this.zoomInBtn.UseVisualStyleBackColor = true;
            this.zoomInBtn.Click += new System.EventHandler(this.zoomInBtn_Click);
            // 
            // maxNum
            // 
            this.maxNum.DecimalPlaces = 5;
            this.maxNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.maxNum.Location = new System.Drawing.Point(267, 4);
            this.maxNum.Name = "maxNum";
            this.maxNum.Size = new System.Drawing.Size(65, 20);
            this.maxNum.TabIndex = 8;
            this.maxNum.ValueChanged += new System.EventHandler(this.maxNum_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Min:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(872, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Quality:";
            // 
            // minNum
            // 
            this.minNum.DecimalPlaces = 5;
            this.minNum.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.minNum.Location = new System.Drawing.Point(33, 3);
            this.minNum.Name = "minNum";
            this.minNum.Size = new System.Drawing.Size(67, 20);
            this.minNum.TabIndex = 4;
            this.minNum.ValueChanged += new System.EventHandler(this.minNum_ValueChanged);
            // 
            // trackBar2
            // 
            this.trackBar2.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar2.Location = new System.Drawing.Point(914, 0);
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(120, 29);
            this.trackBar2.TabIndex = 3;
            this.trackBar2.Value = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cubename);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.recalBtn);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(1034, 77);
            this.panel2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(637, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Inbuilt Library";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Default",
            "Sine",
            "Cosine ",
            "Tangent",
            "Cot",
            "Sine inverse",
            "Cosine inverse",
            "Tan inverse",
            "Logarithmic (base 10)",
            "Logarithmic (base 2)",
            "Logarithmic",
            "Round Off",
            "Round Up",
            "Round Down",
            "Squared",
            "Cubic",
            "Inverse"});
            this.comboBox1.Location = new System.Drawing.Point(727, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(780, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "New Cube name";
            // 
            // cubename
            // 
            this.cubename.Location = new System.Drawing.Point(728, 39);
            this.cubename.Name = "cubename";
            this.cubename.Size = new System.Drawing.Size(140, 20);
            this.cubename.TabIndex = 7;
            this.cubename.Text = "Cube1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(873, 37);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save Cube";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(990, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // recalBtn
            // 
            this.recalBtn.Location = new System.Drawing.Point(631, 38);
            this.recalBtn.Name = "recalBtn";
            this.recalBtn.Size = new System.Drawing.Size(91, 23);
            this.recalBtn.TabIndex = 4;
            this.recalBtn.Text = "Recalculate";
            this.recalBtn.UseVisualStyleBackColor = true;
            this.recalBtn.Click += new System.EventHandler(this.recalBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox1.Location = new System.Drawing.Point(10, 10);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(612, 57);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "val";
            // 
            // Plot3DMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1034, 547);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "Plot3DMainForm";
            this.Text = "Plot 3D surface";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Click += new System.EventHandler(this.Plot3DMainForm_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Plot3DMainForm_MouseDown_1);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Plot3DMainForm_MouseMove);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown minNum;
        private System.Windows.Forms.TrackBar trackBar2;
        public System.Windows.Forms.NumericUpDown maxNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button zoomOutBtn;
        private System.Windows.Forms.Button zoomInBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown zNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown yNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown xNum;
        private System.Windows.Forms.Button recalBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox bw;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox cubename;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;

    }
}

