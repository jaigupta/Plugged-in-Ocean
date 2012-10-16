using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using Slb.Ocean.Geometry;

namespace CrossPlotter
{
    public class Surface3DRenderer
    {
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

        public double screenDistance = 0.00004, sf, cf, st, ct, R, A, B, C, D; //transformations coeficients
        public int sx, sy;
        public int quality = 7;
        public int blue = 10;
        public int bw = 0;
        public int bno = 32, radius = 3;
        int showWidth, showHeight;
        Color penColor = Color.Transparent;
        Point3[] vals;
        public int ax = 0, ay = 0, az = 0;
        SolidBrush[] brushes;
        public double minZ, maxZ;
        double[] min, max;
        Point3[] minP, maxP;
        #region Properties

        /// <summary>
        /// Surface spanning net density
        /// </summary>
        public int Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        /// <summary>
        /// Quadrilateral pen color
        /// </summary>
        public Color PenColor
        {
            get { return penColor; }
            set { penColor = value; }
        }

        #endregion
        int flg = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface3DRenderer"/> class. Calculates transformations coeficients.
        /// </summary>
        /// <param name="obsX">Observator's X position</param>
        /// <param name="obsY">Observator's Y position</param>
        /// <param name="obsZ">Observator's Z position</param>
        /// <param name="xs0">X coordinate of screen</param>
        /// <param name="ys0">Y coordinate of screen</param>
        /// <param name="screenWidth">Drawing area width in pixels.</param>
        /// <param name="screenHeight">Drawing area height in pixels.</param>
        /// <param name="screenDistance">The screen distance.</param>
        /// <param name="screenWidthPhys">Width of the screen in meters.</param>
        /// <param name="screenHeightPhys">Height of the screen in meters.</param>
        public Surface3DRenderer(double obsX, double obsY, double obsZ, int xs0, int ys0, int screenWidth,
            int screenHeight, double screenWidthPhys, double screenHeightPhys, Point3[] vals, 
            double[] min, double[] max, Point3[] minP, Point3[] maxP)
        {
            ReCalculateTransformationsCoeficients(obsX, obsY, obsZ, xs0, ys0, screenWidth, screenHeight, screenWidthPhys, screenHeightPhys);
            this.vals = vals;
            this.min = min;
            this.max = max;
            this.minP = minP;
            this.maxP = maxP;
        }

        public void ReCalculateTransformationsCoeficients(double obsX, double obsY, double obsZ, int xs0, int ys0, int screenWidth, int screenHeight,
             double screenWidthPhys, double screenHeightPhys)
        {
            double r1, a;
            if (screenWidthPhys <= 0)//when screen dimensions are not specified
                screenWidthPhys = screenWidth * 0.0257 / 72.0;        //0.0257 m = 1 inch. Screen has 72 px/inch
            if (screenHeightPhys <= 0)
                screenHeightPhys = screenHeight * 0.0257 / 72.0;
            this.showWidth = screenWidth;
            this.showHeight = screenHeight;

            r1 = obsX * obsX + obsY * obsY;
            a = Math.Sqrt(r1);//distance in XY plane
            R = Math.Sqrt(r1 + obsZ * obsZ);//distance from observator to center
            if (flg == 0)
            {
                if (a != 0) //rotation matrix coeficients calculation
                {
                    sf = obsY / a;//sin( fi)
                    cf = obsX / a;//cos( fi)
                }
                else
                {
                    sf = 0;
                    cf = 1;
                }

                st = a / R;//sin( teta)
                ct = obsZ / R;//cos( teta)
                flg = 1;
            }
            //linear tranfrormation coeficients
            A = screenWidth / screenWidthPhys;
            B = xs0 + A * screenWidthPhys / 2.0;
            C = -(double)screenHeight / screenHeightPhys;
            D = ys0 - C * screenHeightPhys / 2.0;
            this.sx = xs0;
            this.sy = ys0;
            createNewBrushes();
        }

        /// <summary>
        /// Performs projection. Calculates screen coordinates for 3D point.
        /// </summary>
        /// <param name="x">Point's x coordinate.</param>
        /// <param name="y">Point's y coordinate.</param>
        /// <param name="z">Point's z coordinate.</param>
        /// <returns>Point in 2D space of the screen.</returns>
        public PointF Project(double x, double y, double z)
        {
            double xn, yn;//point coordinates in computer's frame of reference

            //transformations
            xn = -sf * x + cf * y;
            yn = -cf * ct * x - sf * ct * y + st * z;

            //Tales' theorem
            return new PointF((float)(A * xn * screenDistance + B), (float)(C * yn * screenDistance + D));
        }
        public double getz(double x, double y, double z)
        {
            return (-cf * st * x - sf * st * y - ct * z) / Math.Sqrt(x * x + y * y + z * z)+R;
        }
        public void createNewBrushes()
        {
            brushes = new SolidBrush[64];
            if (bw == 0)
            {
                for (int i = 0, j = 0; j < brushes.Length / 2; j++, i += 8)
                {
                    brushes[j] = new SolidBrush(Color.FromArgb(0, i, 255 - i));
                    brushes[brushes.Length / 2 + j] = new SolidBrush(Color.FromArgb(i, 255 - i, 0));
                }
            }
        }

        public void RenderSurface(Graphics graphics)
        {
            double z1, z2; int i, j;
            
            PointF[] meshF = new PointF[vals.GetLength(0)];
            float[] depth = new float[vals.GetLength(0)];
            //X-Y
            try
            {
                for (i = 0; i < vals.GetLength(0); i++)
                {
                    meshF[i] = Project(vals[i].X, vals[i].Y, vals[i].Z);
                    depth[i] = (float)getz(vals[i].X, vals[i].Y, vals[i].Z);
                }
                for (i = 0; i < vals.GetLength(0) - 1; i++)
                {
                    for (j = 0; j < vals.GetLength(0) - i - 1; j++)
                    {
                        if (depth[j] < depth[j + 1])
                        {
                            PointF temp = meshF[j];
                            meshF[j] = meshF[j + 1];
                            meshF[j + 1] = temp;
                            float temp1 = depth[j];
                            depth[j] = depth[j + 1];
                            depth[j + 1] = temp1;
                        }
                    }
                }
                double xdepth = getz(1, 0, 0);
                double ydepth = getz(0, 1, 0);
                double zdepth = getz(0, 0, 1);
                int k = 0;
                while(k<vals.GetLength(0) )
                {
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.FillEllipse(brushes[bno], meshF[k].X - radius, meshF[k].Y - radius, 2 * radius, 2 * radius);
                    k++;
                }
                PointF origin = Project(0, 0, 0);
                PointF xAxis = Project(maxP[0].X, 0, 0);
                PointF yAxis = Project(0, maxP[1].Y, 0);
                PointF zAxis = Project(0, 0, maxP[2].Z);
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawLine(new Pen(Color.White), origin, xAxis);
                graphics.DrawLine(new Pen(Color.White), origin, yAxis);
                graphics.DrawLine(new Pen(Color.White), origin, zAxis);
                graphics.DrawString("X", new Font(FontFamily.GenericMonospace, 10), 
                    new SolidBrush(Color.White), Project(maxP[0].X-14, 0, 0));
                graphics.DrawString("Y", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(-14,maxP[1].Y, 0));
                graphics.DrawString("Z", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(-14, 0, maxP[2].Z));
                graphics.DrawString("(" + (float)min[0] + "," + (float)min[1] + "," + (float)min[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(0, 0, 0));
                graphics.DrawString("(" + (float)max[0] + "," + (float)min[1] + "," + (float)min[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(maxP[0].X, 0, 0));
                graphics.DrawString("(" + (float)min[0] + "," + (float)max[1] + "," + (float)min[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(0, maxP[1].Y, 0));
                graphics.DrawString("(" + (float)min[0] + "," + (float)min[1] + "," + (float)max[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(0, 0, maxP[2].Z));
                graphics.DrawString("(" + (float)(max[0]/2.0) + "," + (float)min[1] + "," + (float)min[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(maxP[0].X/2.0, 0, 0));
                graphics.DrawString("(" + (float)min[0] + "," + (float)(max[1]/2.0) + "," + (float)min[2] + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(0, maxP[1].Y/2.0, 0));
                graphics.DrawString("(" + (float)min[0] + "," + (float)min[1] + "," + (float)(max[2]/2.0) + ")", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.White), Project(0, 0, maxP[2].Z/2.0));
            }
            catch (Exception e)
            {
                ;
            }
        }
    }
}