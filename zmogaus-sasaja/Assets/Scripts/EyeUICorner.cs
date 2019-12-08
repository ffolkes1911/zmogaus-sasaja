/*
 * Copyright (c) 2013-present, The Eye Tribe. 
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree. 
 *
 */

using System;
using System.Collections;
using UnityEngine;
using EyeTribe.ClientSdk;
using EyeTribe.ClientSdk.Data;
using EyeTribe.Unity;

namespace EyeTribe.Unity.Calibration
{
    /// <summary>
    /// Handles the UI associated to displaying eye pupils
    /// </summary>
    public class EyeUICorner : MonoBehaviour
    {
        private Camera _Camera;

        private GameObject _LeftEye;
        private GameObject _RightEye;

        [SerializeField] private float _EyeScaleInitSize = 1f;
        [SerializeField] private float _ResizeMultiplier = 1f;

        private float xScreen;
        private float yScreen;

        private double _EyesDistance;
        private Vector3 _EyeBaseScale;
        private double _DepthMod;

        private Eye _LastLeftEye;
        private Eye _LastRightEye;

        private bool _IsOff;

        void Awake()
        {
            _Camera = gameObject.GetComponentInParent<Camera>();
            _LeftEye = GameObject.FindGameObjectWithTag("leftEye");
            _RightEye = GameObject.FindGameObjectWithTag("rightEye");

            if (null == _Camera)
                throw new Exception("_Camera is not set!");

            if (null == _LeftEye)
                throw new Exception("_LeftEye is not set!");

            if (null == _RightEye)
                throw new Exception("_RightEye is not set!");

            xScreen = Screen.width;
            yScreen = Screen.height;
        }

        void OnEnable()
        {
            // Only use in 'remote' mode
            if (VRMode.IsRunningInVRMode)
            {
                gameObject.SetActive(false);
            }

            _EyeBaseScale = _LeftEye.transform.localScale;
            TurnOn();
        }

        public void TurnOff()
        {
            _IsOff = true;
            _LeftEye.SetRendererEnabled(false);
            _RightEye.SetRendererEnabled(false);
        }

        public void TurnOn()
        {
            _IsOff = false;
            _LeftEye.SetRendererEnabled(true);
            _RightEye.SetRendererEnabled(true);
        }
        
        void Update()
        {
            if (!Application.isPlaying)
                return;

            if (!VRMode.IsRunningInVRMode && !_IsOff)
            {
                if (!GazeManager.Instance.IsCalibrating)
                {
                    // If running in 'remote' mode and not calibrating, we position eyes
                    // and set size based on distance

                    _EyesDistance = GazeFrameCache.Instance.GetLastUserPosition().Z;

                    _DepthMod = (1 - _EyesDistance) * .25f;
                    Vector3 scaleVec = new Vector3((float)(_DepthMod), (float)(_DepthMod), (float)_EyeBaseScale.z);

                    Eye left = GazeFrameCache.Instance.GetLastLeftEye();
                    Eye right = GazeFrameCache.Instance.GetLastRightEye();

                    double angle = GazeFrameCache.Instance.GetLastEyesAngle();

                    if (null != left)
                    {
                        if (!left.Equals(_LastLeftEye))
                        {
                            _LastLeftEye = left;

                            if (!_LeftEye.IsRendererEnabled())
                                _LeftEye.SetRendererEnabled(true);

                            //position GO based on screen coordinates
                            Point2D gp = UnityGazeUtils.GetRelativeToScreenSpace(left.PupilCenterCoordinates);

                            ////////////////
                            // normalize coordinates to top right corner
                            //float xMin = 0, xMax = Screen.width, yMin = 0, yMax = Screen.height;
                            float xMin = 0, xMax = xScreen, yMin = 0, yMax = yScreen;
                            float xOffset = xMax * (1 - _ResizeMultiplier), yOffset = yMax * (1 - _ResizeMultiplier);
                            gp.X = (((gp.X - xMin) / (xMax - xMin)) * _ResizeMultiplier * xMax + xOffset);
                            gp.Y = (((gp.Y - yMin) / (yMax - yMin)) * _ResizeMultiplier * yMax + yOffset) - yMax * (1 - _ResizeMultiplier);
                            ////////////////

                            _LeftEye.SetWorldPositionFromGaze(_Camera, gp, _LeftEye.transform.localPosition.z);
                            _LeftEye.transform.localScale = scaleVec * _EyeScaleInitSize;
                            _LeftEye.transform.localEulerAngles = new Vector3(_LeftEye.transform.localEulerAngles.x, _LeftEye.transform.localEulerAngles.y, (float)-angle);
                        }
                    }
                    else
                    {
                        if (_LeftEye.IsRendererEnabled())
                            _LeftEye.SetRendererEnabled(false);
                    }

                    if (null != right)
                    {
                        if (!right.Equals(_LastRightEye))
                        {
                            _LastRightEye = right;

                            if (!_RightEye.IsRendererEnabled())
                                _RightEye.SetRendererEnabled(true);

                            //position GO based on screen coordinates
                            Point2D gp = UnityGazeUtils.GetRelativeToScreenSpace(right.PupilCenterCoordinates);

                            ////////////////
                            // normalize coordinates to top right corner
                            //float xMin = 0, xMax = Screen.width, yMin = 0, yMax = Screen.height;
                            float xMin = 0, xMax = xScreen, yMin = 0, yMax = yScreen;
                            float xOffset = xMax * (1 - _ResizeMultiplier), yOffset = yMax * (1 - _ResizeMultiplier);
                            gp.X = (((gp.X - xMin) / (xMax - xMin)) * _ResizeMultiplier * xMax + xOffset);
                            gp.Y = (((gp.Y - yMin) / (yMax - yMin)) * _ResizeMultiplier * yMax + yOffset) - yMax * (1 - _ResizeMultiplier);
                            ////////////////

                            _RightEye.SetWorldPositionFromGaze(_Camera, gp, _RightEye.transform.localPosition.z);
                            _RightEye.transform.localScale = scaleVec * _EyeScaleInitSize;
                            _RightEye.transform.localEulerAngles = new Vector3(_RightEye.transform.localEulerAngles.x, _RightEye.transform.localEulerAngles.y, (float)-angle);
                        }
                    }
                    else
                    {
                        if (_RightEye.IsRendererEnabled())
                            _RightEye.SetRendererEnabled(false);
                    }
                }
            }
            else
            {
                if (_LeftEye.IsRendererEnabled())
                    _LeftEye.SetRendererEnabled(false);
                if (_RightEye.IsRendererEnabled())
                    _RightEye.SetRendererEnabled(false);
            }
        }
    }
}
