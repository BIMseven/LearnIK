using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public abstract class EyeTrackingHeadset : MonoBehaviour
    {
//-------------------------------------------------------EyeTrackingHeadset METHODS:

        // side must be left or right
        public abstract Camera GetCamera( Sides side );

        public abstract Ray GetGazeConvergence();

        public abstract Ray GetLeftEyeRay();

        public abstract Ray GetRightEyeRay();

        public abstract Vector3 GetLeftEyeVector();

        public abstract Vector3 GetRightEyeVector();

        public abstract Vector3 GetHMDPosition();

        public abstract Quaternion GetHMDRotation();

        public abstract bool IsEyeTrackingCalibrated();

        public abstract bool IsHardwareConnected();

        public abstract bool IsHardwareReady();

        public abstract void TarePosition();

        public abstract void TareOrientation();

        // side must be left or right
        public abstract Vector3 WorldToViewportPoint( Vector3 pos, Sides eye );

    }
}

//TODO:
    // eyes closed
    