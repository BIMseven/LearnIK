using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public interface EyeTrackingHeadset
    {
//-------------------------------------------------------EyeTrackingHeadset METHODS:

        // side must be left or right
        Camera GetCamera( Sides side );

        Ray GetGazeConvergence();

        Ray GetLeftEyeRay();

        Ray GetRightEyeRay();

        Vector3 GetLeftEyeVector();

        Vector3 GetRightEyeVector();

        Vector3 GetHMDPosition();

        Quaternion GetHMDRotation();

        bool IsEyeTrackingCalibrated();

        bool IsHardwareConnected();

        bool IsHardwareReady();

        void TarePosition();

        void TareOrientation();

        // side must be left or right
        Vector3 WorldToViewportPoint( Vector3 pos, Sides eye );

    }
}

//TODO:
    // eyes closed
    