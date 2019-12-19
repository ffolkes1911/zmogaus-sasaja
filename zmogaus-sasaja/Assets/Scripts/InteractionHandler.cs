using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


namespace EyeTribe.Unity.Interaction
{

    public abstract class InteractionHandler : MonoBehaviour
    {

        protected VRInteractiveItem InteractiveItem;
        protected bool disabled = false;

        public virtual void Awake()
        {
            InteractiveItem = gameObject.GetComponentInChildren<VRInteractiveItem>();
            InteractiveItem.OnOver += HandleIn;
            InteractiveItem.OnOut += HandleOut;

            SelectionRadialEyeTribe.OnSelectionStarted += SelectionStarted;
            SelectionRadialEyeTribe.OnSelectionAborted += SelectionCanceled;
            SelectionRadialEyeTribe.OnSelectionComplete += SelectionCompleted;
        }

        public virtual void Disable()
        {
            if (!disabled)
            {
                disabled = true;
                Debug.Log("ON DISABLE");
            }
        }

        public virtual void Enable()
        {
            if (disabled)
            {
                disabled = false;
                Debug.Log("ON ENABLE");
            }
        }

        public abstract void HandleIn();
        public abstract void HandleOut();
        public abstract void SelectionStarted();
        public abstract void SelectionCanceled();
        public abstract void SelectionCompleted();
    }
}
