using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System;

namespace EyeTribe.Unity.Interaction
{
    public class interactive : MonoBehaviour
    {

        /// <summary>
        /// Handles the process of interpolating a Renderer between 2 color states using coroutines.
        /// </summary>
        [SerializeField] private VRInteractiveItem InteractiveItem;
        private InteractionHandler handler;
        public Button button;

        private float temp;

        public void Start()
        {
            if (null == InteractiveItem)
                throw new Exception("InteractiveItem is not set!");

            button = gameObject.GetComponent<Button>();
            handler = gameObject.GetComponent<InteractionHandler>();
            temp = button.transform.localScale.y;

            InteractiveItem.OnOver += handler.HandleIn;
            InteractiveItem.OnOut += handler.HandleOut;

            SelectionRadialEyeTribe.OnSelectionStarted += handler.SelectionStarted;
            SelectionRadialEyeTribe.OnSelectionAborted += handler.SelectionCanceled;
            SelectionRadialEyeTribe.OnSelectionComplete += handler.SelectionCompleted;
        }

        void OnDisable()
        {
            InteractiveItem.OnOver -= handler.HandleIn;
            InteractiveItem.OnOut -= handler.HandleOut;

            SelectionRadialEyeTribe.OnSelectionStarted -= handler.SelectionStarted;
            SelectionRadialEyeTribe.OnSelectionAborted -= handler.SelectionCanceled;
            SelectionRadialEyeTribe.OnSelectionComplete -= handler.SelectionCompleted;
        }
    }
}