using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


namespace EyeTribe.Unity.Interaction
{

    public abstract class InteractionHandler : MonoBehaviour
    {

        protected VRInteractiveItem InteractiveItem;

        public virtual void Start()
        {
            InteractiveItem = gameObject.GetComponent<VRInteractiveItem>();
            InteractiveItem.OnOver += HandleIn;
            InteractiveItem.OnOut += HandleOut;

            SelectionRadialEyeTribe.OnSelectionStarted += SelectionStarted;
            SelectionRadialEyeTribe.OnSelectionAborted += SelectionCanceled;
            SelectionRadialEyeTribe.OnSelectionComplete += SelectionCompleted;
        }

        public abstract void HandleIn();
        public abstract void HandleOut();
        public abstract void SelectionStarted();
        public abstract void SelectionCanceled();
        public abstract void SelectionCompleted();
    }
}
