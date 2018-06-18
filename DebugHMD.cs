using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class DebugHMD : EyeTrackingHeadset
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "DebugHMD";
	public bool VERBOSE = false;

    private const float REALLY_FAR = 1000;

//---------------------------------------------------------------------------FIELDS:

    [SerializeField]
    private Camera leftCam;
    [SerializeField]
    private Camera rightCam;

    [SerializeField]
    public Transform leftEyeGaze;
    [SerializeField]
    public Transform rightEyeGaze;
    
//--------------------------------------------------------------------------METHODS:

//-------------------------------------------------------EyeTrackingHeadset METHODS:

    public override Camera GetCamera( Sides side )
    {
        if( side == Sides.Left )    return leftCam;

        return rightCam;
    }

    public override Ray GetGazeConvergence()
    {
        return forwardRay( transform );
    }

    public override Vector3 GetHMDPosition()
    {
        return transform.position;
    }

    public override Quaternion GetHMDRotation()
    {
        return transform.rotation;
    }

    public override Ray GetLeftEyeRay()
    {
        return forwardRay( leftEyeGaze );
    }

    public override Ray GetRightEyeRay()
    {
        return forwardRay( rightEyeGaze );
    }

    public override Vector3 GetLeftEyeVector()
    {
        return rightEyeGaze.forward;
    }

    public override Vector3 GetRightEyeVector()
    {
        return leftEyeGaze.forward;
    }

    public override bool IsEyeTrackingCalibrated()
    {
        vLog( "IsEyeTrackingCalibrated" );
        return true;
    }

    public override bool IsHardwareConnected()
    {
        vLog( "IsHardwareConnected" );
        return true;
    }

    public override bool IsHardwareReady()
    {
        vLog( "IsHardwareReady" );
        return true;
    }

    public override void TareOrientation()
    {
        vLog( "TareOrientation" );
    }

    public override void TarePosition()
    {
        vLog( "TarePosition" );
    }

    public override Vector3 WorldToViewportPoint( Vector3 pos, Sides eye )
    {
        if( eye == Sides.Left )
        {
            return leftCam.WorldToViewportPoint( pos );
        }
        return rightCam.WorldToViewportPoint( pos );
    }
    
//--------------------------------------------------------------------------HELPERS:

    private Ray forwardRay( Transform trans )
    {
        return new Ray( trans.position, trans.forward * REALLY_FAR );
    }

    private void vLog( string message )
    {
        if( VERBOSE )    LOG_TAG.TPrint( message );
    }
}
