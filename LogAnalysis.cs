using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace LogAnalysis
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(LogAnalysisAppearance))]
    public class LogAnalysis : IModule
    {
        public LogAnalysis()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            DataManager.WorkspaceOpened += this.WorkspaceOpened;
            DataManager.WorkspaceClosing += this.WorkspaceClosing;
            DataManager.WorkspaceClosed += this.WorkspaceClosed;

            // TODO:  Add LogAnalysis.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Registrations:
            Workstep1 workstep1Instance = new Workstep1();
            PetrelSystem.WorkflowEditor.AddUIFactory<Workstep1.Arguments>(new Workstep1.UIFactory());
            PetrelSystem.WorkflowEditor.Add(workstep1Instance);
            PetrelSystem.ProcessDiagram.Add(new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(workstep1Instance), "Plug-ins");



            // TODO:  Add LogAnalysis.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {
            // Registrations:


            // TODO:  Add LogAnalysis.IntegratePresentation implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time the petrel creates or loads a project.
        /// </summary>
        private void WorkspaceOpened(object sender, System.EventArgs e)
        {
            // TODO:  Add LogAnalysis.WorkspaceOpened implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time before the petrel closes a project.
        /// </summary>
        private void WorkspaceClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO:  Add LogAnalysis.WorkspaceClosing implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time after the petrel closed a project.
        /// </summary>
        private void WorkspaceClosed(object sender, System.EventArgs e)
        {
            // TODO:  Add LogAnalysis.WorkspaceClosed implementation
        }

        /// <summary>
        /// This method called once in the life of the module; 
        /// right before the module is unloaded. 
        /// It is usually when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            DataManager.WorkspaceOpened -= this.WorkspaceOpened;
            DataManager.WorkspaceClosing -= this.WorkspaceClosing;
            DataManager.WorkspaceClosed -= this.WorkspaceClosed;

            // TODO:  Add LogAnalysis.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add LogAnalysis.Dispose implementation
        }

        #endregion

    }

    #region ModuleAppearance Class

    /// <summary>
    /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
    /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
    /// </summary>
    internal class LogAnalysisAppearance : IModuleAppearance
    {
        /// <summary>
        /// Description of the module.
        /// </summary>
        public string Description
        {
            get { return "LogAnalysis"; }
        }

        /// <summary>
        /// Display name for the module.
        /// </summary>
        public string DisplayName
        {
            get { return "LogAnalysis"; }
        }

        /// <summary>
        /// Returns the name of a image resource.
        /// </summary>
        public string ImageResourceName
        {
            get { return null; }
        }

        /// <summary>
        /// A link to the publisher or null.
        /// </summary>
        public Uri ModuleUri
        {
            get { return null; }
        }
    }

    #endregion
}