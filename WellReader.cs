using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.UI.Tools;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Basics;

namespace WellReader
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(WellReaderAppearance))]
    public class WellReader : IModule
    {
        SimpleContextMenuHandler<SeismicCube> cm;
        public WellReader()
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

            // TODO:  Add WellReader.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Registrations:
            WellLogReader welllogreaderInstance = new WellLogReader();
            PetrelSystem.WorkflowEditor.AddUIFactory<WellLogReader.Arguments>(new WellLogReader.UIFactory());
            PetrelSystem.WorkflowEditor.Add(welllogreaderInstance);
            PetrelSystem.ProcessDiagram.Add(new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(welllogreaderInstance), "Plug-ins");
            // TODO:  Add WellReader.Integrate implementation
        }
        private void callback(object sender, ContextMenuClickedEventArgs<SeismicCube> clickedCube)
        {
            try
            {
                SeismicCube getcube = ((SeismicCube)clickedCube.ContextObject);
                Index3 start = new Index3(0, 0, 0);
                Index3 end = new Index3(getcube.NumSamplesIJK.I - 1, getcube.NumSamplesIJK.J - 1, getcube.NumSamplesIJK.K - 1);
                ISubCube from = getcube.GetSubCube(start, end);
                float[, ,] vals = from.ToArray();

                new Plot3D.Plot3DMainForm(vals).Show();
            }
            catch { }

        }
        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {
            // Registrations:

            GeoCommunication.Discuss.AddNewMenuItemsUnderDiscussMenuItem();
            cm = new SimpleContextMenuHandler<SeismicCube>("GeoCom 3D Visualizer", callback);
            PetrelSystem.ToolService.AddContextMenuHandler(cm);
            
            // TODO:  Add WellReader.IntegratePresentation implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time the petrel creates or loads a project.
        /// </summary>
        private void WorkspaceOpened(object sender, System.EventArgs e)
        {
            // TODO:  Add WellReader.WorkspaceOpened implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time before the petrel closes a project.
        /// </summary>
        private void WorkspaceClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO:  Add WellReader.WorkspaceClosing implementation
        }

        /// <summary>
        /// This method is not part of the IModule interface.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and called every time after the petrel closed a project.
        /// </summary>
        private void WorkspaceClosed(object sender, System.EventArgs e)
        {
            // TODO:  Add WellReader.WorkspaceClosed implementation
        }

        /// <summary>
        /// This method called once in the life of the module; 
        /// right before the module is unloaded. 
        /// It is usually when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            PetrelSystem.ToolService.RemoveContextMenuHandler(cm);
            DataManager.WorkspaceOpened -= this.WorkspaceOpened;
            DataManager.WorkspaceClosing -= this.WorkspaceClosing;
            DataManager.WorkspaceClosed -= this.WorkspaceClosed;

            // TODO:  Add WellReader.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add WellReader.Dispose implementation
        }

        #endregion

    }

    #region ModuleAppearance Class

    /// <summary>
    /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
    /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
    /// </summary>
    internal class WellReaderAppearance : IModuleAppearance
    {
        /// <summary>
        /// Description of the module.
        /// </summary>
        public string Description
        {
            get { return "WellReader"; }
        }

        /// <summary>
        /// Display name for the module.
        /// </summary>
        public string DisplayName
        {
            get { return "WellReader"; }
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