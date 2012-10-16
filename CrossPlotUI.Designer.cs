namespace CrossPlotter
{
    partial class CrossPlotUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.deltaZ = new System.Windows.Forms.NumericUpDown();
            this.centerZ = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.deltaY = new System.Windows.Forms.NumericUpDown();
            this.centerY = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.deltaX = new System.Windows.Forms.NumericUpDown();
            this.centerX = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerX)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.deltaZ);
            this.panel1.Controls.Add(this.centerZ);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.deltaY);
            this.panel1.Controls.Add(this.centerY);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.deltaX);
            this.panel1.Controls.Add(this.centerX);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 618);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1080, 84);
            this.panel1.TabIndex = 1;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(533, 51);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(31, 23);
            this.button4.TabIndex = 30;
            this.button4.Text = "?";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(376, 26);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(32, 20);
            this.numericUpDown1.TabIndex = 27;
            this.numericUpDown1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(369, 0);
            this.trackBar1.Maximum = 63;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(195, 45);
            this.trackBar1.TabIndex = 29;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(324, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Zoom :";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(414, 55);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(29, 23);
            this.button3.TabIndex = 26;
            this.button3.Text = "-";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(377, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(31, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(326, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Radius:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(335, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Color:";
            // 
            // deltaZ
            // 
            this.deltaZ.DecimalPlaces = 5;
            this.deltaZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.deltaZ.Location = new System.Drawing.Point(204, 55);
            this.deltaZ.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.deltaZ.Name = "deltaZ";
            this.deltaZ.Size = new System.Drawing.Size(108, 20);
            this.deltaZ.TabIndex = 22;
            // 
            // centerZ
            // 
            this.centerZ.DecimalPlaces = 5;
            this.centerZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.centerZ.Location = new System.Drawing.Point(48, 55);
            this.centerZ.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.centerZ.Minimum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            -2147483648});
            this.centerZ.Name = "centerZ";
            this.centerZ.Size = new System.Drawing.Size(111, 20);
            this.centerZ.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Delta";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Center";
            // 
            // deltaY
            // 
            this.deltaY.DecimalPlaces = 5;
            this.deltaY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.deltaY.Location = new System.Drawing.Point(204, 27);
            this.deltaY.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.deltaY.Name = "deltaY";
            this.deltaY.Size = new System.Drawing.Size(108, 20);
            this.deltaY.TabIndex = 18;
            // 
            // centerY
            // 
            this.centerY.DecimalPlaces = 5;
            this.centerY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.centerY.Location = new System.Drawing.Point(48, 27);
            this.centerY.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.centerY.Minimum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            -2147483648});
            this.centerY.Name = "centerY";
            this.centerY.Size = new System.Drawing.Size(111, 20);
            this.centerY.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(166, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Delta";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Center";
            // 
            // deltaX
            // 
            this.deltaX.DecimalPlaces = 5;
            this.deltaX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.deltaX.Location = new System.Drawing.Point(204, 0);
            this.deltaX.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.deltaX.Name = "deltaX";
            this.deltaX.Size = new System.Drawing.Size(108, 20);
            this.deltaX.TabIndex = 14;
            // 
            // centerX
            // 
            this.centerX.DecimalPlaces = 5;
            this.centerX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.centerX.Location = new System.Drawing.Point(48, 0);
            this.centerX.Maximum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            0});
            this.centerX.Minimum = new decimal(new int[] {
            -469762048,
            -590869294,
            5421010,
            -2147483648});
            this.centerX.Name = "centerX";
            this.centerX.Size = new System.Drawing.Size(111, 20);
            this.centerX.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Delta";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Center";
            // 
            // CrossPlotUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.panel1);
            this.Name = "CrossPlotUI";
            this.Size = new System.Drawing.Size(1080, 702);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CrossPlot_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CrosPlotUI_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CrosPlotUI_MouseDown);
            this.Resize += new System.EventHandler(this.CrosPlotUI_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown deltaX;
        private System.Windows.Forms.NumericUpDown centerX;
        private System.Windows.Forms.NumericUpDown deltaZ;
        private System.Windows.Forms.NumericUpDown centerZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown deltaY;
        private System.Windows.Forms.NumericUpDown centerY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}
