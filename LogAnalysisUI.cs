using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;


using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI.Controls;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Basics;
using Slb.Ocean.Data;
using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System.Collections.Generic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Geometry;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



namespace LogAnalysis
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    public partial class LogAnalysisUI : UserControl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Workstep1UI"/> class.
        /// </summary>
        /// <param name="workstep">the workstep instance</param>
        /// <param name="args">the arguments</param>
        /// <param name="context">the underlying context in which this UI is being used</param>

        //WellLogs are declayred here
        WellLog porWell, swWell, vshWell;
        WellLog velWell;
        WellLog PSVelWell, AEImpWell;
        WellLog DensWell, swWell2, vshWell2;
        WellLog SdtWell, swWell3, vshWell3;
        WellLog AcImpWell;
        WellLog porWell2, swWell4, vshWell4;
        WellLog SdtWell2, vshWell5;
        WellLog porWell3, swWell6, vshWell6;
        WellLog NporWell, vshWell7;
        WellLog GrWell, SpWell;
        WellLog SdtWell3;
        WellLog DensWell2, PSVelWell2;
        WellLog DensWell3;
        WellLog DensWell4;
        WellLog vshWell8;
        WellLog vpvsWell1;
        WellLog DensWell5, SdtWell4, poisWell1;
        WellLog DensWell6, SdtWell5, poisWell2;

        public LogAnalysisUI()
        {
            InitializeComponent();
            this.porWell = WellLog.NullObject; this.swWell = WellLog.NullObject; this.vshWell = WellLog.NullObject;
            this.comboLogs1.SelectedIndex = 0;
            this.comboLogs2.SelectedIndex = 0;
            OK.Image = PetrelImages.OK;

            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 0)
            {
                this.DensityPan1.Visible = true;
                this.DensityPan2.Visible = false;
                this.DensityPan2.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;

            }

            GCText.Text = "0.31";
            GEText.Text = "0.25";



        }

        public void comboLogs1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.comboLogs1.SelectedIndex == 0)
            {
                this.comboLogs2.Items.Clear();

                this.comboLogs2.Items.Add("Porosity->Density");
                this.comboLogs2.Items.Add("Velocity->Density");
                this.comboLogs2.Items.Add("Impedance->Density");
                this.comboLogs2.SelectedIndex = 0;

            }
            else if (this.comboLogs1.SelectedIndex == 1)
            {

                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Density->Porosity Log");
                this.comboLogs2.Items.Add("Sonic->Porosity Log(Wyllie Method)");
                this.comboLogs2.Items.Add("Sonic->Porosity Log(Hunt-Raymer Method)");
                this.comboLogs2.Items.Add("Neutron Porosity->Porosity Log");
                this.comboLogs2.Items.Add("");
                this.comboLogs2.SelectedIndex = 0;

            }
            else if (this.comboLogs1.SelectedIndex == 2)
            {
                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Acoustic Impedance->Reflection Coefficient Log");
                this.comboLogs2.SelectedIndex = 0;


            }
            else if (this.comboLogs1.SelectedIndex == 3)
            {
                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Porosity->Velocity Log(Hunt-Raymer Method)");
                this.comboLogs2.Items.Add("Porosity->Velocity Log(Wyllie Method)");
                this.comboLogs2.Items.Add("Density->Velocity Log(Gardner's Method)");

                this.comboLogs2.SelectedIndex = 0;
            }
            else if (this.comboLogs1.SelectedIndex == 4)
            {
                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Gamma-Ray->Shale volume Log");
                this.comboLogs2.Items.Add("SP->Shale volume Log");

                this.comboLogs2.SelectedIndex = 0;
            }
            else if (comboLogs1.SelectedIndex == 5)
            {
                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Sonic Log->Impedance Log(Gardner's Method)");
                this.comboLogs2.Items.Add("Density Log->Impedance Log");
                this.comboLogs2.Items.Add("Density Log->Impedance Log(Gardner's Method)");

                this.comboLogs2.SelectedIndex = 0;
            }
            else if (comboLogs1.SelectedIndex == 6)
            {
                this.comboLogs2.Items.Clear();
                this.comboLogs2.Items.Add("Poison Ratio Log->Shale Volume Log");
                this.comboLogs2.Items.Add("Poison Ratio Log->P and S Velocity Ratio Log");
                this.comboLogs2.Items.Add("Shear Modulus Log>Poison Ratio Log");
                this.comboLogs2.Items.Add("Bulk Modulus Log>Poison Ratio Log");

                this.comboLogs2.SelectedIndex = 0;
            }
        }



        public void comboLogs2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 0)
            {
                this.DensityPan1.Visible = true;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Density log is created using the classical equation of mass balance taking input from porosity log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are density of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");
            }

            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 0)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = true;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Density log is created using the Gardner's empirical relation taking input from Velocity log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are Gardner's coefficient (a) and Gardner's exponent (b)set to default values which can be changed accordingly.");
            }

            if (this.comboLogs2.SelectedIndex == 2 && this.comboLogs1.SelectedIndex == 0)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = true;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Density log is created using the relation Z=Rhob*Vel taking input from Impedance and Velocity log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");
            }

            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 1)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = true;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Porosity log is created using mass balance equation taking input from density log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are density of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");
            }

            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 1)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = true;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Porosity log is created using Wyllie's method taking input from sonic log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are sonic values of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");
            }

            if (this.comboLogs2.SelectedIndex == 2 && this.comboLogs1.SelectedIndex == 1)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = true;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Porosity log is created using Hunt-Raymer's method taking input from sonic log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are sonic values of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");

            }
            if (this.comboLogs2.SelectedIndex == 3 && this.comboLogs1.SelectedIndex == 1)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = true;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Porosity log is created using classical balance method taking input from neutron porosity log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nOnly one parameter is involved in this method neutron porosity in 100% shale set to default value which can be changed accordingly.");

            }

            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 2)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = true;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Reflection coefficient log is created using equation R.C = Z2-Z1 / Z2+Z1 taking input from Impedance log.";
                this.comboLogsInfoText.AppendText("\r\nNo Parameters are involved in this method.");

            }
            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 3)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = true;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Velocity log is created using Hunt-Raymer's method taking input from porosity log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are sonic values of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");

            }

            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 3)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = true;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Velocity log is created using Wyllie's method taking input from porosity log, water saturation log and shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are sonic values of water, hydrocarbon, shale and rock matrix set to default values which can be changed accordingly.");
            }
            if (this.comboLogs2.SelectedIndex == 2 && this.comboLogs1.SelectedIndex == 3)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = true;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Veloity log is created using the Gardner's empirical relation taking input from density log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are Gardner's coefficient (a) and Gardner's exponent (b) set to default values which can be changed for P-Velocity or S-Velocity.");
            }
            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 4)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = true;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Shale Volume Fraction log is created using gamma-ray method taking input from gamma-ray log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are gamma-ray absorbance values in 0% (GRmin) and 100% (GRmax) shale. These values are set to default and can be changed accordingly.");
            }
            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 4)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = true;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Shale Volume Fraction log is created using spontaneous potential (SP) method taking input from SP log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are SP values in 0% (SPmin) and 100% (SPmax) shale. These values are set to default and can be changed accordingly.");
            }

            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 5)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = true;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Impedance log is created using the Gardner's empirical relation taking input from sonic log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are Gardner's coefficient (a) and Gardner's exponent (b) set to default values which can be changed accordingly.");


            }

            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 5)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = true;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Impedance log is created using the relation Z=Rhob*Vel taking input from Density log and Velocity log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");

            }
            if (this.comboLogs2.SelectedIndex == 2 && this.comboLogs1.SelectedIndex == 5)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = true;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Impedance log is created using the Gardner's empirical relation taking input from Velocity log.";
                this.comboLogsInfoText.AppendText("\r\nParameters involved in this method are Gardner's coefficient (a) and Gardner's exponent (b) set to default values which can be changed accordingly.");

            }

            if (this.comboLogs2.SelectedIndex == 0 && this.comboLogs1.SelectedIndex == 6)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = true;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Poisson ratio log is created using the empirical relation taking input from shale volume fraction log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");

            }
            if (this.comboLogs2.SelectedIndex == 1 && this.comboLogs1.SelectedIndex == 6)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = true;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Poisson ratio log is created using standard equation taking input from Vp/Vs ratio log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");

            }
            if (this.comboLogs2.SelectedIndex == 2 && this.comboLogs1.SelectedIndex == 6)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = true;
                this.ElasticPan4.Visible = false;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Shear modulus log is created using standard equation taking input from density log, s-velocity log and poisson ratio log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");


            }
            if (this.comboLogs2.SelectedIndex == 3 && this.comboLogs1.SelectedIndex == 6)
            {
                this.DensityPan1.Visible = false;
                this.DensityPan2.Visible = false;
                this.DensityPan3.Visible = false;
                this.PorosityPan1.Visible = false;
                this.PorosityPan2.Visible = false;
                this.PorosityPan3.Visible = false;
                this.PorosityPan4.Visible = false;
                this.RCPan.Visible = false;
                this.VelocityPan1.Visible = false;
                this.VelocityPan2.Visible = false;
                this.VelocityPan3.Visible = false;
                this.ShaleVolumePan1.Visible = false;
                this.ShaleVolumePan2.Visible = false;
                this.ImpedancePan1.Visible = false;
                this.ImpedancePan2.Visible = false;
                this.ImpedancePan3.Visible = false;
                this.ElasticPan1.Visible = false;
                this.ElasticPan2.Visible = false;
                this.ElasticPan3.Visible = false;
                this.ElasticPan4.Visible = true;
                this.comboLogsInfoText.Clear();
                this.comboLogsInfoText.Text = "Bulk modulus log is created using standard equation taking input from density log, p-velocity log and poisson ratio log.";
                this.comboLogsInfoText.AppendText("\r\nNo parameters are required to carry out this process.");

            }
        }

        /*Info display of each log
       public void comboLogs3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboLogs1.SelectedIndex == 0 && comboLogs2.SelectedIndex == 0)
            {
                comboLogs3.Items.Clear();
                comboLogs3.Items.Add("Density log is created using the classical equation taking input from porosity log, water saturation log and shale volume fraction log");
                comboLogs3.Items.Add("Parameters involved in this method are density of water, hydrocarbon, shale and matrix");
            }
 
        }*/



        //Reading Logs from well inputs



        public void porDrop1_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            porWell = dropped as WellLog;

            if (porWell == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            porText1.Text = porWell.Name;

        }

        public void swDrop1_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            swWell = dropped as WellLog;
            if (swWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                swText1.Text = swWell.Name;
            }

        }

        public void vshDrop1_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell = dropped as WellLog;
            if (vshWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vshText1.Text = vshWell.Name;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        public void OK_Click(object sender, EventArgs e)
        {
            // if Density from Porosity is calcuated

            if (comboLogs1.SelectedIndex == 0 && comboLogs2.SelectedIndex == 0)
            {
                if (porWell == null || swWell == null || vshWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                Density D1 = new Density(porWell, swWell, vshWell);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());
                D1.rhow = (double)DensityWater.Value;
                D1.rhoh = (double)DensityHydrocarbon.Value;
                D1.rhosh = (double)DensityShale.Value;
                D1.rhomat = (double)DensityMatrix.Value;
                D1.fromporosity();
                //PetrelLogger.InfoBox("The Density Log has been created in the same Well");

            }


            // if Density from Velocity using gardner's equation is calcuated

            if (comboLogs1.SelectedIndex == 0 && comboLogs2.SelectedIndex == 1)
            {
                if (velWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                Density D2 = new Density(velWell);

                D2.gcoef = (double)GardCoef.Value / 100;
                D2.gexp = (double)GardExp.Value / 100;

                D2.fromvelocity();
                //PetrelLogger.InfoBox("The Density Log has been created in the same Well");
            }

            //If Density from Impedance is calculated

            if (comboLogs1.SelectedIndex == 0 && comboLogs2.SelectedIndex == 2)
            {
                if (AEImpWell == null || PSVelWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                Density D2 = new Density(AEImpWell, PSVelWell);
                D2.fromimpedance();
                //PetrelLogger.InfoBox("The Density Log has been created in the same Well");
            }

            //If Porosity from Density is calculated

            if (comboLogs1.SelectedIndex == 1 && comboLogs2.SelectedIndex == 0)
            {
                if (DensWell == null || swWell2 == null || vshWell2 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                Porosity P1 = new Porosity(DensWell, swWell2, vshWell2);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());
                P1.rhow = (double)DensityWater2.Value;
                P1.rhoh = (double)DensityHydrocarbon2.Value;
                P1.rhosh = (double)DensityShale2.Value;
                P1.rhomat = (double)DensityMatrix2.Value;
                P1.FromDensity();
                //PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");

            }

            //If Porosity from Sonic is calculated(Wyllie Method)

            if (comboLogs1.SelectedIndex == 1 && comboLogs2.SelectedIndex == 1)
            {
                if (SdtWell == null || swWell3 == null || vshWell3 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;
                Porosity P2 = new Porosity(SdtWell, swWell3, vshWell3, no_use);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());

                P2.dtw = (double)SonicWater.Value / 1000000;
                P2.dth = (double)SonicHydrocarbon.Value / 1000000;
                P2.dtsh = (double)SonicShale.Value / 1000000;
                P2.dtmat = (double)SonicMatrix.Value / 1000000;
                P2.FromSonic();
                //PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");

            }

            //Porosity from Sonic is calculated(Hunt-Raymer Method)

            if (comboLogs1.SelectedIndex == 1 && comboLogs2.SelectedIndex == 2)
            {
                if (SdtWell2 == null || vshWell5 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                Porosity P3 = new Porosity(SdtWell2, vshWell5);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());

                P3.dtw = (double)SonicWater.Value / 1000000;
                P3.dtsh = (double)SonicShale.Value / 1000000;
                P3.dtmat = (double)SonicMatrix.Value / 1000000;
                P3.FromSonic2();
                //PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");
            }

            //Porosity from neutron density log

            if (comboLogs1.SelectedIndex == 1 && comboLogs2.SelectedIndex == 3)
            {
                if (NporWell == null || vshWell7 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;

                Porosity P4 = new Porosity(NporWell, vshWell7, no_use);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());

                P4.Gnsh = (double)NDShale.Value / 100;

                P4.FromNeutron();
                //PetrelLogger.InfoBox("The Porosity Log has been created in the same Well");
            }

            //If Reflection coefficient from Acoustic Impedance is calculate

            if (comboLogs1.SelectedIndex == 2 && comboLogs2.SelectedIndex == 0)
            {
                if (AcImpWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                ReflectionCoefficient R1 = new ReflectionCoefficient(AcImpWell);
                R1.FromAcImp();
                //PetrelLogger.InfoBox("The Reflection coefficient Log has been created in the same Well");
            }


            //velocity from porosity is calculated using Hunt-Raymer method

            if (comboLogs1.SelectedIndex == 3 && comboLogs2.SelectedIndex == 0)
            {
                if (porWell2 == null || swWell4 == null || vshWell4 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                Velocity V1 = new Velocity(porWell2, swWell4, vshWell4);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());

                V1.dtw = (double)SonicWater3.Value / 1000000;
                V1.dth = (double)SonicHydrocarbon3.Value / 1000000;
                V1.dtsh = (double)SonicShale3.Value / 1000000;
                V1.dtmat = (double)SonicMatrix3.Value / 1000000;
                V1.FromPorosity2();
                //PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");

            }

            //velocity from porosity is calculated using Wyllie Method


            if (comboLogs1.SelectedIndex == 3 && comboLogs2.SelectedIndex == 1)
            {
                if (porWell3 == null || swWell6 == null || vshWell6 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;
                Velocity V2 = new Velocity(porWell3, swWell6, vshWell6, no_use);
                //PetrelLogger.InfoOutputWindow(DensityWater.Value.ToString());

                V2.dtw = (double)SonicWater4.Value / 1000000;
                V2.dth = (double)SonicHydrocarbon4.Value / 1000000;
                V2.dtsh = (double)SonicShale4.Value / 1000000;
                V2.dtmat = (double)SonicMatrix4.Value / 1000000;
                V2.FromPorosity3();
                //PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");

            }

            //Velocity from gardner's relation is calculated here

            if (comboLogs1.SelectedIndex == 3 && comboLogs2.SelectedIndex == 2)
            {
                if (DensWell3 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                Velocity V3 = new Velocity(DensWell3);

                V3.GCoef = (double)GardCoef3.Value / 100;
                V3.GExp = (double)GardExp3.Value / 100;

                V3.FromDensity();
                V3.save();
                //PetrelLogger.InfoBox("The Velocity Log has been created in the same Well");
            }

            //Shale volume from gamma-ray log is calculated here

            if (comboLogs1.SelectedIndex == 4 && comboLogs2.SelectedIndex == 0)
            {
                if (GrWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                ShaleVolume SH1 = new ShaleVolume(GrWell);
                SH1.Gr0 = (double)ShaleGR0.Value;
                SH1.Gr100 = (double)ShaleGR100.Value;
                SH1.FromGamma();
                //PetrelLogger.InfoBox("The Shale Volume Fraction Log has been created in the same Well");
            }

            //Shale volume from spontaneous potential log is calculated here

            if (comboLogs1.SelectedIndex == 4 && comboLogs2.SelectedIndex == 1)
            {
                if (SpWell == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;

                ShaleVolume SH2 = new ShaleVolume(SpWell, no_use);
                SH2.Sp0 = (double)SPShale0.Value / 1000;
                SH2.Sp100 = (double)SPShale100.Value / 1000;
                SH2.FromSP();
                //PetrelLogger.InfoBox("The Shale Volume Fraction Log has been created in the same Well");
            }

            //Impedance using Gardners equation is calculated here

            if (comboLogs1.SelectedIndex == 5 && comboLogs2.SelectedIndex == 0)
            {
                if (SdtWell3 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                Impedance I1 = new Impedance(SdtWell3);

                I1.GCoef = (double)GardCoef2.Value / 100;
                I1.GExp = (double)GardExp2.Value / 100;

                I1.FromSonic();
                //PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
            }
            //Impedance from Density and Velocity is calculated 

            if (comboLogs1.SelectedIndex == 5 && comboLogs2.SelectedIndex == 1)
            {
                if (DensWell2 == null || PSVelWell2 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                Impedance I2 = new Impedance(PSVelWell2, DensWell2);
                I2.FromDensity();
                //PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
            }

            //Impedance from density using gardners's method
            if (comboLogs1.SelectedIndex == 5 && comboLogs2.SelectedIndex == 2)
            {
                if (DensWell4 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;
                Impedance I3 = new Impedance(DensWell4, no_use);

                I3.GCoef = (double)GardCoef4.Value / 100;
                I3.GExp = (double)GardExp4.Value / 100;

                I3.FromDensity2();
                //PetrelLogger.InfoBox("The Impedance Log has been created in the same Well");
            }

            if (comboLogs1.SelectedIndex == 6 && comboLogs2.SelectedIndex == 0)
            {
                if (vshWell8 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }

                ElasticConstants E1 = new ElasticConstants(vshWell8);
                E1.FromVsh();
                //E1.save(vshWell8, E1.poisn, "Poissons ratio");
                //PetrelLogger.InfoBox("The Poisson Ratio Log has been created in the same Well");

            }
            if (comboLogs1.SelectedIndex == 6 && comboLogs2.SelectedIndex == 1)
            {
                if (vpvsWell1 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                int no_use = 0;

                ElasticConstants E2 = new ElasticConstants(vpvsWell1, no_use);
                E2.FromVpvs();
                //PetrelLogger.InfoBox("The Poisson Ratio Log has been created in the same Well");


            }
            if (comboLogs1.SelectedIndex == 6 && comboLogs2.SelectedIndex == 2)
            {
                if (DensWell5 == null && SdtWell4 == null && poisWell1 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                ElasticConstants E3 = new ElasticConstants(DensWell5, SdtWell4, poisWell1);
                E3.FromDensDt1();
                //PetrelLogger.InfoBox("The Shear Modulus Log has been created in the same Well");


            }
            if (comboLogs1.SelectedIndex == 6 && comboLogs2.SelectedIndex == 3)
            {
                if (DensWell6 == null && SdtWell5 == null && poisWell2 == null)
                {
                    PetrelLogger.ErrorBox("One or more inputs are not given properly");
                    return;
                }
                ElasticConstants E4 = new ElasticConstants(DensWell6, SdtWell5, poisWell2);
                E4.FromDensDt2();

                //PetrelLogger.InfoBox("The Bulk Modulus Log has been created in the same Well");
            }
        }

        public void DensityWater_Scroll(object sender, EventArgs e)
        {

            DWText.Text = DensityWater.Value.ToString();
        }

        public void DensityHydrocarbon_Scroll(object sender, EventArgs e)
        {
            DHText.Text = DensityHydrocarbon.Value.ToString();

        }

        public void DensityShale_Scroll(object sender, EventArgs e)
        {
            DSText.Text = DensityShale.Value.ToString();

        }

        public void DensityMatrix_Scroll(object sender, EventArgs e)
        {
            DMText.Text = DensityMatrix.Value.ToString();
        }

        public void GardCoef_Scroll(object sender, EventArgs e)
        {
            GCText.Text = ((float)GardCoef.Value / 100).ToString();
        }

        public void GardExp_Scroll(object sender, EventArgs e)
        {
            GEText.Text = ((float)GardExp.Value / 100).ToString();
        }

        // Reading Velocity Logs



        public void pvelLog1_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            velWell = dropped as WellLog;
            if (velWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN"); 
                pveltext1.Text = velWell.Name;
            }

        }

        public void AEImpLog_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            AEImpWell = dropped as WellLog;
            if (AEImpWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                AEImpText.Text = AEImpWell.Name;
            }

        }

        public void PSVelLog_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            PSVelWell = dropped as WellLog;
            if (PSVelWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                PSVelText.Text = PSVelWell.Name;
            }
        }

        public void cancel_Click(object sender, EventArgs e)
        {
            ((Form)this.Parent).Hide();
            this.Dispose(true);
        }



        //Porosity calculation process starting from here

        public void DensLog1_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            DensWell = dropped as WellLog;

            if (DensWell == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            DensText1.Text = DensWell.Name;

        }


        public void swDrop2_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            swWell2 = dropped as WellLog;
            if (swWell2 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                swText2.Text = swWell2.Name;
            }

        }

        public void vshDrop2_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell2 = dropped as WellLog;
            if (vshWell2 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vshText2.Text = vshWell2.Name;
            }

        }
        public void DensityWater2_Scroll(object sender, EventArgs e)
        {

            DWText2.Text = DensityWater2.Value.ToString();
        }

        public void DensityHydrocarbon2_Scroll(object sender, EventArgs e)
        {
            DHText2.Text = DensityHydrocarbon2.Value.ToString();

        }

        public void DensityShale2_Scroll(object sender, EventArgs e)
        {
            DSText2.Text = DensityShale2.Value.ToString();

        }

        public void DensityMatrix2_Scroll(object sender, EventArgs e)
        {
            DMText2.Text = DensityMatrix2.Value.ToString();
        }


        //Porosity calculation from sonic log

        public void SonDrop1_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            SdtWell = dropped as WellLog;

            if (SdtWell == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            SdtText.Text = SdtWell.Name;

        }


        public void swDrop3_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            swWell3 = dropped as WellLog;
            if (swWell3 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                SwText3.Text = swWell3.Name;
            }

        }

        public void VSHDrop_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell3 = dropped as WellLog;
            if (vshWell3 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                VshText3.Text = vshWell3.Name;
            }

        }
        public void SonicWater_Scroll(object sender, EventArgs e)
        {

            SwText.Text = SonicWater.Value.ToString();
        }

        public void SonicHydrocarbon_Scroll(object sender, EventArgs e)
        {
            ShText.Text = SonicHydrocarbon.Value.ToString();

        }

        public void SonicShale_Scroll(object sender, EventArgs e)
        {
            SsText.Text = SonicShale.Value.ToString();

        }

        public void SonicMatrix_Scroll(object sender, EventArgs e)
        {
            SmText.Text = SonicMatrix.Value.ToString();
        }

        //Reflection coefficient are calculated

        public void RCDrop_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            AcImpWell = dropped as WellLog;

            if (AcImpWell == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            RCText.Text = AcImpWell.Name;

        }

        //Velocity is calculated below from porosity

        public void porDrop3_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            porWell2 = dropped as WellLog;

            if (porWell2 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            porText3.Text = porWell2.Name;

        }

        public void swDrop4_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            swWell4 = dropped as WellLog;

            if (swWell4 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                swText4.Text = swWell4.Name;
            }
        }

        public void vshDrop4_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell4 = dropped as WellLog;
            if (vshWell4 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vshText4.Text = vshWell4.Name;
            }
        }
        public void SonicWater3_Scroll(object sender, EventArgs e)
        {

            SWTxt3.Text = SonicWater3.Value.ToString();
        }

        public void SonicHydrocarbon3_Scroll(object sender, EventArgs e)
        {
            SHTxt3.Text = SonicHydrocarbon3.Value.ToString();

        }

        public void SonicShale3_Scroll(object sender, EventArgs e)
        {
            SSTxt3.Text = SonicShale3.Value.ToString();

        }

        public void SonicMatrix3_Scroll(object sender, EventArgs e)
        {
            SMTxt3.Text = SonicMatrix3.Value.ToString();
        }

        //Porosity is calculated by Hunt-Raymer method

        public void SonDrop2_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            SdtWell2 = dropped as WellLog;

            if (SdtWell2 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            SdtText2.Text = SdtWell2.Name;

        }


        public void vshDrop5_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell5 = dropped as WellLog;
            if (vshWell5 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                vshText5.Text = vshWell5.Name;
            }

        }

        public void SonicWater2_Scroll(object sender, EventArgs e)
        {

            SWTxt2.Text = SonicWater2.Value.ToString();
        }


        public void SonicShale2_Scroll(object sender, EventArgs e)
        {
            SSTxt2.Text = SonicShale2.Value.ToString();

        }

        public void SonicMatrix2_Scroll(object sender, EventArgs e)
        {
            SMTxt2.Text = SonicMatrix2.Value.ToString();
        }

        //Porosity from gamma ray log is calculated here

        public void GrDrop1_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            NporWell = dropped as WellLog;

            if (NporWell == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            GrText1.Text = NporWell.Name;

        }


        public void vshDrop7_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell7 = dropped as WellLog;
            if (vshWell7 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                vshText7.Text = vshWell7.Name;
            }

        }

        public void NDShale_Scroll(object sender, EventArgs e)
        {

            NDSText.Text = ((float)NDShale.Value / 100).ToString();
        }

        //Velocity is calculated from Wyllie Method

        public void porDrop4_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            porWell3 = dropped as WellLog;

            if (porWell3 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            porText5.Text = porWell3.Name;

        }

        public void swDrop6_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            swWell6 = dropped as WellLog;

            if (swWell6 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                swText6.Text = swWell6.Name;
            }
        }

        public void vshDrop6_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell6 = dropped as WellLog;
            if (vshWell6 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vshText6.Text = vshWell6.Name;
            }
        }
        public void SonicWater4_Scroll(object sender, EventArgs e)
        {

            SWTxt4.Text = SonicWater4.Value.ToString();
        }

        public void SonicHydrocarbon4_Scroll(object sender, EventArgs e)
        {
            SHTxt4.Text = SonicHydrocarbon4.Value.ToString();

        }

        public void SonicShale4_Scroll(object sender, EventArgs e)
        {
            SSTxt4.Text = SonicShale4.Value.ToString();

        }

        public void SonicMatrix4_Scroll(object sender, EventArgs e)
        {
            SMTxt4.Text = SonicMatrix4.Value.ToString();
        }

        public void ComboLog2Scroll_Scroll(object sender, ScrollEventArgs e)
        {
            //this.comboLogs2.LocationChanged = ComboLog2Scroll.Value;

        }
        // Velocity is calculated from Gardner's equation

        public void DensDrop3_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            DensWell3 = dropped as WellLog;

            if (DensWell3 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            DensText3.Text = DensWell3.Name;

        }
        public void GardCoef3_Scroll(object sender, EventArgs e)
        {
            GCText3.Text = ((float)GardCoef3.Value / 100).ToString();
        }

        public void GardExp3_Scroll(object sender, EventArgs e)
        {
            GEText3.Text = ((float)GardExp3.Value / 100).ToString();
        }


        //Shale volume is calculated from here

        public void GrDrop_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            GrWell = dropped as WellLog;
            if (GrWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");

            }
            else
            {
                e.Effect = DragDropEffects.All;
                GrText.Text = GrWell.Name;
            }

        }

        public void SpDrop_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            SpWell = dropped as WellLog;
            if (SpWell == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");

            }
            else
            {
                e.Effect = DragDropEffects.All;
                SPText.Text = SpWell.Name;
            }

        }

        public void ShaleGR0_Scroll(object sender, EventArgs e)
        {

            SGr0Text.Text = ShaleGR0.Value.ToString();
        }

        public void ShaleGR100_Scroll(object sender, EventArgs e)
        {

            SGr100Text.Text = ShaleGR100.Value.ToString();
        }
        public void SP0Shale_Scroll(object sender, EventArgs e)
        {

            SpS0Text.Text = SPShale0.Value.ToString();
        }

        public void SP100Shale_Scroll(object sender, EventArgs e)
        {

            SpS100Text.Text = SPShale100.Value.ToString();
        }

        //Impedance is from Gardners relation is calculate here

        public void SonDrop3_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            SdtWell3 = dropped as WellLog;

            if (SdtWell3 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }

            sonText3.Text = SdtWell3.Name;
        }


        public void GardCoef2_Scroll(object sender, EventArgs e)
        {
            GCText2.Text = ((float)GardCoef2.Value / 100).ToString();
        }

        public void GardExp2_Scroll(object sender, EventArgs e)
        {
            GEText2.Text = ((float)GardExp2.Value / 100).ToString();
        }

        public void DensDrop_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            DensWell2 = dropped as WellLog;
            if (DensWell2 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                DensText2.Text = DensWell2.Name;
            }

        }

        public void PSVelDrop2_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            PSVelWell2 = dropped as WellLog;
            if (PSVelWell2 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                PSVText2.Text = PSVelWell2.Name;
            }
        }

        public void DensDrop4_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            DensWell4 = dropped as WellLog;

            if (DensWell4 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }

            DensText4.Text = DensWell4.Name;
        }


        public void GardCoef4_Scroll(object sender, EventArgs e)
        {
            GCText4.Text = ((float)GardCoef4.Value / 100).ToString();
        }

        public void GardExp4_Scroll(object sender, EventArgs e)
        {
            GEText4.Text = ((float)GardExp4.Value / 100).ToString();
        }

        //Calculating Elastic constants from here

        //Poison Ratio1

        public void vshDrop8_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vshWell8 = dropped as WellLog;
            if (vshWell8 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vshText8.Text = vshWell8.Name;
            }

        }

        //Poison Ratio2

        public void vpvsDrop1_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            vpvsWell1 = dropped as WellLog;
            if (vpvsWell1 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                vpvsText1.Text = vpvsWell1.Name;
            }

        }
        //Shear Modulus

        public void DensDrop5_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            DensWell5 = dropped as WellLog;

            if (DensWell5 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            DensText5.Text = DensWell5.Name;

        }


        public void sonDrop4_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            SdtWell4 = dropped as WellLog;
            if (SdtWell4 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                sonText4.Text = SdtWell4.Name;
            }

        }

        public void poisDrop1_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            poisWell1 = dropped as WellLog;
            if (poisWell1 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                poisText1.Text = poisWell1.Name;
            }

        }

        //Bulk Modulus
        public void DensDrop6_DragDrop(object sender, DragEventArgs e)
        {

            object dropped = e.Data.GetData(typeof(object));
            //PetrelLogger.InfoBox(dropped.ToString());
            DensWell6 = dropped as WellLog;

            if (DensWell6 == null)
            {

                e.Effect = DragDropEffects.None;
                //porText1.Text = porWell.Name;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                return;

            }


            e.Effect = DragDropEffects.All;
            DensText6.Text = DensWell6.Name;

        }


        public void sonDrop5_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            SdtWell5 = dropped as WellLog;
            if (SdtWell5 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //swText.Text = swWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                //PetrelLogger.InfoBox("Something's is IN");
                sonText5.Text = SdtWell5.Name;
            }

        }

        public void poisDrop2_DragDrop(object sender, DragEventArgs e)
        {
            object dropped = e.Data.GetData(typeof(object));
            poisWell2 = dropped as WellLog;
            if (poisWell2 == null)
            {
                e.Effect = DragDropEffects.None;
                PetrelLogger.InfoBox("Error:This is not a Well Log");
                //vshText.Text = vshWell.Name;
            }
            else
            {
                e.Effect = DragDropEffects.All;
                poisText2.Text = poisWell2.Name;
            }

        }

        public void Doc_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Well_Log_Analysis.chm");
                sw.Close();
                FileStream fs = new FileStream("C:\\Well_Log_Analysis.chm", FileMode.Open, FileAccess.Write);
                Stream str;

                fs.Write(global::WellReader.Resource_chracterizer.Well_Log_Analysis, 0, global::WellReader.Resource_chracterizer.Well_Log_Analysis.Length);
                fs.Close();

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\Well_Log_Analysis.chm";
                p.Start();
            }
            catch
            {
                PetrelLogger.InfoOutputWindow("Unable to Read to your drive -check permissions");
            };
 
        }

        
       

    }
}

