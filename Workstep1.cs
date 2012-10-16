using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace LogAnalysis
{
    /// <summary>
    /// This class contains all the methods and subclasses of the Workstep1.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    public class Workstep1 : Workstep<Workstep1.Arguments>, IExecutorSource, IPresentation, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>
        protected override Workstep1.Arguments CreateArgumentPackageCore()
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
        /// ArgumentPackage class for Workstep1.
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
        /// Gets the description of the Workstep1
        /// </summary>
        public IDescription Description
        {
            get { return Workstep1Description.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the Workstep1.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class Workstep1Description : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private  static Workstep1Description instance = new Workstep1Description();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static Workstep1Description Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of Workstep1
            /// </summary>
            public string Name
            {
                get { return "Well Log Analysis"; }
            }
            /// <summary>
            /// Gets the short description of Workstep1
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of Workstep1
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion

        public class UIFactory : WorkflowEditorUIFactory
        {
            /// <summary>
            /// This method creates the dialog UI for the given workstep, arguments
            /// and context.
            /// </summary>
            /// <param name="workstep">the workstep instance</param>
            /// <param name="argumentPackage">the arguments to pass to the UI</param>
            /// <param name="context">the underlying context in which the UI is being used</param>
            /// <returns>a Windows.Forms.Control to edit the argument package with</returns>
            protected override System.Windows.Forms.Control CreateDialogUICore(Workstep workstep, object argumentPackage, WorkflowContext context)
            {
                return new Workstep1UI((Workstep1)workstep, (Arguments)argumentPackage, context);
            }
        }
    }
}