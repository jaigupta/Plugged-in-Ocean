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
    class TwoLogCheck
    {
        public double startMD, endMD, interval1, interval2;
        public int count=0;
        public int startLog1, endLog1, startLog2, endLog2;
        //WellLog WellLog1, WellLog2;

        public TwoLogCheck(WellLog LogType1, WellLog LogType2)
        {
            List<WellLogSample> Log1 = new List<WellLogSample>(LogType1.Samples);
            List<WellLogSample> Log2 = new List<WellLogSample>(LogType2.Samples);
            //Console.WriteLine(Log1.Count);
            //Console.WriteLine(Log2.Count);

            interval1 = Log1[1].MD - Log1[0].MD;
            interval2 = Log2[1].MD - Log2[0].MD;
            if (Math.Abs(interval1 - interval2) < 10e-4d)
            {
                if ( Math.Abs(Log1[0].MD - Log2[0].MD)>10e-4 || Math.Abs(Log1[Log1.Count - 1].MD - Log2[Log2.Count - 1].MD)>10e-4)
                {
                    if (Log1[0].MD > Log2[0].MD)
                        startMD = Log1[0].MD;
                    else
                        startMD = Log2[0].MD;
                    if (Log1[Log1.Count - 1].MD > Log2[Log2.Count - 1].MD)
                        endMD = Log2[Log2.Count - 1].MD;
                    else
                        endMD = Log1[Log1.Count].MD;
                    for (int xxx = 0; xxx < Log1.Count; xxx++)
                    {
                        if (Math.Abs(Log1[xxx].MD - startMD)<10e-4)
                            startLog1 = xxx;
                        if (Math.Abs(Log1[xxx].MD - endMD)<10e-4)
                            endLog1 = xxx;
                    }
                    for (int xxx = 0; xxx < Log2.Count; xxx++)
                    {
                        if (Math.Abs(Log2[xxx].MD - startMD)<10e-4)
                            startLog2 = xxx;
                        if (Math.Abs(Log2[xxx].MD - endMD)<10e-4)
                            endLog2 = xxx;
                    }
                    count = endLog1 - startLog1 + 1;
                }
                else
                {
                    count = Log1.Count;
                    startMD = Log1[0].MD;
                    endMD = Log1[Log1.Count - 1].MD;
                }
            }
        }

    }
}
