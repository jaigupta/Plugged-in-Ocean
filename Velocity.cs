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
    class Velocity
    {
        public double dt, phie, vf, dtw, dth, dtsh, dtmat, vm, c;
        public double sw, vsh;
        WellLog porLog, swLog, vshLog;

        public Velocity(WellLog LogType1, WellLog LogType2, WellLog LogType3)
        {
            this.porLog = LogType1;
            this.swLog = LogType2;
            this.vshLog = LogType3;
        }

        public void FromPorosity2()
        {
            //int count;
            //count = Math.Min(porLog.SampleCount, vshLog.SampleCount);
            //count = Math.Min(count, swLog.SampleCount);

            List<WellLogSample> LogPhie = new List<WellLogSample>(porLog.Samples);
            List<WellLogSample> LogSw = new List<WellLogSample>(swLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);
            ThreeLogCheck C1 = new ThreeLogCheck(porLog, swLog, vshLog);
            int count = C1.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startPor = C1.startLog1, startSw = C1.startLog2, startVsh = C1.startLog3;

            double[] vel = new double[count];

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
                c = vsh / (1 - phie);
                vf = sw / dtw + (1 - sw) / dth;
                vm = c / dtsh + (1 - c) / dtmat;
                vel[i] = Math.Pow((1 - phie), 2) * vm + phie * vf;
                //vel[i] = 1 / dt;


            }

            Borehole b1 = porLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Velocity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C1.startMD + C1.interval1 * i2;
                    float val = (float)vel[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");
        }
         
        
        public Velocity(WellLog LogType1, WellLog LogType2, WellLog LogType3, int j)
        {
            this.porLog = LogType1;
            this.swLog = LogType2;
            this.vshLog = LogType3;
        }

        public void FromPorosity3()
        {
            //int count;
            //count = Math.Min(porLog.SampleCount, vshLog.SampleCount);
            //count = Math.Min(count, swLog.SampleCount);

            List<WellLogSample> LogPhie = new List<WellLogSample>(porLog.Samples);
            List<WellLogSample> LogSw = new List<WellLogSample>(swLog.Samples);
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);
            ThreeLogCheck C2 = new ThreeLogCheck(porLog, swLog, vshLog);
            int count = C2.count;

            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startPor = C2.startLog1, startSw = C2.startLog2, startVsh = C2.startLog3;

            double[] vel = new double[count];

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
                c = vsh / (1 - phie);
                vf = sw / dtw + (1 - sw) / dth;
                vm = c / dtsh + (1 - c) / dtmat;
                vel[i] = (1-phie) * vm + phie * vf;
                //vel[i] = 1 / dt;


            }

            Borehole b1 = porLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Velocity");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = C2.startMD + C2.interval1 * i2;
                    float val = (float)vel[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");
        }

        public double rhob, GCoef, GExp;
        WellLog densLog;

       public Velocity(WellLog LogType)
        {
            this.densLog = LogType;

        }
       int count;


       public double[] vel;
       public void FromDensity()
       {

           count = densLog.SampleCount;
           vel = new double[count];
           List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);

           for (int i = 0; i < count; i++)
           {
               rhob = LogDens[i].Value;
               vel[i] = Math.Pow(rhob / (GCoef*1000.0d), 1 / GExp);
           }
       }

       public void save()
       {
           List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);
           Borehole b1 = densLog.Borehole;
           using (ITransaction trans = DataManager.NewTransaction())
           {
               IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
               ILogTemplate glob = pvs.FindTemplateByMnemonics("Velocity");
               PropertyVersion pv = pvs.FindOrCreate(glob);
               trans.Lock(b1);
               WellLog log = b1.Logs.CreateWellLog(pv);
               //log.Name = "rhoB";
               WellLogSample[] tsamples = new WellLogSample[count];
               for (int i2 = 0; i2 < count; i2++)
               {
                   double md = LogDens[i2].MD;
                   float val = (float)vel[i2];
                   tsamples[i2] = new WellLogSample(md, val);
               }
               log.Samples = tsamples;
               trans.Commit();
           }
           PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");
       }
            
}
}

