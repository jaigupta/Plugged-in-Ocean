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
    class ShaleVolume
    {
        public double Gr0, Gr100;
        WellLog grLog;
        public ShaleVolume(WellLog LogType)
        {
            this.grLog = LogType;
        }

        public void FromGamma()
        {
            int count;
            count = grLog.SampleCount;
            List<WellLogSample> LogGr = new List<WellLogSample>(grLog.Samples);
            double[] vsh = new double[count];
            double[] Gr = new double[count]; 
            for (int i = 0; i < count; i++)
            {
                Gr[i] = LogGr[i].Value;
                vsh[i] = (LogGr[i].Value - Gr0) / (Gr100 - Gr0);
            }
            double max=0, min=0;
            for (int k = 0; k < count; k++)
            {
                max = Math.Max(Gr[k],max);
                min = Math.Min(Gr[k], min);
            }
            
            Borehole b1 = grLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("VShale");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = LogGr[i2].MD;
                    float val = (float)vsh[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Shale Volume Fraction Log has been created in the same Well");
        }

        //From SP Log
        public double Sp0, Sp100;
        WellLog spLog;
        public ShaleVolume(WellLog LogType,int j)
        {
            this.spLog = LogType;
        }

        public void FromSP()
        {
            int count;
            count = spLog.SampleCount;
            List<WellLogSample> LogSp = new List<WellLogSample>(spLog.Samples);
            double[] vsh = new double[count];
            double[] Sp = new double[count];
            for (int i = 0; i < count; i++)
            {
                Sp[i] = LogSp[i].Value;
                vsh[i] = (LogSp[i].Value - Sp0) / (Sp100 - Sp0);
            }
            Borehole b1 = spLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("VShale");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);

                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = LogSp[i2].MD;
                    float val = (float)vsh[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Shale Volume Fraction Log has been created in the same Well");
        }
    }
}
