using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Geometry;
using System.Threading;

namespace WellReader
{
    class Reservoir
    {
        SeismicCube cubeDensity, cubeSonic;
        SeismicCollection scol;
        public Int64 Kc;
        public Int64 Kq;
        public Int64 Gc;
        public Int64 Gq;
        public Int64 Kw;
        public Int64 Ko;
        public int rhoC;
        public int rhoQ;
        public int rhoW;
        public int rhoO;
        public int RPMndex;
        public double minPor;
        public double maxPor;
        public double minWater;
        public double maxWater;
        public double minClay;
        public double maxClay;
        public int maincomboIndex;
        public String porCube;
        public String clayCube;
        public String waterCube;
        public int quality;
        
        public SeismicCube PHI_CUBE=SeismicCube.NullObject;
        public SeismicCube SW_CUBE=SeismicCube.NullObject;
        public SeismicCube VSHALE_CUBE=SeismicCube.NullObject;
        public Reservoir(SeismicCube cubeDensity, SeismicCube cubeSonic)
        {
            this.cubeDensity = cubeDensity;
            this.cubeSonic = cubeSonic;
            this.scol = cubeDensity.SeismicCollection;
        }

        public void workingFunction()
        {
            maincomboIndex = 0;
            SeismicRoot seismicRoot = SeismicRoot.Get(PetrelProject.PrimaryProject);
            SeismicProject proj = seismicRoot.SeismicProject;
            

            float[, ,] phi2 = new float[cubeDensity.NumSamplesIJK.I, cubeDensity.NumSamplesIJK.J, cubeDensity.NumSamplesIJK.K];
            float[, ,] S2 = new float[cubeDensity.NumSamplesIJK.I, cubeDensity.NumSamplesIJK.J, cubeDensity.NumSamplesIJK.K];
            float[, ,] C2 = new float[cubeDensity.NumSamplesIJK.I, cubeDensity.NumSamplesIJK.J, cubeDensity.NumSamplesIJK.K];
            float minphi2 = 1.0f;
            float maxphi2 = 0.0f;
            float minC2 = 1.0f;
            float maxC2 = 0.0f;
            float minS2 = 1.0f;
            float maxS2 = 0.0f;
            using (ITransaction txn = DataManager.NewTransaction())
            {
                try
                {

                    using (IProgress i1 = PetrelLogger.NewProgress(1, cubeDensity.NumSamplesIJK.J))
                    {
                        for (int p = 0; p < cubeDensity.NumSamplesIJK.I; p++)
                        {
                            for (int q = 0; q < cubeDensity.NumSamplesIJK.J; q++)
                            {
                                ITrace trace1 = cubeDensity.GetTrace(p, q);
                                ITrace trace2 = cubeSonic.GetTrace(p, q);
                                //ITrace tracePor = PHI_CUBE.GetTrace(p, q);
                                //ITrace traceSw = SW_CUBE.GetTrace(p, q);
                                //ITrace traceVSh = VSHALE_CUBE.GetTrace(p, q);
                                for (int k = 0; k < trace1.Length; k++)
                                {
                                    double sample1 = trace1[k];
                                    double sample2 = trace2[k];
                                    float rho = (float)sample1;
                                    float Vinv = (float)(1.0 / sample2);
                                    float ac_imp = rho * Vinv;
                                    float error1 = 100e30f;

                                    for (float phi = (float)minPor; phi <= (float)(maxPor + 0.1); phi += 0.1f)
                                    {
                                        for (float S = (float)minWater; S <= (float)(maxWater + 0.1); S += 0.1f)
                                        {
                                            for (float C = (float)minClay; C <= (float)(maxClay + 0.1); C += 0.1f)
                                            {
                                                double error = dfunc(ac_imp, rho, Vinv, C, S, phi);
                                                if (error1 > (float)error)
                                                {
                                                    C2[p, q, k] = C;
                                                    S2[p, q, k] = S;
                                                    phi2[p, q, k] = phi;
                                                    error1 = (float)error;

                                                }
                                            }
                                        }
                                    }
                                    updateVals(ac_imp, rho, Vinv, ref C2[p, q, k], ref S2[p, q, k], ref phi2[p, q, k]);
                                    if (phi2[p, q, k] < minphi2)
                                        minphi2 = phi2[p, q, k];
                                    if (phi2[p, q, k] > maxphi2)
                                        maxphi2 = phi2[p, q, k];
                                    if (C2[p, q, k] < minC2)
                                        minC2 = C2[p, q, k];
                                    if (C2[p, q, k] > maxC2)
                                        maxC2 = C2[p, q, k];
                                    if (S2[p, q, k] < minS2)
                                        minS2 = S2[p, q, k];
                                    if (S2[p, q, k] > maxS2)
                                        maxS2 = S2[p, q, k];

                                }


                            } 
                            i1.ProgressStatus = p + 1;

                        }
                    }
                    txn.Lock(proj);
                    txn.Lock(scol);
                    Index3 size = new Index3(cubeDensity.NumSamplesIJK.I, cubeDensity.NumSamplesIJK.J, cubeDensity.NumSamplesIJK.K);
                    IndexDouble3 tempindex = new IndexDouble3(0, 0, 0);
                    Point3 origin = cubeDensity.PositionAtIndex(tempindex);

                    double d1, d2, d3;

                    d1 = cubeDensity.PositionAtIndex(new IndexDouble3(1, 0, 0)).X - origin.X;
                    d2 = cubeDensity.PositionAtIndex(new IndexDouble3(1, 0, 0)).Y - origin.Y;
                    d3 = cubeDensity.PositionAtIndex(new IndexDouble3(1, 0, 0)).Z - origin.Z;
                    Vector3 iVec = new Vector3(d1, d2, d3);

                    d1 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 1, 0)).X - origin.X;
                    d2 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 1, 0)).Y - origin.Y;
                    d3 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 1, 0)).Z - origin.Z;
                    Vector3 jVec = new Vector3(d1, d2, d3);

                    d1 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 0, 1)).X - origin.X;
                    d2 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 0, 1)).Y - origin.Y;
                    d3 = cubeDensity.PositionAtIndex(new IndexDouble3(0, 0, 1)).Z - origin.Z;
                    Vector3 kVec = new Vector3(d1, d2, d3);


                    /*double inlineI = (cubeDensity.Lattice.Single.SpacingI) * Math.Sin(cubeDensity.Lattice.Single.RotationJ);
                    double inlineJ = (cubeDensity.Lattice.Single.SpacingI) * Math.Cos(cubeDensity.Lattice.Single.RotationJ);
                    double crosslineI = (cubeDensity.Lattice.Single.SpacingJ) * Math.Sin(cubeDensity.Lattice.Single.RotationJ);
                    double crosslineJ = (cubeDensity.Lattice.Single.SpacingJ) * -Math.Cos(cubeDensity.Lattice.Single.RotationJ);
                    Vector3 iSpacing = new Vector3(inlineJ, inlineI, 0.0);
                    Vector3 jSpacing = new Vector3(crosslineI, crosslineJ, 0.0);
                    Vector3 kSpacing = new Vector3(0.0, 0.0, 3.048);
                     */
                    if (scol.CanCreateSeismicCube(size, origin, iVec, jVec, kVec))
                    {
                        Type dataType = typeof(float);
                        Domain vDomain = cubeDensity.Domain;
                        IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                        ILogTemplate glob = pvs.FindTemplateByMnemonics("Seismic");
                        PropertyVersion pv = pvs.FindOrCreate(glob);
                        //PropertyVersion pv = cubeDensity.PropertyVersion;
                        Range1<double> r1 = new Range1<double>(minphi2, maxphi2);
                        Range1<double> r2 = new Range1<double>(minS2, maxS2);
                        Range1<double> r3 = new Range1<double>(minC2, maxC2);
                        PetrelLogger.InfoOutputWindow("OUTPUT TEMPLATE UNDER PROCESS");
                        try
                        {
                            PHI_CUBE = scol.CreateSeismicCube(size, origin, iVec, jVec, kVec, dataType, vDomain, pv, r1);
                            SW_CUBE = scol.CreateSeismicCube(size, origin, iVec, jVec, kVec, dataType, vDomain, pv, r2);
                            VSHALE_CUBE = scol.CreateSeismicCube(size, origin, iVec, jVec, kVec, dataType, vDomain, pv, r3);
                            
                        }
                        catch (System.InvalidOperationException e)
                        {
                            PetrelLogger.ErrorBox(e.Message);
                        }
                        catch (System.ArgumentNullException e)
                        {
                            PetrelLogger.InfoOutputWindow(e.Message);
                        }
                    }

                    PHI_CUBE.Name = porCube;
                    SW_CUBE.Name = waterCube;
                    VSHALE_CUBE.Name = clayCube;

                    if (PHI_CUBE.IsWritable)
                    {
                        using (ITransaction txn1 = DataManager.NewTransaction())
                        {
                            PetrelLogger.InfoOutputWindow("Writing Data in the Porosity cube");
                            Index3 start = new Index3(0, 0, 0);
                            Index3 end = new Index3(cubeDensity.NumSamplesIJK.I - 1, cubeDensity.NumSamplesIJK.J - 1, cubeDensity.NumSamplesIJK.K - 1);
                            ISubCube to = cubeDensity.GetSubCube(start, end);
                            to.CopyFrom(phi2);
                            txn1.Commit();
                        }
                    }
                    if (SW_CUBE.IsWritable)
                    {
                        using (ITransaction txn1 = DataManager.NewTransaction())
                        {
                            PetrelLogger.InfoOutputWindow("Writing Data in the Water Saturation cube");
                            Index3 start = new Index3(0, 0, 0);
                            Index3 end = new Index3(cubeDensity.NumSamplesIJK.I - 1, cubeDensity.NumSamplesIJK.J - 1, cubeDensity.NumSamplesIJK.K - 1);
                            ISubCube to = cubeDensity.GetSubCube(start, end);
                            to.CopyFrom(S2);
                            txn1.Commit();
                        }
                    }
                    if (VSHALE_CUBE.IsWritable)
                    {
                        using (ITransaction txn1 = DataManager.NewTransaction())
                        {
                            PetrelLogger.InfoOutputWindow("Writing Data in the Shale Volume cube");
                            Index3 start = new Index3(0, 0, 0);
                            Index3 end = new Index3(cubeDensity.NumSamplesIJK.I - 1, cubeDensity.NumSamplesIJK.J - 1, cubeDensity.NumSamplesIJK.K - 1);
                            ISubCube to = cubeDensity.GetSubCube(start, end);
                            to.CopyFrom(C2);
                            txn1.Commit();
                        }
                    }
                    txn.Commit();
                    PetrelLogger.InfoOutputWindow("OUTPUT CUBES' Construction completed");
                }

                catch (ArgumentNullException e)
                {
                    PetrelLogger.ErrorBox("Seismic cube name or propertyVersion can not be blank (null)" + e.Message);
                }

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
                if (maincomboIndex == 1)
                    Vo = 0;
                else
                    Vo = Math.Sqrt(Ko / rhoO);
                Vc = 1 / ((1 - phi) / Vm + phi * S / Vw + (1 - S) * phi / Vo);
            }
            Zc = Vc * rhoB;

            sqdiff = Math.Pow(((rhoB - rho)/rho ), 2) + Math.Pow(((Vinv - Vc)/Vinv ), 2) + Math.Pow(((ac_imp - Zc)/ac_imp ), 2);
            return sqdiff;
        }
        

        public void updateVals(float ac_imp, float rho, float Vinv, ref float C, ref float S, ref float phi)
        {
            double Y;
            double rhoF, rhoM, rhoB;
            double Km,Gm,Kf;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 3*quality; j++)
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
                        if (maincomboIndex == 1)
                            Vo = 0;
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
                    if(j %(3*quality) < quality)
                    {
                        S = (float)(S - dY_BY_dS/d2Y_BY_dS2);
                        if (S > 1)
                            S = 1.0f;
                        else if (S < 0.0)
                            S = 0.0f;
                    }
                    else if (j % (3 * quality) < 2*quality)
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
