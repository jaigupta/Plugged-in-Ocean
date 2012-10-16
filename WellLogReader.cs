using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace WellReader
{
    /// <summary>
    /// This class contains all the methods and subclasses of the WellLogReader.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    public class WellLogReader : Workstep<WellLogReader.Arguments>, IExecutorSource, IPresentation, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>
        protected override WellLogReader.Arguments CreateArgumentPackageCore()
        {
            return new Arguments();
        }
        /// <summary>
        /// Copies the Arguments instance.
        /// </summary>
        /// <param name="fromArgumentPackage">the source Arguments instance</param>
        /// <param name="toArgumentPackage">the target Arguments instance</param>
        protected override void CopyArgumentPackageCore(Arguments fromArgumentPackage, Arguments toArgumentPackage)
        {
            DescribedArgumentsHelper.Copy(fromArgumentPackage, toArgumentPackage);
        }

        #endregion

        #region IExecutorSource Members and Executor class

        /// <summary>
        /// Creates the Executor instance for this workstep. This class will do the work of the Workstep.
        /// </summary>
        /// <param name="argumentPackage">the argumentpackage to pass to the Executor</param>
        /// <param name="workflowRuntimeContext">the context to pass to the Executor</param>
        /// <returns>The Executor instance.</returns>
        public Slb.Ocean.Petrel.Workflow.Executor GetExecutor(object argumentPackage, WorkflowRuntimeContext workflowRuntimeContext)
        {
            return new Executor(argumentPackage as Arguments, workflowRuntimeContext);
        }

        public class Executor : Slb.Ocean.Petrel.Workflow.Executor
        {
            Arguments arguments;
            WorkflowRuntimeContext context;

            public Executor(Arguments arguments, WorkflowRuntimeContext context)
            {
                this.arguments = arguments;
                this.context = context;
            }


            public override void ExecuteSimple()
            {
                // TODO: Implement the workstep logic here.
            }
        }

        #endregion

        /// <summary>
        /// ArgumentPackage class for WellLogReader.
        /// Each public property is an argument in the package.  The name, type and
        /// input/output role are taken from the property and modified by any
        /// attributes applied.
        /// </summary>
        public class Arguments : DescribedArgumentsByReflection
        {


        }
    
        #region IPresentation Members

        public event EventHandler PresentationChanged;

        public string Text
        {
            get { return Description.Name; }
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Modules; }
        }

        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the WellLogReader
        /// </summary>
        public IDescription Description
        {
            get { return WellLogReaderDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the WellLogReader.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class WellLogReaderDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private  static WellLogReaderDescription instance = new WellLogReaderDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static WellLogReaderDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of WellLogReader
            /// </summary>
            public string Name
            {
                get { return "GeoCom Plugin"; }
            }
            /// <summary>
            /// Gets the short description of WellLogReader
            /// </summary>
            public string ShortDescription
            {
                get { return "Plug-in for Well and Reservoir Characterization"; }
            }
            /// <summary>
            /// Gets the detailed description of WellLogReader
            /// </summary>
            public string Description
            {
                get { return "Plug-in for Well and Reservoir Characterization. It converts acoustic properties to petrophysical properties. It performs characterization of wells as well as reservoirs. Taking sonic and density data as input, it designs Rock physics models and iterates them to determine porosity, clay content and water saturation variation for the study region"; }
            }

            #endregion
        }
        #endregion

        public class UIFactory : WorkflowEditorUIFactory
        {
            WellLogReaderUI abc;
            private delegate void SetControlPropertyThreadSafeDelegate(System.Windows.Forms.Control control, string propertyName, object propertyValue);

            public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
                }
                else
                {
                    control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
                }
            }
            protected override System.Windows.Forms.Control CreateDialogUICore(Workstep workstep, object argumentPackage, WorkflowContext context)
            {
                abc = new WellLogReaderUI((WellLogReader)workstep, (Arguments)argumentPackage, context);
                new Thread(new ThreadStart(this.setFixSize)).Start();
                return abc;
            }
            public void setFixSize()
            {
                int c = 0;
                while (abc.Parent == null && c < 10)
                {
                    Thread.Sleep(2000);
                    c++;
                }
                if (abc.Parent != null)
                    SetControlPropertyThreadSafe((Form)abc.Parent, "MaximumSize", new System.Drawing.Size(abc.Width + 19, abc.Height + 47));
            }
        }
    }
}