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
    class Impedance
    {
        public double GCoef, GExp;
        WellLog dtLog;

        public Impedance(WellLog LogType)
        {
            this.dtLog = LogType;
        }
        public void FromSonic()
         {
             int count = dtLog.SampleCount;

             List<WellLogSample> LogDt = new List<WellLogSample>(dtLog.Samples);

             double[] Imp = new double[count];

             for (int i = 0; i < count; i++)
             {
                 Imp[i] = 1000.0d*GCoef * Math.Pow(1 / dtLog[i].Value, GExp) / dtLog[i].Value;
             }
             Borehole b1 = dtLog.Borehole;
             using (ITransaction trans = DataManager.NewTransaction())
             {
                 IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                 ILogTemplate glob = pvs.FindTemplateByMnemonics("Acoustic Impedance");
                 PropertyVersion pv = pvs.FindOrCreate(glob);
                 trans.Lock(b1);
                 WellLog log = b1.Logs.CreateWellLog(pv);
                 //log.Name = "rhoB";
                 WellLogSample[] tsamples = new WellLogSample[count];
                 for (int i2 = 0; i2 < count; i2++)
                 {
                     double md = LogDt[i2].MD;
                     float val = (float)Imp[i2];
                     tsamples[i2] = new WellLogSample(md, val);
                 }
                 log.Samples = tsamples;
                 trans.Commit();
             }
             PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
         }

        public double vel, rhob;
        WellLog velLog, densLog;

        public Impedance(WellLog LogType1, WellLog LogType2)
        {
            this.velLog = LogType1;
            this.densLog = LogType2;
        }

        public void FromDensity()
        {
            //int count = Math.Min(velLog.SampleCount, densLog.SampleCount);

            List<WellLogSample> LogVel = new List<WellLogSample>(velLog.Samples);
            List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);

            TwoLogCheck c2 = new TwoLogCheck(velLog, densLog);
            int count = c2.count;
            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            double startMD = c2.startMD, endMD = c2.endMD;
            int startVel = c2.startLog1, startDens = c2.startLog2;

            double[] Imp = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDens[i + startDens].Value.ToString() == "NaN")
                    continue;
                
                if (LogVel[i + startVel].Value.ToString() == "NaN")
                    continue;

                vel = LogVel[i + startVel].Value;
                rhob = LogDens[i + startDens].Value;
                Imp[i] = vel * rhob;
            }
            Borehole b1 = densLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Acoustic Impedance");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                
                for (int i2 = 0; i2 < count; i2++)
                {

                    double md = startMD + i2 * c2.interval2;
                    float val = (float)Imp[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
        }

        

        //public double rhob;

        public Impedance(WellLog LogType, int k)
        {
            this.densLog = LogType;
        }

        public void FromDensity2()
        {
            int count = densLog.SampleCount;

            List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);
            Velocity V1 = new Velocity(densLog);
            V1.GCoef = GCoef;
            V1.GExp = GExp;
            V1.FromDensity();
            double[] Imp = new double[count];

            for (int i = 0; i < count; i++)
            {
                rhob = LogDens[i].Value;
                Imp[i] = V1.vel[i] * rhob;    
            }

            Borehole b1 = densLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Acoustic Impedance");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];

                for (int i2 = 0; i2 < count; i2++)
                {

                    double md = LogDens[i2].MD;
                    float val = (float)Imp[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
        }
    }
}
