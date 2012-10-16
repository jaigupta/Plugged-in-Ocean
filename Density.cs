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
    class Density
    {
        //public double[] rhob;
        public double phie, sw, rhof, rhow, rhoh, rhom, rhosh, rhomat, vsh;
        WellLog porLog, swLog, vshLog;
        public Density(WellLog LogType1, WellLog LogType2, WellLog LogType3)
        {
            this.porLog = LogType1;
            this.swLog = LogType2;
            this.vshLog = LogType3;
        }
            //double[] LogPhie, LogSw, LogVsh;
            //WellLog LogType4;

        public void fromporosity()
        {
            //int count;
            //count = porLog.SampleCount;
            List<WellLogSample> LogPhie = new List<WellLogSample>(porLog.Samples);
            List<WellLogSample> LogSw = new List<WellLogSample>(swLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);
            //List<WellLogSample> LogRhob = new List<WellLogSample>(LogType4.Samples);

            ThreeLogCheck C1 = new ThreeLogCheck(porLog, swLog, vshLog);
            int count = C1.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startPor = C1.startLog1, startSw = C1.startLog2, startVsh = C1.startLog3;

            double[] rhob = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogPhie[i + startPor].Value.ToString() == "NaN")
                    continue;

                if (LogSw[i + startSw].Value.ToString() == "NaN")
                    continue;

                if (LogVsh[i + startVsh].Value.ToString() == "NaN")
                    continue;

                phie = (double)LogPhie[i + startPor].Value;
                sw = (double)LogSw[i + startSw].Value;
                vsh = (double)LogVsh[i + startVsh].Value;
                rhof = sw * rhow + (1 - sw) * rhoh;
                rhom = vsh * rhosh + (1 - vsh - phie) * rhomat;
                rhob[i] = phie * rhof + rhom;
                //PetrelLogger.InfoOutputWindow(rhob.ToString());
                //LogRhob[count].Value = (float)rhob;

            }
            Borehole b1 = porLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Density");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C1.startMD + C1.interval1 * i2;
                    float val = (float)rhob[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Density Log has been created in the same Well");


         }

        public double gcoef, gexp, vel;
        WellLog velLog;

        public Density(WellLog LogType)
        {
            this.velLog = LogType;
        }


        public void fromvelocity()
        {
            int count;
            List<WellLogSample> LogVel = new List<WellLogSample>(velLog.Samples);
            count = velLog.SampleCount;

            double[] rhob=new double[count];

            for (int i = 0; i < count; i++)
            {
                vel = (double)LogVel[i].Value;
                rhob[i] = 1000.0d*gcoef * Math.Pow(vel, gexp);
            }
            Borehole b1 = velLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Density");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = LogVel[i2].MD;
                    float val = (float)rhob[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Density Log has been created in the same Well");
        }

        WellLog ImpLog, VelLog;

        public Density(WellLog LogType1, WellLog LogType2)
        {
            this.ImpLog = LogType1;
            this.VelLog = LogType2;
        }

        public void fromimpedance()
        {
            //int count;
            //count = ImpLog.SampleCount;
            List<WellLogSample> LogImp = new List<WellLogSample>(ImpLog.Samples);
            List<WellLogSample> LogVel = new List<WellLogSample>(VelLog.Samples);

            TwoLogCheck C2 = new TwoLogCheck(ImpLog, VelLog);
            int count = C2.count;
            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startImp = C2.startLog1, startVel = C2.startLog2;
            double[] rhob = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogImp[i + startImp].Value.ToString() == "NaN")
                    continue;

                if (LogVel[i + startVel].Value.ToString() == "NaN")
                    continue;
                rhob[i] = (double)LogImp[i + startImp].Value / LogVel[i + startVel].Value;
                
            }
            Borehole b1 = ImpLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Density");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C2.startMD + C2.interval1 * i2;
                    float val = (float)rhob[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Density Log has been created in the same Well");
        }

    }
}
