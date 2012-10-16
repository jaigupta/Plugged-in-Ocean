using System;
using Slb.Ocean.Petrel.UI;

namespace WellReader
{
    /// <summary>
    /// Summary description for Reader.
    /// </summary>
    public class Reader : ToggleWindow, IPresentation
    {
        public Reader()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region ToggleWindow Members

        /// <summary>
        /// This method determines whether the window can show any or all instances 
        /// of the given Type.
        /// </summary>
        /// <param name="domainObjType">the Type of domain object to check</param>
        /// <returns>
        /// "All" if all objects of the given type can be shown,
        /// "Some" if only certain instances of the type can be shown,
        /// "None" if no objects of the type can be shown.
        /// </returns>
        protected override ToggleWindowTypeSupport CanShowTypeCore(Type domainObjType)
        {
            return ToggleWindowTypeSupport.Some;
        }

        /// <summary>
        /// This method determines whether the window can show the given object.
        /// </summary>
        /// <param name="domainObj">the object to check</param>
        /// <returns>true when it can be shown, false when not</returns>
        protected override bool CanShowObjectCore(object domainObj)
        {
            if (domainObj == null)
                return false;

            return true;
        }

        /// <summary>
        /// This method will be called when the window should show an object.
        /// It raises the VisibleObjects.Changed event when necessary.
        /// </summary>
        /// <param name="domainObj">the object to show in the window</param>
        protected override void ShowObjectCore(object domainObj)
        {
            if (!CanShowObject(domainObj) || IsVisible(domainObj))
                return;

            // TODO: Add code to show the object in the window

            OnVisibleObjectsChanged(VisibilityChangedEventArgs.FromSingleObject(domainObj, VisibilityState.Visible));
        }

        /// <summary>
        /// This method will be called when the window should hide an object.
        /// It raises the VisibleObjects.Changed event when necessary.
        /// </summary>
        /// <param name="domainObj">the object to hide</param>
        protected override void HideObjectCore(object domainObj)
        {
            if (!IsVisible(domainObj))
                return;

            // TODO: Add code to hide the object

            OnVisibleObjectsChanged(VisibilityChangedEventArgs.FromSingleObject(domainObj, VisibilityState.Hidden));
        }

        /// <summary>
        /// Determines whether or not the given object is currently visualized in the window.
        /// </summary>
        /// <param name="domainObj">the object to check</param>
        /// <returns>true if the object is visible; otherwise false</returns>
        protected override bool IsVisibleCore(object domainObj)
        {
            // TODO: Add implementation to determine if the object is showing

            return false;
        }

        /// <summary>
        /// Gets an enumerator containing all of the objects currently visible in this window
        /// </summary>
        /// <returns></returns>
        protected override System.Collections.IEnumerator GetVisibleObjectsEnumeratorCore()
        {
            // TODO: Add implementation to provide the list of visible objects

            yield break;
        }

        /// <summary>
        /// This method will be called to get the actual Control to host in the Petrel Form.
        /// </summary>
        /// <returns>the Control handling visualization for this window</returns>
        protected override System.Windows.Forms.Control CreateControlCore()
        {
            return new ReaderUI();
        }

        /// <summary>
        /// This method will be called when the window's Control should be disposed.
        /// </summary>
        protected override void DisposeControlCore()
        {
            // TODO: Call Dispose on the Control and/or clean up any additional resources
        }

        #endregion

        #region IPresentation Members

        public event EventHandler PresentationChanged;

        /// <summary>
        /// Gets the text displayed for this window
        /// </summary>
        public string Text
        {
            get { return "Reader name"; }
        }

        /// <summary>
        /// Gets the image displayed for this window
        /// </summary>
        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Window; }
        }

        #endregion
    }
}