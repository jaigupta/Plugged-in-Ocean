using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System.Collections.Generic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Geometry;

namespace WellReader
{
    public struct node
    {
        public float min0, min1;
        public float max0, max1;
        public int depth;
        public float value;
        ///public node next;
    }

    public struct vnode
    {
        public float val;
        public int v1, v2;
    };

    public struct record
    {
        int[] index;
        int count;
        //public record next;
        //public record prev;
        float dfunc;
    };   
    class RACO
    {
        static float dummy_phi = 0;
        static int ctr = 0;
        static double por, Sw, Cc;
        public static void workingFunction(WellLog density, WellLog sonic)
        {
            List<WellLogSample> den = new List<WellLogSample>(density.Samples);
            List<WellLogSample> dt = new List<WellLogSample>(sonic.Samples);
            Console.WriteLine(den.Count);
            Console.WriteLine(dt.Count);


            int i;
            double startMD, endMD;
            int startden=0, endden=0, startdt=0, enddt=0;
            int count=0;

            if (!(den[0].MD == dt[0].MD && den[den.Count - 1].MD == dt[dt.Count - 1].MD))
            {
                if (den[0].MD > dt[0].MD)
                    startMD = den[0].MD;
                else
                    startMD = dt[0].MD;
                if (den[den.Count - 1].MD > dt[dt.Count - 1].MD)
                    endMD = dt[dt.Count - 1].MD;
                else
                    endMD = den[den.Count].MD;
                for (int xxx = 0; xxx < den.Count; xxx++)
                {
                    if (den[xxx].MD == startMD)
                        startden = xxx;
                    if (den[xxx].MD == endMD)
                        endden = xxx;
                }
                for (int xxx = 0; xxx < dt.Count; xxx++)
                {
                    if (dt[xxx].MD == startMD)
                        startdt = xxx;
                    if (dt[xxx].MD == endMD)
                        enddt = xxx;
                }
                count = endden - startden + 1;
            }
            else
                count = den.Count;
            float[] C2 = new float[count];
            float[] S2 = new float[count];
            float[] phi2 = new float[count];
            float[] error1 = new float[count];

            using (IProgress i1 = PetrelLogger.NewProgress(1, count))
            {
                for (i = 0; i < count; i++)
                {

                    float ac_imp;
                    if (den[i+startden].Value.ToString() == "NaN")
                    {
                        continue;
                    }
                    if (dt[i+startdt].Value.ToString() == "NaN")
                        continue;
                    float rho = den[i+startden].Value;
                    float son = dt[i+startdt].Value;
                    float Vinv = 1.0f / son;
                    ac_imp = rho * Vinv;
                    error1[i] = 1.0f;
                    for (float phi = 0.01f; phi <= 0.6f; phi += 0.01f)
                    {
                        for (float S = 0.01f; S <= 1.0f; S += 0.01f)
                        {
                            for (float C = 0.01f; C <= 1.0f; C += 0.01f)
                            {
                                double error = dfunc(ac_imp, rho, Vinv, C, S, phi);
                                if (error1[i] > (float)error)
                                {
                                    C2[i] = C;
                                    S2[i] = S;
                                    phi2[i] = phi;
                                    error1[i] = (float)error;

                                }
                            }
                        }
                    }
                    i1.ProgressStatus = i + 1;
                }
            }
         }
        

        

        public static double dfunc(float ac_imp, float rho, float Vinv, float C, float S, float phi)
        {
            double sqdiff;
            Int64 Kc = 20900000000;
            Int64 Kq = 37000000000;
            Int64 Gc = 6850000000;
            Int64 Gq = 44000000000;
            Int64 Kw = 2540000000;
            Int64 Ko = 1410000000;
            int rhoC, rhoQ, rhoW, rhoO;
            float rhoF, rhoM, rhoB;
            rhoC = 2580;
            rhoQ = 2650;
            rhoW = 1040;
            rhoO = 824;
            rhoF = (1 - S) * rhoO + S * rhoW;
            rhoM = (1 - C) * rhoQ + C * rhoC;
            rhoB = (1 - phi) * rhoM + phi * rhoF;


            double Km;
            Km = 0.5 * (C * Kc + (1 - C) * Kq + 1 / (C / Kc + (1 - C) / Kq));
            double Gm;
            Gm = 0.5 * (C * Gc + (1 - C) * Gq + 1 / (C / Gc + (1 - C) / Gc));
            double Kf;
            Kf = 0.5 * (S * Kw + (1 - S) * Ko + 1 / (S / Kw + (1 - S) / Ko));
            double Vm, Vf, Vc, Zc;
            Vm = Math.Sqrt((Km + Gm) / rhoM);
            Vf = Math.Sqrt(Kf / rhoF);

            //Raymer's model for velocity estimation
            Vc = Math.Pow((1 - phi), 2) * Vm + phi * Vf;
            Zc = Vc * rhoB;
            //double Kb, Gb, poissonc;
            //Kb = 0.5 * (phi * Kf + (1 - phi) * Km + 1 / (phi / Kf + (1 - phi) / Km));
            //Gb = Gm;
            //poissonc = (3 * Kb - 2 * Gb) / (2 * (3 * Kb + Gb));
                sqdiff = Math.Pow(((rhoB - rho) / rho), 2) + Math.Pow(((Vinv- Vc) / Vinv), 2) + Math.Pow(((ac_imp - Zc) / ac_imp), 2);
            return sqdiff;
        }
    }
}
