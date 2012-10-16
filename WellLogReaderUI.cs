using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;

using System.Text;
using System.Linq;
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
using System.IO;
using System.Reflection;
namespace WellReader
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    public partial class WellLogReaderUI : UserControl
    {
        public List<WellLog> wlog = new List<WellLog>();
        Borehole BR;
        SeismicCube RHO, DT;
        public WellLog logX, logY, logZ;
        SeismicCollection scolT;
        SeismicCube visual_cube = null;
        public int wellcount = 0;
        private WellLogReader workstep;
        /// <summary>
        /// The argument package instance being edited by the UI.
        /// </summary>
        private WellLogReader.Arguments args;
        /// <summary>
        /// Contains the actual underlaying context.
        /// </summary>
        private WorkflowContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WellLogReaderUI"/> class.
        /// </summary>
        /// <param name="workstep">the workstep instance</param>
        /// <param name="args">the arguments</param>
        /// <param name="context">the underlying context in which this UI is being used</param>
        public WellLogReaderUI(WellLogReader workstep, WellLogReader.Arguments args, WorkflowContext context)
        {
            InitializeComponent();
            //this.
            this.mainCombo.SelectedIndex = 0;
            this.comboRPM.SelectedIndex = 1;
            this.comboRPM2.SelectedIndex = 1;
            this.workstep = workstep;
            this.args = args;
            this.context = context;
            WellRoot wroot = WellRoot.Get(PetrelProject.PrimaryProject);


            LogAnalysis.LogAnalysisUI analysisUserControl = new LogAnalysis.LogAnalysisUI();
            analysisUserControl.Show();
            convertorPage.Controls.Add(analysisUserControl);

            ChatServer.ServerForm chatServerForm = new ChatServer.ServerForm();
            foreach (Control c in chatServerForm.Controls)
            {
                c.Show();
                chatServerArea.Controls.Add(c);
            }


            // Now traverse the Boreholes
            String tempstr;
            String tempstr1;
            int tempctr = 0;
            int tempctr1 = 0;
            int tempctr2 = 0;
            int tempctr3 = 0;


            SeismicRoot sroot = SeismicRoot.Get(PetrelProject.PrimaryProject);
        }

        private void bulkQuartzTrack_Scroll_1(object sender, EventArgs e)
        {
            bulkQuartzText.Text = bulkQuartzTrack.Value.ToString();
        }

        private void bulkClayTrack_Scroll_1(object sender, EventArgs e)
        {

            bulkClayText.Text = bulkClayTrack.Value.ToString();
        }

        private void bulkWaterTrack_Scroll_1(object sender, EventArgs e)
        {
            bulkWaterText.Text = bulkWaterTrack.Value.ToString();
        }

        private void bulkOilTrack_Scroll_1(object sender, EventArgs e)
        {
            bulkOilText.Text = bulkOilTrack.Value.ToString();
        }

        private void shearQuartzTrack_Scroll_1(object sender, EventArgs e)
        {
            shearQuartzText.Text = shearQuartzTrack.Value.ToString();
        }

        private void densityClayTrack_Scroll_1(object sender, EventArgs e)
        {
            densityClayText.Text = densityClayTrack.Value.ToString();
        }
        private void densityQuartzTrack_Scroll_1(object sender, EventArgs e)
        {
            densityQuartzText.Text = densityQuartzTrack.Value.ToString();
        }

        private void densityWaterTrack_Scroll_1(object sender, EventArgs e)
        {
            densityWaterText.Text = densityWaterTrack.Value.ToString();
        }

        private void densityOilTrack_Scroll_1(object sender, EventArgs e)
        {
            densityOilText.Text = densityOilTrack.Value.ToString();
        }

        private void shearClayTrack_Scroll_1(object sender, EventArgs e)
        {
            shearClayText.Text = shearClayTrack.Value.ToString();
        }

        private void mainCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.mainCombo.SelectedIndex == 1)
            {
                this.label7.Text = "Bulk Modulus of Hydrate";
                this.bulkOilTrack.Minimum = 8000;
                this.bulkOilTrack.Maximum = 9000;
                this.bulkOilTrack.Value = 8700;
                this.bulkOilText.Text = "8700";
                this.label21.Text="Hydrate Density";
                this.densityOilTrack.Value = 920;
                this.densityOilText.Text = "920";
                this.label71.Visible = true;
                this.shearHydrateTrack.Visible = true;
                this.shearHydrateText.Visible = true;
                this.label72.Visible = true;
                this.seismicPanel.Visible = false;
                this.button2.Visible = false;
            }
            else if (this.mainCombo.SelectedIndex == 0)
            {
                this.label7.Text = "Bulk Modulus of Oil";
                this.bulkOilTrack.Minimum = 1000;
                this.bulkOilTrack.Maximum = 2000;
                this.bulkOilTrack.Value = 1410;
                this.bulkOilText.Text = "1410";
                this.label21.Text = "Oil Density";
                this.densityOilTrack.Value = 824;
                this.densityOilText.Text = "824";
                this.label71.Visible = false;
                this.shearHydrateTrack.Visible = false;
                this.shearHydrateText.Visible = false;
                this.label72.Visible = false;
                this.seismicPanel.Visible = false;
                this.button2.Visible = false;
            }
            else
            {
                this.seismicPanel.Visible = true;
                this.button2.Visible = true;
            }
        }
            

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.comboDensity.SelectedItem == null || this.comboSonic.SelectedItem == null || this.BRname==null)
                PetrelLogger.ErrorBox("INVALID INPUT");
            else
            {
                if (this.comboDensity.SelectedItem.ToString() == this.comboSonic.SelectedItem.ToString())
                {
                    PetrelLogger.ErrorBox("WRONG INPUT");
                    return;
                }
                WellLog densityLog = null;
                WellLog sonicLog = null;
                if (this.mainCombo.SelectedIndex == 0 || this.mainCombo.SelectedIndex == 1)
                {
                    if (this.comboDensity.SelectedItem == null || this.comboSonic.SelectedItem == null)
                    {
                        PetrelLogger.InfoOutputWindow("!!Process Terminated. Log Missing!!");
                    }
                    else
                    {
                        foreach (WellLog log in wlog)
                        {
                            if (this.comboDensity.SelectedItem.ToString() == log.Name)
                                densityLog = log;
                            else if (this.comboSonic.SelectedItem.ToString() == log.Name)
                                sonicLog = log;
                        }
                    }
                    if (!(densityLog == null || sonicLog == null))
                    {
                        PetrelLogger.InfoOutputWindow("!!Iteration started!!");
                        RACO obj1 = new RACO(densityLog, sonicLog);
                        obj1.rhoC = Convert.ToInt32(this.densityClayText.Text);
                        obj1.rhoQ = Convert.ToInt32(this.densityQuartzText.Text);
                        obj1.rhoW = Convert.ToInt32(this.densityWaterText.Text);
                        obj1.rhoO = Convert.ToInt32(this.densityOilText.Text);
                        obj1.Kc = Convert.ToInt64(this.bulkClayText.Text) * 1000000;
                        obj1.Kq = Convert.ToInt64(this.bulkQuartzText.Text) * 1000000;
                        obj1.Kw = Convert.ToInt64(this.bulkWaterText.Text) * 1000000;
                        obj1.Ko = Convert.ToInt64(this.bulkOilText.Text) * 1000000;
                        obj1.Gc = Convert.ToInt64(this.shearClayText.Text) * 1000000;
                        obj1.Gq = Convert.ToInt64(this.shearQuartzText.Text) * 1000000;
                        obj1.RPMndex = this.comboRPM.SelectedIndex;
                        obj1.porLog = this.porText.Text;
                        obj1.clayLog = this.claycontText.Text;
                        obj1.waterLog = this.waterSatText.Text;
                        obj1.minPor = Convert.ToDouble(this.minPorText.Text);
                        obj1.maxPor = Convert.ToDouble(this.maxPorText.Text);
                        obj1.minWater = Convert.ToDouble(this.minWaterText.Text);
                        obj1.maxWater = Convert.ToDouble(this.maxWaterText.Text);
                        obj1.minClay = Convert.ToDouble(this.minClayText.Text);
                        obj1.maxClay = Convert.ToDouble(this.maxClayText.Text);
                        obj1.maincomboIndex = this.mainCombo.SelectedIndex;
                        obj1.b1 = BR;
                        obj1.workingFunction();
                        PetrelLogger.InfoOutputWindow("Process Completed");
                    }

                }
            }
        }

        private void minPorTrack_Scroll(object sender, EventArgs e)
        {
            this.minPorText.Text = ((double)this.minPorTrack.Value/100.0).ToString();
            this.maxPorTrack.Minimum = this.minPorTrack.Value;
            if (this.maxPorTrack.Value < this.maxPorTrack.Minimum)
                maxPorTrack.Value = maxPorTrack.Minimum;
            this.maxPorText.Text = ((double)this.maxPorTrack.Value / 100.0).ToString();


        }

        private void maxPorTrack_Scroll(object sender, EventArgs e)
        {
            this.maxPorText.Text = ((double)this.maxPorTrack.Value / 100.0).ToString();
            this.minPorTrack.Maximum = this.maxPorTrack.Value;
            if (this.minPorTrack.Value > this.minPorTrack.Maximum)
                minPorTrack.Value = minPorTrack.Maximum;
            this.minPorText.Text = ((double)this.minPorTrack.Value / 100).ToString();
        }

        private void minWaterTrack_Scroll(object sender, EventArgs e)
        {
            this.minWaterText.Text = ((double)this.minWaterTrack.Value / 100.0).ToString();
            this.maxWaterTrack.Minimum = this.minWaterTrack.Value;
            if (this.maxWaterTrack.Value < this.maxWaterTrack.Minimum)
                maxWaterTrack.Value = maxWaterTrack.Minimum;
            this.maxWaterText.Text = ((double)this.maxWaterTrack.Value / 100.0).ToString();
        }

        private void maxWaterTrack_Scroll(object sender, EventArgs e)
        {
            this.maxWaterText.Text = ((double)this.maxWaterTrack.Value / 100.0).ToString();
            this.minWaterTrack.Maximum = this.maxWaterTrack.Value;
            if (this.minWaterTrack.Value > this.minWaterTrack.Maximum)
                minWaterTrack.Value = minWaterTrack.Maximum;
            this.minWaterText.Text = ((double)this.minWaterTrack.Value / 100).ToString();
        }

        private void minClayTrack_Scroll(object sender, EventArgs e)
        {
            this.minClayText.Text = ((double)this.minClayTrack.Value / 100.0).ToString();
            this.maxClayTrack.Minimum = this.minClayTrack.Value;
            if (this.maxClayTrack.Value < this.maxClayTrack.Minimum)
                maxClayTrack.Value = maxClayTrack.Minimum;
            this.maxClayText.Text = ((double)this.maxClayTrack.Value / 100.0).ToString();
        }

        private void maxClayTrack_Scroll(object sender, EventArgs e)
        {
            this.maxClayText.Text = ((double)this.maxClayTrack.Value / 100.0).ToString();
            this.minClayTrack.Maximum = this.maxClayTrack.Value;
            if (this.minClayTrack.Value > this.minClayTrack.Maximum)
                minClayTrack.Value = minClayTrack.Maximum;
            this.minClayText.Text = ((double)this.minClayTrack.Value / 100).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            
        }

        private void shearHydrateTrack_Scroll(object sender, EventArgs e)
        {
            shearHydrateText.Text = shearHydrateTrack.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (RHO == null || DT == null ||this.porText1.Text=="" || this.waterText1.Text=="" || this.VshText1.Text=="")
                PetrelLogger.ErrorBox("INVALID INPUT");
            else
            {
                if (!(RHO.NumSamplesIJK.I == DT.NumSamplesIJK.I && RHO.NumSamplesIJK.J == DT.NumSamplesIJK.J || RHO.NumSamplesIJK.K == DT.NumSamplesIJK.K))
                {
                    PetrelLogger.ErrorBox("Cube Dimension Mismatch");
                    return;
                }
                Reservoir obj1 = new Reservoir(RHO, DT);
                obj1.rhoC = Convert.ToInt32(this.densityClayText1.Text);
                obj1.rhoQ = Convert.ToInt32(this.densityQuartzText1.Text);
                obj1.rhoW = Convert.ToInt32(this.densityWaterText1.Text);
                obj1.rhoO = Convert.ToInt32(this.densityOilText1.Text);
                obj1.Kc = Convert.ToInt64(this.bulkClayText1.Text) * 1000000;
                obj1.Kq = Convert.ToInt64(this.bulkQuartzText1.Text) * 1000000;
                obj1.Kw = Convert.ToInt64(this.bulkWaterText1.Text) * 1000000;
                obj1.Ko = Convert.ToInt64(this.bulkOilText1.Text) * 1000000;
                obj1.Gc = Convert.ToInt64(this.shearClayText1.Text) * 1000000;
                obj1.Gq = Convert.ToInt64(this.shearQuartzText1.Text) * 1000000;
                obj1.RPMndex = this.comboRPM2.SelectedIndex;
                obj1.minPor = Convert.ToDouble(this.minPorText1.Text);
                obj1.maxPor = Convert.ToDouble(this.maxPorText1.Text);
                obj1.minWater = Convert.ToDouble(this.minWaterText1.Text);
                obj1.maxWater = Convert.ToDouble(this.maxWaterText1.Text);
                obj1.minClay = Convert.ToDouble(this.minClayText1.Text);
                obj1.maxClay = Convert.ToDouble(this.maxClayText1.Text);
                obj1.porCube = this.porText1.Text;
                obj1.clayCube = this.VshText1.Text;
                obj1.waterCube = this.waterText1.Text;
                obj1.quality = this.trackQuality.Value;
                obj1.workingFunction();
            }

        }



        private void bulkClayTrack1_Scroll(object sender, EventArgs e)
        {
            bulkClayText1.Text = bulkClayTrack1.Value.ToString();
        }

        private void bulkQuartzTrack1_Scroll(object sender, EventArgs e)
        {
            bulkQuartzText1.Text = bulkQuartzTrack1.Value.ToString();
        }

        private void bulkWaterTrack1_Scroll(object sender, EventArgs e)
        {
            bulkWaterText1.Text = bulkWaterTrack1.Value.ToString();
        }

        private void bulkOilTrack1_Scroll(object sender, EventArgs e)
        {
            bulkOilText1.Text = bulkOilTrack1.Value.ToString();
        }

        private void shearClayTrack1_Scroll(object sender, EventArgs e)
        {
            shearClayText1.Text = shearClayTrack1.Value.ToString();
        }

        private void shearQuartzTrack1_Scroll(object sender, EventArgs e)
        {
            shearQuartzText1.Text = shearQuartzTrack1.Value.ToString();
        }

        private void densityClayTrack1_Scroll(object sender, EventArgs e)
        {
            densityClayText1.Text = densityClayTrack1.Value.ToString();
        }

        private void densityQuartzTrack1_Scroll(object sender, EventArgs e)
        {
            densityQuartzText1.Text = densityQuartzTrack1.Value.ToString();
        }

        private void densityWaterTrack1_Scroll(object sender, EventArgs e)
        {
            densityWaterText1.Text = densityWaterTrack1.Value.ToString();
        }

        private void densityOilTrack1_Scroll(object sender, EventArgs e)
        {
            densityOilText1.Text = densityOilTrack1.Value.ToString();
        }

        private void minPorTrack1_Scroll(object sender, EventArgs e)
        {
            this.minPorText1.Text = ((double)this.minPorTrack1.Value / 100.0).ToString();
            this.maxPorTrack1.Minimum = this.minPorTrack1.Value;
            if (this.maxPorTrack1.Value < this.maxPorTrack1.Minimum)
                maxPorTrack1.Value = maxPorTrack1.Minimum;
            this.maxPorText1.Text = ((double)this.maxPorTrack1.Value / 100.0).ToString();
        }

        private void minWaterTrack1_Scroll(object sender, EventArgs e)
        {
            this.minWaterText1.Text = ((double)this.minWaterTrack1.Value / 100.0).ToString();
            this.maxWaterTrack1.Minimum = this.minWaterTrack1.Value;
            if (this.maxWaterTrack1.Value < this.maxWaterTrack1.Minimum)
                maxWaterTrack1.Value = maxWaterTrack1.Minimum;
            this.maxWaterText1.Text = ((double)this.maxWaterTrack1.Value / 100.0).ToString();
        }

        private void minClayTrack1_Scroll(object sender, EventArgs e)
        {
            this.minClayText1.Text = ((double)this.minClayTrack1.Value / 100.0).ToString();
            this.maxClayTrack1.Minimum = this.minClayTrack1.Value;
            if (this.maxClayTrack1.Value < this.maxClayTrack1.Minimum)
                maxClayTrack1.Value = maxClayTrack1.Minimum;
            this.maxClayText1.Text = ((double)this.maxClayTrack1.Value / 100.0).ToString();
        }

        private void maxPorTrack1_Scroll(object sender, EventArgs e)
        {
            this.maxPorText1.Text = ((double)this.maxPorTrack1.Value / 100.0).ToString();
            this.minPorTrack1.Maximum = this.maxPorTrack1.Value;
            if (this.minPorTrack1.Value > this.minPorTrack1.Maximum)
                minPorTrack1.Value = minPorTrack1.Maximum;
            this.minPorText1.Text = ((double)this.minPorTrack1.Value / 100).ToString();
        }

        private void maxWaterTrack1_Scroll(object sender, EventArgs e)
        {
            this.maxWaterText1.Text = ((double)this.maxWaterTrack1.Value / 100.0).ToString();
            this.minWaterTrack1.Maximum = this.maxWaterTrack1.Value;
            if (this.minWaterTrack1.Value > this.minWaterTrack1.Maximum)
                minWaterTrack1.Value = minWaterTrack1.Maximum;
            this.minWaterText1.Text = ((double)this.minWaterTrack1.Value / 100).ToString();
        }

        private void maxClayTrack1_Scroll(object sender, EventArgs e)
        {
            this.maxClayText1.Text = ((double)this.maxClayTrack1.Value / 100.0).ToString();
            this.minClayTrack1.Maximum = this.maxClayTrack1.Value;
            if (this.minClayTrack1.Value > this.minClayTrack1.Maximum)
                minClayTrack1.Value = minClayTrack1.Maximum;
            this.minClayText1.Text = ((double)this.minClayTrack1.Value / 100).ToString();
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            ((Form)this.Parent).Hide();
            this.Dispose(true);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            PetrelLogger.InfoBox("");
        }

        private void help_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\help.chm");
                sw.Close();
                FileStream fs = new FileStream("C:\\help.chm", FileMode.Open, FileAccess.Write);
                Stream str;

                fs.Write(global::WellReader.Resource_chracterizer.Well_and_Reservoir_Characterization, 0, global::WellReader.Resource_chracterizer.Well_and_Reservoir_Characterization.Count());
                fs.Close();

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\help.chm";
                p.Start();
            }
            catch
            {
                PetrelLogger.InfoOutputWindow("Unable to Read to your drive -check permissions");
            };
            
        }

        private void genButton_Click(object sender, EventArgs e)
        {
            if (logX == null || logY == null || logZ == null)
            {
                MessageBox.Show("Error! Select three Logs!");
                return;
            }
            if (logX.SampleCount != logY.SampleCount ||
                logY.SampleCount != logZ.SampleCount)
            {
                MessageBox.Show("All logs must be of same Dimension!");
                return;
            }
            List<WellLogSample> listX = new List<WellLogSample>(logX.Samples);
            List<WellLogSample> listY = new List<WellLogSample>(logY.Samples);
            List<WellLogSample> listZ = new List<WellLogSample>(logZ.Samples);
            if (Math.Abs(listX[0].MD - listY[0].MD) >= 10E-4)
            {
                MessageBox.Show(logX.Name + " and " + logY.Name + "do not start at same Measured depth!");
                return;
            }
            if (Math.Abs(listX[0].MD - listZ[0].MD) >= 10E-4)
            {
                MessageBox.Show(logX.Name + " and " + logZ.Name + "do not start at same Measured depth!");
                return;
            }
            if (Math.Abs(listX[listX.Count - 1].MD - listY[listY.Count - 1].MD) >= 10E-4)
            {
                MessageBox.Show(logX.Name + " and " + logY.Name + "do not end at same Measured depth!");
                return;
            }
            if (Math.Abs(listX[listX.Count - 1].MD - listZ[listZ.Count - 1].MD) >= 10E-4)
            {
                MessageBox.Show(logX.Name + " and " + logZ.Name + "do not end at same Measured depth!");
                return;
            }
            CrossPlotter.CrossPlotUI plot = new CrossPlotter.CrossPlotUI(logX, logY, logZ);
            plot.Show();
            Invalidate();
        }

        private void xDropper_DragDrop(object sender, DragEventArgs e)
        {
            logX = (WellLog)e.Data.GetData(typeof(WellLog));
            if (logX != null)
            {
                xText.Text = logX.Name;
            }
            else
            {
                xText.Text = "";
                MessageBox.Show("Not a valid Well Log");
            }
        }

        private void yDropper_DragDrop(object sender, DragEventArgs e)
        {
            logY = (WellLog)e.Data.GetData(typeof(WellLog));
            if (logY != null)
            {
                yText.Text = logY.Name;
            }
            else
            {
                yText.Text = "";
                MessageBox.Show("Not a valid Well Log");
            }
        }

        private void zDropper_DragDrop(object sender, DragEventArgs e)
        {
            logZ = (WellLog)e.Data.GetData(typeof(WellLog));
            if (logZ != null)
            {
                zText.Text = logZ.Name;
            }
            else
            {
                zText.Text = "";
                MessageBox.Show("Not a valid Well Log");
            }
        }

        private void joinServerBtn_Click(object sender, EventArgs e)
        {
            new ChatClient.ClientForm().Show();
        }

        private void visual_dropTarget_DragDrop(object sender, DragEventArgs e)
        {
            SeismicCube droppedCube = e.Data.GetData(typeof(SeismicCube)) as SeismicCube;
            if (droppedCube != null)
            {
                visual_textBox.Text = droppedCube.Name;
                visual_cube = droppedCube;
            }
            else
            {
                MessageBox.Show("INVALID CUBE");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (visual_cube == null)
            {
                MessageBox.Show("Select a valid Cube.");
                return;
            }
            Index3 start, end;
            ISubCube from;
            try
            {
                start = new Index3(0, 0, 0);
                end = new Index3(visual_cube.NumSamplesIJK.I - 1, visual_cube.NumSamplesIJK.J - 1, visual_cube.NumSamplesIJK.K - 1);
                from = visual_cube.GetSubCube(start, end);
                
            }
            catch (System.InvalidOperationException)
            {
                PetrelLogger.ErrorBox("NOT A PROPER INPUT CUBE");
                return;
            }
            float[, ,] vals = from.ToArray();
            Plot3D.Plot3DMainForm obj3D=new Plot3D.Plot3DMainForm(vals);
            obj3D.scube = visual_cube;
            obj3D.Show();
            
        }

        private void label67_Click(object sender, EventArgs e)
        {

        }

        private void trackQuality_Scroll(object sender, EventArgs e)
        {

        }

        private void borehole_DragDrop(object sender, DragEventArgs e)
        {
            BR = (Borehole)e.Data.GetData(typeof(Borehole));
            if (BR != null)
            {
                BRname.Text = BR.Name;
            }
            else
            {
                BRname.Text = "";
                MessageBox.Show("Not a valid Borehole");
                return;
            }

            
            this.comboDensity.Items.Clear();
            this.comboSonic.Items.Clear();
            wlog.Clear();
            int tempctr1, tempctr2;
            int tempctr = 0;
            tempctr1 = 0;
            tempctr2 = 0;

            foreach (WellLog log in BR.Logs.WellLogs)
            {
                wlog.Add(log);
                this.comboDensity.Items.Add(log.Name);
                this.comboSonic.Items.Add(log.Name);
                tempctr++;
                if (log.Name.Contains("HO"))
                    tempctr1 = tempctr;
                else if (log.Name.Contains("DT"))
                    tempctr2 = tempctr;

            }
            this.comboDensity.SelectedIndex = tempctr1 - 1;
            this.comboSonic.SelectedIndex = tempctr2 - 1;
        }

        private void dropRHO_DragDrop(object sender, DragEventArgs e)
        {
            RHO = (SeismicCube)e.Data.GetData(typeof(SeismicCube));
            if (RHO != null)
            {
                textRHO.Text = RHO.Name;
            }
            else
            {
                textRHO.Text = "";
                MessageBox.Show("Invalid Cube");
                return;
            }
        }

        private void dragDT(object sender, DragEventArgs e)
        {
            DT = (SeismicCube)e.Data.GetData(typeof(SeismicCube));
            if (DT != null)
            {
                textDT.Text = DT.Name;
            }
            else
            {
                textDT.Text = "";
                MessageBox.Show("Invalid Cube");
                return;
            }
        }
    }
}
