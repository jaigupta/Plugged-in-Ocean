using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;

using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Seismic;


using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Basics;
using Slb.Ocean.Data;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Geometry;

namespace LogAnalysis
{
    class Porosity
    {
        public double dens, sw, rhof, rhow, rhoh, rhosh, rhomat, vsh;
        WellLog densLog, swLog, vshLog;
        public Porosity(WellLog LogType1, WellLog LogType2, WellLog LogType3)
        {
            this.densLog = LogType1;
            this.swLog = LogType2;
            this.vshLog = LogType3;
        }


        public void FromDensity()
        {
            //int count;
            //count = Math.Min(densLog.SampleCount, vshLog.SampleCount);
            //count = Math.Min(count, swLog.SampleCount);

            List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);
            List<WellLogSample> LogSw = new List<WellLogSample>(swLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);
            
            ThreeLogCheck C1 = new ThreeLogCheck(densLog, swLog, vshLog);
            int count = C1.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startDens = C1.startLog1, startSw = C1.startLog2, startVsh = C1.startLog3;
            double[] phie = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDens[i + startDens].Value.ToString() == "NaN")
                    continue;

                if (LogSw[i + startSw].Value.ToString() == "NaN")
                    continue;

                if (LogVsh[i + startVsh].Value.ToString() == "NaN")
                    continue;

                dens = (double)LogDens[i + startDens].Value;
                sw = (double)LogSw[i + startSw].Value;
                vsh = (double)LogVsh[i + startVsh].Value;
                rhof = sw * rhow + (1 - sw) * rhoh;
                phie[i] = (dens - (1 - vsh) * rhomat - vsh * rhosh) / (rhof - rhomat - vsh * rhosh);


            }

            Borehole b1 = densLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Porosity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C1.startMD + i2 * C1.interval1;
                    float val = (float)phie[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");
        }

        public double dt, dtf, dtw, dth, dtsh, dtmat;
        WellLog dtLog;

        //Wyllies Method

        public Porosity(WellLog LogType1, WellLog LogType2, WellLog LogType3, int j)
        {
            this.dtLog = LogType1;
            this.swLog = LogType2;
            this.vshLog = LogType3;
        }
            //double[] LogPhie, LogSw, LogVsh;
            //WellLog LogType4;

        public void FromSonic()
        {
            //int count;
            //count = Math.Min(dtLog.SampleCount, vshLog.SampleCount);
            //count = Math.Min(count, swLog.SampleCount);

            List<WellLogSample> LogDt = new List<WellLogSample>(dtLog.Samples);
            List<WellLogSample> LogSw = new List<WellLogSample>(swLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);
            ThreeLogCheck C2 = new ThreeLogCheck(dtLog, swLog, vshLog);
            int count = C2.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startDt = C2.startLog1, startSw = C2.startLog2, startVsh = C2.startLog3;
            double[] phie = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDt[i + startDt].Value.ToString() == "NaN")
                    continue;

                if (LogSw[i + startSw].Value.ToString() == "NaN")
                    continue;

                if (LogVsh[i + startVsh].Value.ToString() == "NaN")
                    continue;

                dt = (double)LogDt[i + startDt].Value;
                sw = (double)LogSw[i + startSw].Value;
                vsh = (double)LogVsh[i + startVsh].Value;
                
                dtf = sw * dtw + (1 - sw) * dth;
                phie[i] = (dt - (1 - vsh) * dtmat - vsh * dtsh) / (dtf - dtmat - vsh * dtsh);


            }

            Borehole b1 = dtLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Porosity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C2.startMD + i2 * C2.interval1;
                    float val = (float)phie[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");
        }

        //Hunt-Raymer model

        public double c, dtc;

        public Porosity(WellLog LogType1, WellLog LogType2)
        {
            this.dtLog = LogType1;
            this.vshLog = LogType2;
        }


        public void FromSonic2()
        {
            //int count;
            //count = Math.Min(dtLog.SampleCount, vshLog.SampleCount);
            
            List<WellLogSample> LogDt = new List<WellLogSample>(dtLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);

            TwoLogCheck C3 = new TwoLogCheck(dtLog, vshLog);
            int count = C3.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startDt = C3.startLog1, startVsh = C3.startLog2;
            double[] phie = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDt[i + startDt].Value.ToString() == "NaN")
                    continue;

                if (LogVsh[i + startVsh].Value.ToString() == "NaN")
                    continue;

                dt = (double)LogDt[i + startDt].Value;
                vsh = (double)LogVsh[i + startVsh].Value;
                
                dtc = dt - vsh * (dtsh - dtmat);
                c = dtmat / (2 * dtw);
                phie[i] = 1 - c - Math.Sqrt(Math.Pow(c, 2) - dtmat / dtw + dtmat / dtc);

            }

            Borehole b1 = dtLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Porosity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C3.startMD + i2 * C3.interval1;
                    float val = (float)phie[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");
 
        }

        //From Gamma ray density log

        public double Gnsh, Gn;

        WellLog GnLog;

        public Porosity(WellLog LogType1, WellLog LogType2, int j)
        {
            this.GnLog = LogType1;
            this.vshLog = LogType2;

        }

        public void FromNeutron()
        {
            //int count;
            //count = GnLog.SampleCount;

            //count=Math.Min(GnLog.SampleCount, vshLog.SampleCount);

            List<WellLogSample> LogGn = new List<WellLogSample>(GnLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);

            TwoLogCheck C4 = new TwoLogCheck(GnLog, vshLog);
            int count = C4.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startGn = C4.startLog1, startVsh = C4.startLog2;
            double[] phie = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogGn[i + startGn].Value.ToString() == "NaN")
                    continue;

                if (LogVsh[i + startVsh].Value.ToString() == "NaN")
                    continue;

                Gn = (double)LogGn[i + startGn].Value;
                vsh = (double)LogVsh[i + startVsh].Value;
                phie[i] = Gn - vsh * Gnsh * 100;
            }

            Borehole b1 = vshLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Porosity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C4.startMD + i2 * C4.interval1;
                    float val = (float)phie[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");

        }

    }
}