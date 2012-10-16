using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
namespace Plot3D
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

        public double screenDistance = 0.0004, sf, cf, st, ct, R, A, B, C, D; //transformations coeficients
        public int sx, sy;
        Plot3D.Plot3DMainForm frame3D;
        public int quality = 7;
        public int blue = 10;
        public int bw = 0;
        int showWidth, showHeight;
        bool functionNeedsCalculation = true;
        Color penColor = Color.Transparent;
        RendererFunction function = defaultFunction;
        ColorSchema colorSchema = ColorSchema.Autumn;
        float[, ,] vals;
        public int ax = 0, ay = 0, az = 0;
        double[,] mesh1, mesh2, mesh3;
        SolidBrush[] brushes;
        public double minZ, maxZ;
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

        public RendererFunction Function
        {
            get { return function; }
            set { function = value; }
        }

        public ColorSchema ColorSchema
        {
            get { return colorSchema; }
            set { colorSchema = value; }
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
            int screenHeight,  double screenWidthPhys, double screenHeightPhys, float[, ,] vals, Plot3D.Plot3DMainForm frame3D)
        {
            ReCalculateTransformationsCoeficients(obsX, obsY, obsZ, xs0, ys0, screenWidth, screenHeight, screenWidthPhys, screenHeightPhys);
            this.vals = vals;
            this.frame3D = frame3D;
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
            return  -cf * st * x - sf * st * y - ct * z + R;
        }
        public void createNewBrushes()
        {
            brushes = new SolidBrush[512];
            if (bw == 0)
            {
                for (int i = 0; i < brushes.Length / 2; i++)
                {
                    brushes[i] = new SolidBrush(Color.FromArgb(0, i, 255-i));
                    brushes[brushes.Length / 2 + i] = new SolidBrush(Color.FromArgb(i, 255 - i, 0));
                }
            }
            else
            {
                for (int i = 0; i < brushes.Length; i++)
                {
                    brushes[i] = new SolidBrush(Color.FromArgb(i/2, i/2, i/2));
                }
            }
        }
        public void recalculateFunction()
        {
            createNewBrushes();
            minZ = double.PositiveInfinity;
            maxZ = double.NegativeInfinity;
            const int qjump = 1;
            mesh1 = new double[vals.GetLength(0), vals.GetLength(1)];
            mesh2 = new double[vals.GetLength(1), vals.GetLength(2)];
            mesh3 = new double[vals.GetLength(2), vals.GetLength(0)];
            int tz = vals.GetLength(2) - 1;
            for (int x = 0; x < vals.GetLength(0); x += qjump)
            {
                for (int y = 0; y < vals.GetLength(1); y += qjump)
                {
                    double zz = function(x, y, az, vals[x, y, tz - az]);
                    mesh1[x, y] = zz;
                    if (double.IsInfinity(zz) || double.IsNaN(zz))
                    {
                    }
                    else
                    {
                        if (minZ > zz) minZ = zz;
                        if (maxZ < zz) maxZ = zz;
                    }
                }
            }
            //Y-Z
            for (int y = 0; y < vals.GetLength(1); y += qjump)
            {
                for (int z = 0; z < vals.GetLength(2); z += qjump)
                {
                    double zz = function(ax, y, z, vals[ax, y, tz - z]);
                    mesh2[y, z] = zz;
                    if (double.IsInfinity(zz) || double.IsNaN(zz))
                    {
                    }
                    else
                    {
                        if (minZ > zz) minZ = zz;
                        if (maxZ < zz) maxZ = zz;
                    }
                }
            }
            //Z-X
            for (int z = 0; z < vals.GetLength(2); z += qjump)
            {
                for (int x = 0; x < vals.GetLength(0); x += qjump)
                {
                    double zz = function(x, ay, z, vals[x, ay, tz - z]);
                    mesh3[z, x] = zz;
                    if (double.IsInfinity(zz) || double.IsNaN(zz))
                    {
                    }
                    else
                    {
                        if (minZ > zz) minZ = zz;
                        if (maxZ < zz) maxZ = zz;
                    }
                }
            }

            for (int x = 0; x < vals.GetLength(0); x += qjump)
            {
                for (int y = 0; y < vals.GetLength(1); y += qjump)
                {
                    if (double.IsInfinity(mesh1[x, y]) || double.IsNaN(mesh1[x, y]))
                    {
                        if (double.IsPositiveInfinity(mesh1[x, y]))
                            mesh1[x, y] = maxZ;
                        else
                            mesh1[x, y] = minZ;
                    }
                }
            }
            //Y-Z
            for (int y = 0; y < vals.GetLength(1); y += qjump)
            {
                for (int z = 0; z < vals.GetLength(2); z += qjump)
                {
                    if (double.IsInfinity(mesh2[y, z]) || double.IsNaN(mesh2[y, z]))
                    {
                        if (double.IsPositiveInfinity(mesh2[y, z]))
                            mesh2[y, z] = maxZ;
                        else
                            mesh2[y, z] = minZ;
                    }
                }
            }
            //Z-X
            for (int z = 0; z < vals.GetLength(2); z += qjump)
            {
                for (int x = 0; x < vals.GetLength(0); x += qjump)
                {
                    if (double.IsInfinity(mesh3[z, x]) || double.IsNaN(mesh3[z, x]))
                    {
                        if (double.IsPositiveInfinity(mesh3[z, x]))
                            mesh3[z, x] = maxZ;
                        else
                            mesh3[z, x] = minZ;
                    }
                }
            }
            functionNeedsCalculation = false;
        }

        public void RenderSurface(Graphics graphics)
        {
            double z1, z2;
            PointF[] polygon = new PointF[4];

            PointF[,] meshF1 = new PointF[vals.GetLength(0), vals.GetLength(1)];
            PointF[,] meshF2 = new PointF[vals.GetLength(1), vals.GetLength(2)];
            PointF[,] meshF3 = new PointF[vals.GetLength(2), vals.GetLength(0)];
            int qjump = 11 - quality;
            //X-Y
            try
            {
                for (int x = ax % qjump; x < vals.GetLength(0); x += qjump)
                {
                    for (int y = ay%qjump; y < vals.GetLength(1); y += qjump)
                    {
                        meshF1[x, y] = Project(x, y, az);
                    }
                }
                //Y-Z
                for (int y = ay%qjump; y < vals.GetLength(1); y += qjump)
                {
                    for (int z = az%qjump; z < vals.GetLength(2); z += qjump)
                    {
                        meshF2[y, z] = Project(ax, y, z);
                    }
                }
                //Z-X
                for (int z = az%qjump; z < vals.GetLength(2); z += qjump)
                {
                    for (int x = ax%qjump; x < vals.GetLength(0); x += qjump)
                    {
                        meshF3[z, x] = Project(x, ay, z);
                    }
                }
                double cc = (maxZ - minZ) / (brushes.Length - 1.0);
                double[] depth = new double[12];
                int[] plane = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                depth[0] = getz(1, 1, 0);
                depth[1] = getz(1, -1, 0);
                depth[2] = getz(1, 0, 1);
                depth[3] = getz(1, 0, -1);
                depth[4] = getz(-1, 1, 0);
                depth[5] = getz(-1, -1, 0);
                depth[6] = getz(-1, 0, 1);
                depth[7] = getz(-1, 0, -1);
                depth[8] = getz(0, 1, 1);
                depth[9] = getz(0, 1, -1);
                depth[10] = getz(0, -1, 1);
                depth[11] = getz(0, -1, -1);
            
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11-i; j++)
                    {
                        if (depth[j] < depth[j + 1])
                        {
                            double temp = depth[j];
                            depth[j] = depth[j + 1];
                            depth[j + 1] = temp;
                            int temp2 = plane[j];
                            plane[j] = plane[j + 1];
                            plane[j + 1] = temp2;
                        }
                    }
                }

                if (cc == 0)
                    cc = 1;
                Pen pen2 = new Pen(Color.White);
                PointF p1, p2, p3, p4;
                using (Pen pen = new Pen(penColor))
                {
                    int x=ax,y=ay;
                    for (int i = 0; i < 12; i++)
                    {
                        switch (plane[i])
                        {
                            case 0:
                                y = ay;
                                for (x = ax; x < vals.GetLength(0) - qjump - 10; x += qjump)
                                {
                                    for (y = ay; y < vals.GetLength(1) - qjump - 10; y += qjump)
                                    {
                                        z1 = mesh1[x, y];
                                        z2 = mesh1[x, y + qjump];

                                        polygon[0] = meshF1[x, y];
                                        polygon[1] = meshF1[x, y + qjump];
                                        polygon[2] = meshF1[x + qjump, y + qjump];
                                        polygon[3] = meshF1[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF1[ax,ay];
                                p2 = meshF1[ax,y];
                                p3 = meshF1[x,y];
                                p4 = meshF1[x,ay];
                                graphics.DrawLine(pen2,p1,p2);
                                graphics.DrawLine(pen2,p2,p3);
                                graphics.DrawLine(pen2,p3,p4);
                                graphics.DrawLine(pen2,p4,p1);
                                break;
                            case 1:
                                y = ay;
                                for (x = ax; x < vals.GetLength(0) - qjump -10; x += qjump)
                                {
                                    for (y = ay; y >= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh1[x, y];
                                        z2 = mesh1[x, y - qjump];

                                        polygon[0] = meshF1[x, y];
                                        polygon[1] = meshF1[x, y - qjump];
                                        polygon[2] = meshF1[x + qjump, y - qjump];
                                        polygon[3] = meshF1[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF1[ax, ay];
                                p2 = meshF1[ax, y];
                                p3 = meshF1[x, y];
                                p4 = meshF1[x, ay];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 2:
                                y = ax;
                                for (x = az; x < vals.GetLength(2) - qjump - 10; x += qjump)
                                {
                                    for (y = ax; y < vals.GetLength(0) - qjump -10; y += qjump)
                                    {
                                        z1 = mesh3[x, y];
                                        z2 = mesh3[x, y + qjump];

                                        polygon[0] = meshF3[x, y];
                                        polygon[1] = meshF3[x, y + qjump];
                                        polygon[2] = meshF3[x + qjump, y + qjump];
                                        polygon[3] = meshF3[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF3[az, ax];
                                p2 = meshF3[az, y];
                                p3 = meshF3[x, y];
                                p4 = meshF3[x, ax];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 3:
                                y = ax;
                                for (x = az; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = ax; y < vals.GetLength(0) - qjump - 10; y += qjump)
                                    {
                                        z1 = mesh3[x, y];
                                        z2 = mesh3[x, y + qjump];

                                        polygon[0] = meshF3[x, y];
                                        polygon[1] = meshF3[x, y + qjump];
                                        polygon[2] = meshF3[x - qjump, y + qjump];
                                        polygon[3] = meshF3[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF3[az, ax];
                                p2 = meshF3[az, y];
                                p3 = meshF3[x, y];
                                p4 = meshF3[x, ax];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 4:
                                y = ay;
                                for (x = ax; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = ay; y < vals.GetLength(1) - qjump - 10; y += qjump)
                                    {
                                        z1 = mesh1[x, y];
                                        z2 = mesh1[x, y + qjump];

                                        polygon[0] = meshF1[x, y];
                                        polygon[1] = meshF1[x, y + qjump];
                                        polygon[2] = meshF1[x - qjump, y + qjump];
                                        polygon[3] = meshF1[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF1[ax, ay];
                                p2 = meshF1[ax, y];
                                p3 = meshF1[x, y];
                                p4 = meshF1[x, ay];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 5:
                                try
                                {
                                    y = ay;
                                for (x = ax; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = ay; y >= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh1[x, y];
                                        z2 = mesh1[x, y - qjump];

                                        polygon[0] = meshF1[x, y];
                                        polygon[1] = meshF1[x, y - qjump];
                                        polygon[2] = meshF1[x - qjump, y - qjump];
                                        polygon[3] = meshF1[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF1[ax, ay];
                                p2 = meshF1[ax, y];
                                p3 = meshF1[x, y];
                                p4 = meshF1[x, ay];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                }catch{
                                    ;
                                }
                                break;
                            case 6:
                                y = ax;
                                for (x = az; x < vals.GetLength(2) - qjump - 10; x += qjump)
                                {
                                    for (y = ax; y>= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh3[x, y];
                                        z2 = mesh3[x, y - qjump];

                                        polygon[0] = meshF3[x, y];
                                        polygon[1] = meshF3[x, y - qjump];
                                        polygon[2] = meshF3[x + qjump, y - qjump];
                                        polygon[3] = meshF3[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF3[az, ax];
                                p2 = meshF3[az, y];
                                p3 = meshF3[x, y];
                                p4 = meshF3[x, ax];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 7:
                                y = ax;
                                for (x = az; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = ax; y >= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh3[x, y];
                                        z2 = mesh3[x, y - qjump];

                                        polygon[0] = meshF3[x, y];
                                        polygon[1] = meshF3[x, y - qjump];
                                        polygon[2] = meshF3[x - qjump, y - qjump];
                                        polygon[3] = meshF3[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF3[az, ax];
                                p2 = meshF3[az, y];
                                p3 = meshF3[x, y];
                                p4 = meshF3[x, ax];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 8:
                                y = az;
                                for (x = ay; x < vals.GetLength(1) - qjump - 10; x += qjump)
                                {
                                    for (y = az; y < vals.GetLength(2) - qjump - 10; y += qjump)
                                    {
                                        z1 = mesh2[x, y];
                                        z2 = mesh2[x, y + qjump];

                                        polygon[0] = meshF2[x, y];
                                        polygon[1] = meshF2[x, y + qjump];
                                        polygon[2] = meshF2[x + qjump, y + qjump];
                                        polygon[3] = meshF2[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF2[ay, az];
                                p2 = meshF2[ay, y];
                                p3 = meshF2[x, y];
                                p4 = meshF2[x, az];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 9:
                                y = az;
                                for (x = ay; x < vals.GetLength(1) - qjump - 10; x += qjump)
                                {
                                    for (y = az; y >= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh2[x, y];
                                        z2 = mesh2[x, y - qjump];

                                        polygon[0] = meshF2[x, y];
                                        polygon[1] = meshF2[x, y - qjump];
                                        polygon[2] = meshF2[x + qjump, y - qjump];
                                        polygon[3] = meshF2[x + qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF2[ay, az];
                                p2 = meshF2[ay, y];
                                p3 = meshF2[x, y];
                                p4 = meshF2[x, az];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 10:
                                y = az;
                                for (x = ay; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = az; y < vals.GetLength(2) - qjump - 10; y += qjump)
                                    {
                                        z1 = mesh2[x, y];
                                        z2 = mesh2[x, y + qjump];

                                        polygon[0] = meshF2[x, y];
                                        polygon[1] = meshF2[x, y + qjump];
                                        polygon[2] = meshF2[x - qjump, y + qjump];
                                        polygon[3] = meshF2[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF2[ay, az];
                                p2 = meshF2[ay, y];
                                p3 = meshF2[x, y];
                                p4 = meshF2[x, az];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                            case 11:
                                y = az;
                                for (x = ay; x >= qjump + 10; x -= qjump)
                                {
                                    for (y = az; y >= qjump + 10; y -= qjump)
                                    {
                                        z1 = mesh2[x, y];
                                        z2 = mesh2[x, y - qjump];

                                        polygon[0] = meshF2[x, y];
                                        polygon[1] = meshF2[x, y - qjump];
                                        polygon[2] = meshF2[x - qjump, y - qjump];
                                        polygon[3] = meshF2[x - qjump, y];

                                        if (polygon[2].X > 1.1 * showWidth || polygon[2].Y > 1.1 * showHeight ||
                                            polygon[2].X < -0.1 * showWidth || polygon[2].Y < -0.1 * showHeight ||
                                            polygon[0].X > 1.1 * showWidth || polygon[0].Y > 1.1 * showHeight ||
                                            polygon[0].X < -0.1 * showWidth || polygon[0].Y < -0.1 * showHeight)
                                            continue;
                                        graphics.SmoothingMode = SmoothingMode.None;
                                        int bno = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                                        if (bno >= 0 && bno < brushes.Length)
                                        {
                                            graphics.FillPolygon(brushes[bno], polygon);
                                            graphics.DrawPolygon(pen, polygon);
                                        }
                                    }
                                }
                                p1 = meshF2[ay, az];
                                p2 = meshF2[ay, y];
                                p3 = meshF2[x, y];
                                p4 = meshF2[x, az];
                                graphics.DrawLine(pen2, p1, p2);
                                graphics.DrawLine(pen2, p2, p3);
                                graphics.DrawLine(pen2, p3, p4);
                                graphics.DrawLine(pen2, p4, p1);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ;
            }
        }
        public static RendererFunction GetFunctionHandle(string formula)
        {
            CompiledFunction fn = FunctionCompiler.Compile(4, formula);
            return new RendererFunction(delegate(double x, double y, double z, double val)
            {
                return fn(x, y, z, val);
            });
        }

        public void SetFunction(string formula)
        {
            try
            {
                function = GetFunctionHandle(formula);
            }
            catch
            {
                MessageBox.Show("The Expression contains errors!");
            }
        }

        private static double defaultFunction(double x, double y, double z, double val)
        {
            return val;
        }
    }

    public delegate double RendererFunction(double x, double y, double z, double val);
}
