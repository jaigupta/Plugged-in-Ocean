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
    class ElasticConstants
    {
        public double vsh, vratio, dens, dt;
        WellLog vshLog;
        public ElasticConstants(WellLog LogType)
        {
            this.vshLog = LogType;

        }
        public double[] poisn;
        
        public void FromVsh()
        {
            int count = vshLog.SampleCount;
            List<WellLogSample> LogVsh = new List<WellLogSample>(vshLog.Samples);

            poisn = new double[count];

            for (int i = 0; i < count; i++)
            {
                vsh = LogVsh[i].Value;
                poisn[i] = 0.125 * vsh * 100.0d + 0.27;
            }
            double interval = LogVsh[1].MD - LogVsh[0].MD;
            save(vshLog, vshLog.SampleCount, LogVsh[0].MD, interval, poisn, "Poissons ratio");
            PetrelLogger.InfoBox("The Poisson Ratio Log has been created in the same Well");
        }
        //List<Borehole> b2 = new List<Borehole>();
        //public Borehole b1;
        
        public void save(WellLog LogType, int count, double startMD, double interval, Double[] array, string name)
        {
            //Borehole b1;
            //int count = LogType.SampleCount;
            
            List<WellLogSample> TypeLog = new List<WellLogSample>(LogType.Samples);
            Borehole b1 = LogType.Borehole;
            //List<Borehole> b2 = new List<Borehole>();
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics(name);
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = startMD + i2*interval;
                    float val = (float)array[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }

        }

        public double c, d, pois;
        public double[] shearmod, bulkmod;
        WellLog densLog, dtLog, poisLog;

        public ElasticConstants(WellLog LogType1, WellLog LogType2, WellLog LogType3)
        {
            this.densLog = LogType1;
            this.dtLog = LogType2;
            this.poisLog = LogType3;
        }

        public void FromDensDt1()
        {
            //int count = Math.Min(densLog.SampleCount, dtLog.SampleCount);
            //count = Math.Min(count, poisLog.SampleCount);
            List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);
            List<WellLogSample> LogDt = new List<WellLogSample>(dtLog.Samples);
            List<WellLogSample> LogPois = new List<WellLogSample>(poisLog.Samples);

            ThreeLogCheck C1 = new ThreeLogCheck(densLog, dtLog, poisLog);
            int count = C1.count;
            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startDens = C1.startLog1, startDt = C1.startLog2, startPois = C1.startLog3;
            shearmod = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDens[i + startDens].Value.ToString() == "NaN")
                    continue;

                if (LogDt[i + startDt].Value.ToString() == "NaN")
                    continue;

                if (LogPois[i + startPois].Value.ToString() == "NaN")
                    continue;

                pois = LogPois[i + startPois].Value;
                c = 0.5 * (1 - 2 * pois) / (1 - pois);
                dens = LogDens[i + startDens].Value;
                dt = LogDt[i + startDt].Value;
                shearmod[i] = c * dens * 1.34 * Math.Pow(10, 10) / (Math.Pow(dt, 2));

            }

            save(densLog, C1.count, C1.startMD, C1.interval1, shearmod, "Shear modulus");
            PetrelLogger.InfoBox("The Shear Modulus Log has been created in the same Well");

        }

        public void FromDensDt2()
        {
            //int count = Math.Min(densLog.SampleCount, dtLog.SampleCount);
            //count = Math.Min(count, poisLog.SampleCount);
            List<WellLogSample> LogDens = new List<WellLogSample>(densLog.Samples);
            List<WellLogSample> LogDt = new List<WellLogSample>(dtLog.Samples);
            List<WellLogSample> LogPois = new List<WellLogSample>(poisLog.Samples);

            ThreeLogCheck C2 = new ThreeLogCheck(densLog, dtLog, poisLog);
            int count = C2.count;
            if (count == 0)
            {
                PetrelLogger.ErrorBox("INPUT LOGS DO NOT HAVE SAME DEPTH SPACING");
                return;
            }
            int startDens = C2.startLog1, startDt = C2.startLog2, startPois = C2.startLog3;
            bulkmod = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (LogDens[i + startDens].Value.ToString() == "NaN")
                    continue;

                if (LogDt[i + startDt].Value.ToString() == "NaN")
                    continue;

                if (LogPois[i + startPois].Value.ToString() == "NaN")
                    continue;
                pois = LogPois[i + startPois].Value;
                d = 0.33 * (1 + pois) / (1 - pois);
                dens = LogDens[i + startDens].Value;
                dt = LogDt[i + startDt].Value;
                bulkmod[i] = d * dens * 1.34 * Math.Pow(10, 10) / (Math.Pow(dt, 1));

            }

            save(densLog, C2.count, C2.startMD, C2.interval1, bulkmod, "Bulk modulus");
            PetrelLogger.InfoBox("The Bulk Modulus Log has been created in the same Well");
        }
        WellLog vpvsLog;
        public double vpvs;

        public ElasticConstants(WellLog LogType1, int j)
        {
            this.vpvsLog = LogType1;
        }
        
        public void FromVpvs()
        {
            int count = vpvsLog.SampleCount;
            List<WellLogSample> LogVpvs = new List<WellLogSample>(vpvsLog.Samples);
             poisn = new double[count];
            for (int i = 0; i < count; i++)
            {
                vpvs = LogVpvs[i].Value;
                poisn[i] = 0.5d * (Math.Pow(vpvs,2)  - 2.0d) / (Math.Pow(vpvs,2) - 1.0d);
            }
            save(vpvsLog, vpvsLog.SampleCount, LogVpvs[0].MD, LogVpvs[1].MD - LogVpvs[0].MD, poisn, "Poissons ratio");
            PetrelLogger.InfoBox("The Poisson Ratio Log has been created in the same Well");
        }
    }
}
