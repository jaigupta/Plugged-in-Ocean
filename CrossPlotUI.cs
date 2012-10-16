using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Well;
using System.Collections.Generic;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using System.IO;

namespace CrossPlotter
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    public partial class CrossPlotUI : Form
    {
        Surface3DRenderer sr = null;
        int lx, ly;
        private WellLog logX, logY, logZ;
        /// <summary>
        /// Initializes a new instance of the <see cref="Workstep1UI"/> class.
        /// </summary>
        /// <param name="workstep">the workstep instance</param>
        /// <param name="args">the arguments</param>
        /// <param name="context">the underlying context in which this UI is being used</param>
        public CrossPlotUI(WellLog _x, WellLog _y, WellLog _z)
        {
            InitializeComponent();
            logX = _x;
            logY = _y;
            logZ = _z;
            DoubleBuffered = true;
            ResizeRedraw = true;
            List<WellLogSample> listX = new List<WellLogSample>(logX.Samples);
            List<WellLogSample> listY = new List<WellLogSample>(logY.Samples);
            List<WellLogSample> listZ = new List<WellLogSample>(logZ.Samples);
            if (listX.Count == 0 || listY.Count == 0 || listZ.Count == 0)
            {
                PetrelLogger.ErrorBox("NULL LOG found");

            }
            else
            {
                List<float> xvals = new List<float>(listX.Count);
                List<float> yvals = new List<float>(listY.Count);
                List<float> zvals = new List<float>(listZ.Count);
                List<WellLogSample> abc = new List<WellLogSample>(logX.Samples);
                for (int i = 0; i < listX.Count; i++)
                {
                    xvals.Add(listX[i].Value);
                    yvals.Add(listY[i].Value);
                    zvals.Add(listZ[i].Value);
                }
                xvals.Sort();
                yvals.Sort();
                zvals.Sort();

                List<float> valsx = new List<float>(logX.SampleCount);
                List<WellLogSample> samp = new List<WellLogSample>(logX.Samples);
                for (int i = 0; i < samp.Count; i++)
                {
                    valsx.Add(samp[i].Value);
                }
                valsx.Sort();
                centerX.Value = (decimal)valsx[0];
                float delta = 10000;
                for (int i = 0; i < valsx.Count - 1; i++)
                {
                    if (delta > valsx[i + 1] - valsx[i] &&
                        valsx[i + 1] - valsx[i] > 0.00001)
                        delta = valsx[i + 1] - valsx[i];
                }
                deltaX.Value = (decimal)delta;


                List<float> valsy = new List<float>(logY.SampleCount);
                samp = new List<WellLogSample>(logY.Samples);
                for (int i = 0; i < samp.Count; i++)
                {
                    valsy.Add(samp[i].Value);
                }
                valsy.Sort();
                centerY.Value = (decimal)valsy[0];
                delta = 100000;
                for (int i = 0; i < valsy.Count - 1; i++)
                {
                    if (delta > valsy[i + 1] - valsy[i] &&
                        valsy[i + 1] - valsy[i] > 0.00001)
                        delta = valsy[i + 1] - valsy[i];
                }
                deltaY.Value = (decimal)delta;
                List<float> valsz = new List<float>(logZ.SampleCount);
                samp = new List<WellLogSample>(logZ.Samples);
                for (int i = 0; i < samp.Count; i++)
                {
                    valsz.Add(samp[i].Value);
                }
                valsz.Sort();
                centerZ.Value = (decimal)valsz[0];
                delta = 100000;
                for (int i = 0; i < valsz.Count - 1; i++)
                {
                    if (delta > valsz[i + 1] - valsz[i] &&
                        valsz[i + 1] - valsz[i] > 0.00001)
                        delta = valsz[i + 1] - valsz[i];
                }
                deltaZ.Value = (decimal)delta;

                float xc = (float)centerX.Value;
                float yc = (float)centerY.Value;
                float zc = (float)centerZ.Value;
                float dx = (float)deltaX.Value;
                float dy = (float)deltaY.Value;
                float dz = (float)deltaZ.Value;
                Point3[] vals = new Point3[listX.Count];
                double[] min = new double[3];
                double[] max = new double[3];
                Point3[] minP = new Point3[3];
                Point3[] maxP = new Point3[3];
                max[0] = double.NegativeInfinity;
                max[1] = double.NegativeInfinity;
                max[2] = double.NegativeInfinity;
                min[0] = double.PositiveInfinity;
                min[1] = double.PositiveInfinity;
                min[2] = double.PositiveInfinity;
                for (int i = 0; i < listX.Count; i++)
                {
                    vals[i] = new Point3((listX[i].Value - xc) / dx, (listY[i].Value - yc) / dy,
                        (listZ[i].Value - zc) / dz);
                    if (vals[i].X < min[0])
                    {
                        min[0] = vals[i].X;
                        minP[0] = vals[i];
                    }
                    if (vals[i].Y < min[1])
                    {
                        min[1] = vals[i].Y;
                        minP[1] = vals[i];
                    }
                    if (vals[i].Z < min[2])
                    {
                        min[2] = vals[i].Z;
                        minP[2] = vals[i];
                    }
                    if (vals[i].X > max[0])
                    {
                        max[0] = vals[i].X;
                        maxP[0] = vals[i];
                    }
                    if (vals[i].Y > max[1])
                    {
                        max[1] = vals[i].Y;
                        maxP[1] = vals[i];
                    }
                    if (vals[i].Z > max[2])
                    {
                        max[2] = vals[i].Z;
                        maxP[2] = vals[i];
                    }
                }

                sr = new Surface3DRenderer(70, 35, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height,
                    0, 0, vals, min, max, minP, maxP);
                Invalidate();
            }
        }

        private void CrossPlot_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            if (sr != null)
                sr.RenderSurface(e.Graphics);
        }

        private void CrosPlotUI_Resize(object sender, EventArgs e)
        {
            if (sr != null)
                sr.ReCalculateTransformationsCoeficients(70, 35, 40, 0, 0, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
            Invalidate();
        }

        private void CrosPlotUI_MouseDown(object sender, MouseEventArgs e)
        {
            lx = e.X;
            ly = e.Y;
        }

        private void CrosPlotUI_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sr!=null)
            {
                sr.sf = Math.Sin(Math.Atan2(sr.sf, sr.cf) + ((lx - e.X)) / 600.0);
                sr.cf = Math.Cos(Math.Atan2(sr.sf, sr.cf) + ((lx - e.X)) / 600.0);
                sr.st = Math.Sin(Math.Atan2(sr.st, sr.ct) + ((ly - e.Y)) / 600.0);
                sr.ct = Math.Cos(Math.Atan2(sr.st, sr.ct) + ((ly - e.Y)) / 600.0);
                Invalidate();
                lx = e.X;
                ly = e.Y;
            }
            else if (e.Button == MouseButtons.Right && sr!=null)
            {
                sr.ReCalculateTransformationsCoeficients(70, 35, 40, sr.sx + e.X - lx, sr.sy + e.Y - ly, ClientRectangle.Width, ClientRectangle.Height, 0, 0);
                Invalidate();
                lx = e.X;
                ly = e.Y;
            }
            else if(sr!=null && e.Delta > 0)
            {
                sr.screenDistance *= 1 + e.Delta / 10;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(sr!=null)
                sr.screenDistance *= 1.1;
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(sr!=null)
                sr.screenDistance /= 1.1;
            Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(sr!=null)
                sr.radius = (int)numericUpDown1.Value;
            Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(sr!=null)
                sr.bno = (int)trackBar1.Value;
            Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\help.chm");
                sw.Close();
                FileStream fs = new FileStream("C:\\help.chm", FileMode.Open, FileAccess.Write);
                Stream str;

                fs.Write(global::WellReader.Resource_chracterizer.GeoCom_3D_Crossplot, 0, global::WellReader.Resource_chracterizer.GeoCom_3D_Crossplot.Length);
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
    }
}
