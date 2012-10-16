using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.IO;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Core;
using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.UI;

namespace Plot3D
{
    public partial class Plot3DMainForm : Form
    {
        Surface3DRenderer sr;
        float[, ,] vals;
        int lx = 0, ly = 0;
        float sd = 0.0001f;
        public SeismicCube scube, newCube;
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        public Plot3DMainForm(float[, ,] vals)
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            sr = new Surface3DRenderer(70, 35, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height, 0, 0, vals, this);
            sr.SetFunction("val");
            sr.recalculateFunction();
            minNum.Minimum = (decimal)sr.minZ;
            maxNum.Maximum = (decimal)sr.maxZ;
            maxNum.Minimum = (decimal)sr.minZ;
            minNum.Maximum = (decimal)sr.maxZ;
            minNum.Value = (decimal)sr.minZ;
            maxNum.Value = (decimal)sr.maxZ;

            Form1_Resize(null, null);
            ResizeRedraw = true;
            DoubleBuffered = true;
            this.vals = vals;
            trackBar2.Value = sr.Quality;
            xNum.Maximum = vals.GetLength(0);
            yNum.Maximum = vals.GetLength(1);
            zNum.Maximum = vals.GetLength(2);
            xNum.Value = 1;
            yNum.Value = 1;
            zNum.Value = 1;
            xNum.Minimum = 1;
            yNum.Minimum = 1;
            zNum.Minimum = 1;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            sr.RenderSurface(e.Graphics);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            sr.ReCalculateTransformationsCoeficients(70, 35, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
        }

        private void tbHue_Scroll(object sender, EventArgs e)
        {
        }

        private void Plot3DMainForm_MouseDown_1(object sender, MouseEventArgs e)
        {
            lx = e.X;
            ly = e.Y;
            sr.Quality = 1 + trackBar2.Value / 2;
        }

        private void Plot3DMainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                sr.sf = Math.Sin(Math.Atan2(sr.sf, sr.cf) + ((lx - e.X)) / 600.0);
                sr.cf = Math.Cos(Math.Atan2(sr.sf, sr.cf) + ((lx - e.X)) / 600.0);
                sr.st = Math.Sin(Math.Atan2(sr.st, sr.ct) + ((ly - e.Y)) / 600.0);
                sr.ct = Math.Cos(Math.Atan2(sr.st, sr.ct) + ((ly - e.Y)) / 600.0);
                Invalidate();
                lx = e.X;
                ly = e.Y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx + e.X - lx, sr.sy + e.Y - ly, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
                Invalidate();
                lx = e.X;
                ly = e.Y;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            sr.Quality = trackBar2.Value;
            Invalidate();
        }

        private void Plot3DMainForm_Click(object sender, EventArgs e)
        {
            MouseEventArgs a = e as MouseEventArgs;
            sr.Quality = trackBar2.Value;
            Invalidate();
        }

        private void minNum_ValueChanged(object sender, EventArgs e)
        {
            sr.minZ = (double)minNum.Value;
            sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx, sr.sy, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
            Invalidate();
        }

        private void maxNum_ValueChanged(object sender, EventArgs e)
        {
            sr.maxZ = (double)maxNum.Value;
            sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx, sr.sy, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
            Invalidate();
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            sr.screenDistance *= 1.1;
            sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx, sr.sy, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
            Invalidate();
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            sr.screenDistance /= 1.1;
            sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx, sr.sy, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
            Invalidate();
        }

        private void xNum_ValueChanged(object sender, EventArgs e)
        {
            sr.ax = (int)xNum.Value - 1;
            sr.recalculateFunction();
            Invalidate();
        }

        private void yNum_ValueChanged(object sender, EventArgs e)
        {
            sr.ay = (int)yNum.Value - 1;
            sr.recalculateFunction();
            Invalidate();
        }

        private void zNum_ValueChanged(object sender, EventArgs e)
        {
            sr.az = (int)zNum.Value - 1;
            sr.recalculateFunction();
            Invalidate();
        }

        private void recalBtn_Click(object sender, EventArgs e)
        {
            sr.SetFunction(textBox1.Text);
            sr.recalculateFunction();
            minNum.Minimum = (decimal)sr.minZ;
            minNum.Value = (decimal)sr.minZ;
            maxNum.Maximum = (decimal)sr.maxZ;
            maxNum.Value = (decimal)sr.maxZ;
            Invalidate();
        }

        private void tbHue_Scroll_1(object sender, EventArgs e)
        {
            sr.createNewBrushes();
            Invalidate();
        }

        private void bw_CheckedChanged(object sender, EventArgs e)
        {
            if (this.bw.Checked == true)
            {
                sr.bw = 1;
                pictureBox2.Visible = true;
            }
            else
            {
                sr.bw = 0;
                pictureBox2.Visible = false;
            }
            sr.createNewBrushes();
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\help.chm");
                sw.Close();
                FileStream fs = new FileStream("C:\\help.chm", FileMode.Open, FileAccess.Write);
                Stream str;

                fs.Write(global::WellReader.Resource_chracterizer.GeoCom_3D_Visualizer, 0, global::WellReader.Resource_chracterizer.GeoCom_3D_Visualizer.Length);
                fs.Close();

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\help.chm";
                p.Start();
            }
            catch
            {
                PetrelLogger.InfoOutputWindow("Unable to Read to your drive -check permissions");
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PetrelLogger.InfoOutputWindow("Generating array for edited cube");
            float[, ,] newVal = new float[scube.NumSamplesIJK.I, scube.NumSamplesIJK.J, scube.NumSamplesIJK.K];
            double min_newVal = 10e+16;
            double max_newVal = 10e-16;
            using (IProgress i1 = PetrelLogger.NewProgress(1, scube.NumSamplesIJK.I))
            {
                for (int ii = 0; ii < scube.NumSamplesIJK.I; ii++)
                {
                    for (int jj = 0; jj < scube.NumSamplesIJK.J; jj++)
                    {
                        for (int kk = 0; kk < scube.NumSamplesIJK.K; kk++)
                        {
                            newVal[ii, jj, kk] = (float)sr.Function(ii, jj, kk, vals[ii, jj, kk]);
                            if(newVal[ii,jj,kk]<min_newVal)
                                min_newVal=(double)newVal[ii,jj,kk];
                            if(newVal[ii,jj,kk]>max_newVal)
                                max_newVal=(double)newVal[ii,jj,kk];

                        }
                    }
                    i1.ProgressStatus = ii + 1;
                }
            }
            using (ITransaction txn = DataManager.NewTransaction())
            {
                try
                {
                    txn.Lock(scube.SeismicCollection);
                    Index3 size = new Index3(scube.NumSamplesIJK.I, scube.NumSamplesIJK.J, scube.NumSamplesIJK.K);
                    IndexDouble3 tempindex = new IndexDouble3(0, 0, 0);
                    Point3 origin = scube.PositionAtIndex(tempindex);

                    double d1, d2, d3;

                    d1 = scube.PositionAtIndex(new IndexDouble3(1, 0, 0)).X - origin.X;
                    d2 = scube.PositionAtIndex(new IndexDouble3(1, 0, 0)).Y - origin.Y;
                    d3 = scube.PositionAtIndex(new IndexDouble3(1, 0, 0)).Z - origin.Z;
                    Vector3 iVec = new Vector3(d1, d2, d3);

                    d1 = scube.PositionAtIndex(new IndexDouble3(0, 1, 0)).X - origin.X;
                    d2 = scube.PositionAtIndex(new IndexDouble3(0, 1, 0)).Y - origin.Y;
                    d3 = scube.PositionAtIndex(new IndexDouble3(0, 1, 0)).Z - origin.Z;
                    Vector3 jVec = new Vector3(d1, d2, d3);

                    d1 = scube.PositionAtIndex(new IndexDouble3(0, 0, 1)).X - origin.X;
                    d2 = scube.PositionAtIndex(new IndexDouble3(0, 0, 1)).Y - origin.Y;
                    d3 = scube.PositionAtIndex(new IndexDouble3(0, 0, 1)).Z - origin.Z;
                    Vector3 kVec = new Vector3(d1, d2, d3);

                    if (scube.SeismicCollection.CanCreateSeismicCube(size, origin, iVec, jVec, kVec))
                    {
                        Type dataType = typeof(float);
                        Domain vDomain = scube.Domain;
                        IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                        ILogTemplate glob = pvs.FindTemplateByMnemonics("Seismic");
                        PropertyVersion pv = pvs.FindOrCreate(glob);
                        Range1<double> r = new Range1<double>(min_newVal, max_newVal);
                        PetrelLogger.InfoOutputWindow("OUTPUT TEMPLATE UNDER PROCESS");
                        try
                        {
                            newCube = scube.SeismicCollection.CreateSeismicCube(size, origin, iVec, jVec, kVec, dataType, vDomain, pv, r);
                            PetrelLogger.InfoOutputWindow("OUTPUT CUBE  TEMPLATE Construction completed");
                        }
                        catch (System.InvalidOperationException e1)
                        {
                            PetrelLogger.ErrorBox(e1.Message);
                        }
                        catch (System.ArgumentNullException e1)
                        {
                            PetrelLogger.InfoOutputWindow(e1.Message);
                        }
                    }
                    newCube.Name = cubename.Text;
                    if (newCube.IsWritable)
                    {
                        txn.Lock(newCube);
                        PetrelLogger.InfoOutputWindow("Writing Data in the new cube");
                        Index3 start = new Index3(0, 0, 0);
                        Index3 end = new Index3(newCube.NumSamplesIJK.I - 1, newCube.NumSamplesIJK.J - 1, newCube.NumSamplesIJK.K - 1);
                        ISubCube to = newCube.GetSubCube(start, end);
                        to.CopyFrom(newVal);
                        txn.Commit();
                    }
                }
                catch (ArgumentNullException e2)
                {
                    PetrelLogger.ErrorBox("Seismic cube name or propertyVersion can not be blank (null)" + e2.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
                this.textBox1.Text = "val";
            if(this.comboBox1.SelectedIndex==1)
                this.textBox1.Text = "sin(val)";
            if (this.comboBox1.SelectedIndex == 2)
                this.textBox1.Text = "cos(val)";
            if (this.comboBox1.SelectedIndex == 3)
                this.textBox1.Text = "tan(val)";
            if (this.comboBox1.SelectedIndex == 4)
                this.textBox1.Text = "cot(val)";
            if (this.comboBox1.SelectedIndex == 5)
            {

                this.textBox1.Text = "val; \nif(val>1 || val<-1) \nresult=0;\nelse \nresult=asin(val)";
            }
            if (this.comboBox1.SelectedIndex == 6)
            {

                this.textBox1.Text = "val; \nif(val>1 || val<-1) \nresult=0;\nelse \nresult=acos(val)";
            }
            if (this.comboBox1.SelectedIndex == 7)
                this.textBox1.Text = "atan(val)";
            if (this.comboBox1.SelectedIndex == 8)
                this.textBox1.Text = "val; \nif(val<0) \nresult=0;\nelse \nresult=log10(val)";
            if (this.comboBox1.SelectedIndex == 9)
                this.textBox1.Text = "val; \nif(val<0) \nresult=0;\nelse \nresult=log2(val)";
            if (this.comboBox1.SelectedIndex == 10)
                this.textBox1.Text = "val; \nif(val<0) \nresult=0;\nelse \nresult=log(val)";
            if (this.comboBox1.SelectedIndex == 11)
                this.textBox1.Text = "round(val)";
            if (this.comboBox1.SelectedIndex == 12)
                this.textBox1.Text = "ceil(val)";
            if (this.comboBox1.SelectedIndex == 13)
                this.textBox1.Text = "floor(val)";
            if (this.comboBox1.SelectedIndex == 14)
                this.textBox1.Text = "pow(val, 2)";
            if (this.comboBox1.SelectedIndex == 15)
                this.textBox1.Text = "pow(val, 3)";
            if (this.comboBox1.SelectedIndex == 16)
                this.textBox1.Text = "1/val";


        }
    }
}
              
