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
using System.Threading;

namespace WellReader
{
    class RACO
    {
        WellLog density, sonic;
        public Int64 Kc;
        public Int64 Kq;
        public Int64 Gc;
        public Int64 Gq;
        public Int64 Kw;
        public Int64 Ko;
        public Int64 Gh;
        public int rhoC;
        public int rhoQ;
        public int rhoW;
        public int rhoO;
        public Borehole b1;
        public String porLog;
        public String clayLog;
        public String waterLog;
        public int globalcount;
        public int RPMndex;
        public double minPor;
        public double maxPor;
        public double minWater;
        public double maxWater;
        public double minClay;
        public double maxClay;
        public int maincomboIndex;
        List<Borehole> b2 = new List<Borehole>();
        
        public RACO(WellLog density, WellLog sonic)
        {
            this.density = density;
            this.sonic = sonic;
        }
        public void workingFunction()
        {
            List<WellLogSample> den = new List<WellLogSample>(density.Samples);
            List<WellLogSample> dt = new List<WellLogSample>(sonic.Samples);
            if (den.Count == 0||dt.Count==0)
            {
                PetrelLogger.ErrorBox("NULL LOG found");
                return;
            }
            Console.WriteLine(den.Count);
            Console.WriteLine(dt.Count);

            int i;
            double startMD, endMD;
            int startden = 0,
                endden = 0,
                startdt = 0,
                enddt = 0;

            int count = 0;
            double interval1 = den[1].MD - den[0].MD;
            double interval2 = dt[1].MD - dt[0].MD;
            if (Math.Abs(interval1 - interval2) > 10e-4)
            {
                PetrelLogger.ErrorBox("DEPTH INTERVAL MISMATCH");
                return;
            }
            double interval = interval1;
                if (!(Math.Abs(den[0].MD - dt[0].MD) < 10e-4 && Math.Abs(den[den.Count - 1].MD - dt[dt.Count - 1].MD) < 10e-4))
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
                        if (Math.Abs(den[xxx].MD - startMD) < 10e-4)
                            startden = xxx;
                        if (Math.Abs(den[xxx].MD - endMD) < 10e-4)
                            endden = xxx;
                    }
                    for (int xxx = 0; xxx < dt.Count; xxx++)
                    {
                        if (Math.Abs(dt[xxx].MD - startMD) < 10e-4)
                            startdt = xxx;
                        if (Math.Abs(dt[xxx].MD - endMD) < 10e-4)
                            enddt = xxx;
                    }
                    count = endden - startden + 1;
                }
                else
                {
                    count = den.Count;
                    startMD = den[0].MD;
                    endMD = den[den.Count - 1].MD;
                }
            

            float[] C2 = new float[count];
            float[] S2 = new float[count];
            float[] phi2 = new float[count];
            float[] error1 = new float[count];
        

            using (IProgress i1 = PetrelLogger.NewProgress(1, count))
            {
                for (i = 0; i < count; i++)
                {

                    float ac_imp;
                    if (den[i + startden].Value.ToString() == "NaN")
                    {
                        continue;
                    }
                    if (dt[i + startdt].Value.ToString() == "NaN")
                        continue;
                    float rho = den[i + startden].Value;
                    float son = dt[i + startdt].Value;
                    float Vinv = 1.0f / son;
                    ac_imp = rho * Vinv;
                    error1[i] = 1.0e35f;
                    for (float phi = (float)minPor; phi <= (float)(maxPor+0.1); phi += 0.1f)
                    {
                        for (float S = (float)minWater; S <= (float)(maxWater + 0.1); S += 0.1f)
                        {
                            for (float C = (float)minClay; C <= (float)(maxClay + 0.1); C += 0.1f)
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
                    updateVals(ac_imp, rho, Vinv, ref C2[i], ref S2[i], ref phi2[i]);
                    i1.ProgressStatus = i + 1;
                }
            }
            b1 = density.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Porosity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = startMD + i2 * interval;
                    float val = phi2[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics(clayLog);
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = startMD + i2 * interval;
                    float val = C2[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics(waterLog);
         
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = startMD + i2 * interval;
                    float val = S2[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
        }
        

        public double dfunc(float ac_imp, float rho, float Vinv, float C, float S, float phi)
        {
            double sqdiff;
            double rhoF = (1 - S) * rhoO + S * rhoW;
            double rhoM = (1 - C) * rhoQ + C * rhoC;
            double rhoB = (1 - phi) * rhoM + phi * rhoF;

            double Km;
            Km = 0.5 * (C * Kc + (1 - C) * Kq + 1 / (C / Kc + (1 - C) / Kq));
            double Gm;
            Gm = 0.5 * (C * Gc + (1 - C) * Gq + 1 / (C / Gc + (1 - C) / Gc));
            double Kf;
            Kf = 0.5 * (S * Kw + (1 - S) * Ko + 1 / (S / Kw + (1 - S) / Ko));
            double Vm, Vf, Vc, Zc;
            Vm = Math.Sqrt((Km + (4*Gm/3)) / rhoM);
            Vf = Math.Sqrt(Kf / rhoF);

            //Raymer et al model for velocity estimation
            if (RPMndex == 0)
            {
                Vc = Math.Pow((1 - phi), 2) * Vm + phi * Vf;
            }
            else
            {
                double Vw = Math.Sqrt(Kw / rhoW);
                double Vo;
                if(maincomboIndex==1)
                    Vo = Math.Sqrt(Ko+(4*Gh/3) / rhoO);
                else
                    Vo = Math.Sqrt(Ko / rhoO);
                Vc = 1 / ((1 - phi) / Vm + phi * S / Vw + (1 - S) * phi / Vo);
            }
            Zc = Vc * rhoB;
            //double Kb, Gb, poissonc;
            //Kb = 0.5 * (phi * Kf + (1 - phi) * Km + 1 / (phi / Kf + (1 - phi) / Km));
            //Gb = Gm;
            //poissonc = (3 * Kb - 2 * Gb) / (2 * (3 * Kb + Gb));
            sqdiff = Math.Pow(((rhoB - rho)/rho ), 2) + Math.Pow(((Vinv - Vc)/Vinv ), 2) + Math.Pow(((ac_imp - Zc)/ac_imp ), 2);
            return sqdiff;
        }
        

        public void updateVals(float ac_imp, float rho, float Vinv, ref float C, ref float S, ref float phi)
        {
            double Y;
            double rhoF, rhoM, rhoB;
            double Km,Gm,Kf;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    rhoF = (1 - S) * rhoO + S * rhoW;
                    rhoM = (1 - C) * rhoQ + C * rhoC;
                    rhoB = (1 - phi) * rhoM + phi * rhoF;
                    Km = 0.5 * (C * Kc + (1 - C) * Kq + 1 / (C / Kc + (1 - C) / Kq));
                    Gm = 0.5 * (C * Gc + (1 - C) * Gq + 1 / (C / Gc + (1 - C) / Gc));
                    Kf = 0.5 * (S * Kw + (1 - S) * Ko + 1 / (S / Kw + (1 - S) / Ko));
                    double Vm, Vf, Vc, Zc,Vo=0, Vw=0;
                    Vm = Math.Sqrt((Km + (4*Gm/3)) / rhoM);
                    Vf = Math.Sqrt(Kf / rhoF);
                    if (RPMndex == 0)
                    {
                        Vc = Math.Pow((1 - phi), 2) * Vm + phi * Vf;
                    }
                    else
                    {
                        Vw = Math.Sqrt(Kw / rhoW);
                        if(maincomboIndex==1)
                            Vo = Math.Sqrt(Ko+(4*Gh/3) / rhoO);
                        else
                            Vo = Math.Sqrt(Ko / rhoO);
                        Vc = 1 / ((1 - phi) / Vm + phi * S / Vw + (1 - S) * phi / Vo);
                    }
                    Zc = Vc * rhoB;
                    double dVf_BY_dS=0;
                    double d2Vf_BY_dS2=0;
                    double drhoF_BY_dS = rhoW - rhoO;
                    double drhoM_BY_dC = rhoC - rhoQ;
                    double drhoB_BY_dphi = rhoF - rhoM;
                    double drhoB_BY_dC = (1 - phi) * drhoM_BY_dC;
                    double drhoB_BY_dS = phi * drhoF_BY_dS;

                    double dKm_BY_dC = (Kc - Kq) * (1.0 + (Kc * Kq) / Math.Pow(C * Kq + (1 - C) * Kc, 2)) / 2.0;
                    double dGm_BY_dC = (Gc - Gq) * (1.0 + (Gc * Gq) / Math.Pow(C * Gq + (1 - C) * Gc, 2)) / 2.0;
                    double dKf_BY_dS = (Kw - Ko) * (1.0 + (Kw * Ko) / Math.Pow(S * Ko + (1 - S) * Kw, 2)) / 2.0;

                    double dVm_BY_dC = ((dKm_BY_dC + (4*dGm_BY_dC/3)) * rhoM - (Km + (4*Gm/3)) * drhoM_BY_dC) / (2 * rhoM * Math.Sqrt(rhoM * (Km + Gm)));

                    double d2Vm_BY_dC2 = -0.25 * Math.Pow(rhoM, -0.5) * Math.Pow(Km + (4*Gm/3), -1.5) * Math.Pow(dKm_BY_dC + (4*dGm_BY_dC/3), 2) +
                                         -0.5 * Math.Pow(rhoM, -1.5) * Math.Pow(Km + (4 * Gm / 3), -0.5) * drhoM_BY_dC * (dKm_BY_dC + (4*dGm_BY_dC/3)) +
                                         0.75 * Math.Pow(Km + (4 * Gm / 3), 0.5) * Math.Pow(rhoM, -2.5) * Math.Pow(drhoM_BY_dC, 2);
                    if (RPMndex == 0)
                    {
                        dVf_BY_dS = (rhoF * dKf_BY_dS - Kf * drhoF_BY_dS) / (2 * rhoF * Math.Sqrt(rhoF * Kf));
                        d2Vf_BY_dS2 = -0.25 * Math.Pow(rhoF, -0.5) * Math.Pow(Kf, -1.5) * Math.Pow(dKf_BY_dS, 2) +
                                             -0.5 * Math.Pow(rhoF, -1.5) * Math.Pow(Kf, -0.5) * drhoB_BY_dS * dKf_BY_dS +
                                             0.75 * Math.Pow(Kf, 0.5) * Math.Pow(rhoF, -2.5) * Math.Pow(drhoB_BY_dS, 2);
                    }

                    double dVc_BY_dphi;
                    double d2Vc_BY_dphi2;
                    double dVc_BY_dC;
                    double d2Vc_BY_dC2;
                    double dVc_BY_dS;
                    double d2Vc_BY_dS2;
                    //Raymer et al model
                    if (RPMndex == 0)
                    {
                        dVc_BY_dphi = Vf + 2 * (phi - 1) * Vm;
                        d2Vc_BY_dphi2 = 2 * Vm;

                        dVc_BY_dC = (1 - phi) * (1 - phi) * dVm_BY_dC;
                        d2Vc_BY_dC2 = (1 - phi) * (1 - phi) * d2Vm_BY_dC2;

                        dVc_BY_dS = phi * dVf_BY_dS;
                        d2Vc_BY_dS2 = phi * d2Vf_BY_dS2;


                    }
                    else
                    {
                        dVc_BY_dphi = (1 / Vm - (S / Vw + (1 - S) / Vo)) * Vc * Vc;
                        d2Vc_BY_dphi2 = 2 * Math.Pow(dVc_BY_dphi, 2) / Vc;

                        dVc_BY_dC = (1 - phi) * dVm_BY_dC * Math.Pow(Vc / Vm, 2);
                        d2Vc_BY_dC2 = ((1 - phi) * (2 * Math.Pow(dVm_BY_dC, 2) / Math.Pow(Vm, 3) - (1 / (Vm * Vm) * d2Vm_BY_dC2)) -
                                       2 * Math.Pow(dVc_BY_dC, 2) / Math.Pow(Vc, 3)) * Vc * -Vc;

                        dVc_BY_dS = phi * Vc * Vc * (1 / Vo - 1 / Vw);
                        d2Vc_BY_dS2 = 2 * Math.Pow(dVc_BY_dS, 2) / Vc;
                    }
                    double dZc_BY_dphi = rhoB * dVc_BY_dphi + Vc * drhoB_BY_dphi;
                    double d2Zc_BY_dphi2 = rhoB * d2Vc_BY_dphi2 + 2 * dVc_BY_dphi * drhoB_BY_dphi;

                    double dZc_BY_dC = rhoB * dVc_BY_dC + Vc * drhoB_BY_dC;
                    double d2Zc_BY_dC2 = rhoB * d2Vc_BY_dC2 + 2 * drhoB_BY_dC * dVc_BY_dC;

                    double dZc_BY_dS = rhoB * dVc_BY_dS + Vc * drhoB_BY_dS;
                    double d2Zc_BY_dS2 = 2 * dVc_BY_dS * drhoB_BY_dS +
                                         rhoB * d2Vc_BY_dS2;

                    double dY_BY_dphi = 2.0 * (rhoB - rho) * drhoB_BY_dphi / (rho * rho) +
                                        2.0 * (Vc - Vinv) * dVc_BY_dphi / (Vinv * Vinv) +
                                        2.0 * (Zc - ac_imp) * dZc_BY_dphi / (ac_imp * ac_imp);

                    double d2Y_BY_dphi2 = 2.0 * (0.0/*(rhoB - rho) * d2rhoB_BY_phi2(=0)*/ + drhoB_BY_dphi * drhoB_BY_dphi) / (rho * rho) +
                                        2.0 * ((Vc - Vinv) * d2Vc_BY_dphi2 + dVc_BY_dphi * dVc_BY_dphi) / (Vinv * Vinv) +
                                        2.0 * ((Zc - ac_imp) * d2Zc_BY_dphi2 + dZc_BY_dphi * dZc_BY_dphi) / (ac_imp * ac_imp);

                    double dY_BY_dC = 2.0 * (rhoB - rho) * drhoB_BY_dC / (rho * rho) +
                                        2.0 * (Vc - Vinv) * dVc_BY_dC / (Vinv * Vinv) +
                                        2.0 * (Zc - ac_imp) * dZc_BY_dC / (ac_imp * ac_imp);

                    double d2Y_BY_dC2 = 2.0 * (0.0/*(rhoB - rho) * d2rhoB_BY_C2(=0)*/ + drhoB_BY_dC * drhoB_BY_dC) / (rho * rho) +
                                        2.0 * ((Vc - Vinv) * d2Vc_BY_dC2 + dVc_BY_dC * dVc_BY_dC) / (Vinv * Vinv) +
                                        2.0 * ((Zc - ac_imp) * d2Zc_BY_dC2 + dZc_BY_dC * dZc_BY_dC) / (ac_imp * ac_imp);

                    double dY_BY_dS = 2.0 * (rhoB - rho) * drhoB_BY_dS / (rho * rho) +
                                        2.0 * (Vc - Vinv) * dVc_BY_dS / (Vinv * Vinv) +
                                        2.0 * (Zc - ac_imp) * dZc_BY_dS / (ac_imp * ac_imp);

                    double d2Y_BY_dS2 = 2.0 * (0.0/*(rhoB - rho) * d2rhoB_BY_S2(=0)*/ + drhoB_BY_dS * drhoB_BY_dS) / (rho * rho) +
                                        2.0 * ((Vc - Vinv) * d2Vc_BY_dS2 + dVc_BY_dS * dVc_BY_dS) / (Vinv * Vinv) +
                                        2.0 * ((Zc - ac_imp) * d2Zc_BY_dS2 + dZc_BY_dS * dZc_BY_dS) / (ac_imp * ac_imp);

                    Y = Math.Pow((rhoB - rho) / rho, 2) + Math.Pow((Vinv - Vc) / Vc, 2) + Math.Pow((ac_imp - Zc) / ac_imp, 2);
                    if(j % 15 < 5)
                    {
                        S = (float)(S - dY_BY_dS/d2Y_BY_dS2);
                        if (S > 1)
                            S = 1.0f;
                        else if (S < 0.0)
                            S = 0.0f;
                    }
                    else if (j % 15 < 10)
                    {
                        C = (float)(C - dY_BY_dC / d2Y_BY_dC2);
                        if (C > 1)
                            C = 1.0f;
                        else if (C < 0)
                            C = 0.0f;
                    }
                    else
                    {
                        phi = (float)(phi - dY_BY_dphi / d2Y_BY_dphi2);
                        if (phi > 0.9)
                            phi = 0.9f;
                        else if (phi < 0)
                            phi = 0.0f;
                    }
                }
            }
        }
    }
}
