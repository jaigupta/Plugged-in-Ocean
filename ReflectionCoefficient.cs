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
    class ReflectionCoefficient
    {
        WellLog AcImpLog;
        public ReflectionCoefficient(WellLog LogType)
        {
            this.AcImpLog = LogType;
 
        }
        
        public void FromAcImp()
        {
            int count;
            count = AcImpLog.SampleCount;
            List<WellLogSample> LogAcImp = new List<WellLogSample>(AcImpLog.Samples);

            double[] RC = new double[count];

            for (int i = 0; i < count; i++)
            {
                if(i!=count-1)
                RC[i] = (LogAcImp[i + 1].Value - LogAcImp[i].Value) / (LogAcImp[i + 1].Value + LogAcImp[i].Value);

            }
            Borehole b1 = AcImpLog.Borehole;
            using (ITransaction trans = DataManager.NewTransaction())
            {
                IPropertyVersionService pvs = PetrelSystem.PropertyVersionService;
                ILogTemplate glob = pvs.FindTemplateByMnemonics("Reflection coefficients");
                PropertyVersion pv = pvs.FindOrCreate(glob);
                trans.Lock(b1);
                WellLog log = b1.Logs.CreateWellLog(pv);
                //log.Name = "rhoB";
                WellLogSample[] tsamples = new WellLogSample[count];
                for (int i2 = 0; i2 < count; i2++)
                {
                    double md = LogAcImp[i2].MD;
                    float val = (float)RC[i2];
                    tsamples[i2] = new WellLogSample(md, val);
                }
                log.Samples = tsamples;
                trans.Commit();
            }
            PetrelLogger.InfoBox("The Reflection coefficient Log has been created in the same Well");
        }
    }


}
