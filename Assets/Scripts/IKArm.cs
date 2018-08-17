using UnityEngine;
using System.Collections;
using MyUtility;

public class IKArm : MonoBehaviour
{
    //------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "IKArm";
    public bool VERBOSE = false;

    //---------------------------------------------------------------------------FIELDS:
    #region Inspector Fields
    public AxisSwapper LeftWrist;
    public AxisSwapper LeftElbow;
    public AxisSwapper LeftShoulder;
    #endregion
    //Setup assumes the model's arms are set to default "normal" positions
    //Eventually to deal with non-normal cases, we will need a prefab to revert to.

    
    private float shoulderToElbowDistance;
    private float elbowToWristDistance;
    //---------------------------------------------------------------------MONO METHODS:

    void Start()
    {
        //TODO: add safeguards to ensure that the LeapHands don't go further than our avatar's reach;
        if( LeftWrist != null && LeftElbow != null && LeftShoulder != null )
        {
            LeftShoulder.LocalLookRotation( LeftWrist.transform.forward, LeftWrist.transform.up );
            LeftElbow.LocalLookRotation( LeftWrist.Forward, Vector3.Cross( LeftShoulder.Forward, LeftWrist.Forward ) );

            shoulderToElbowDistance = ( LeftElbow.transform.position - LeftShoulder.transform.position ).magnitude;
            elbowToWristDistance = Vector3.Distance( LeftElbow.transform.position, LeftWrist.transform.position );

            vLog( "INIT Wrist Local Rot: " + LeftWrist.LocalForward + "INIT Shoulder Local Rot: " + LeftShoulder.LocalForward + "INIT elbow Local Rot: " + LeftElbow.LocalForward );
        }
    }

    void Update()
    {
        if( LeftWrist != null && LeftElbow != null && LeftShoulder != null )
        {
            LeftElbow.transform.position = LeftWrist.transform.position - (( LeftWrist.Forward ) * elbowToWristDistance);

            LeftShoulder.LookAt( LeftElbow.transform );

            LeftElbow.LookAt( LeftWrist.transform , Vector3.Cross( LeftShoulder.Forward, LeftWrist.Forward ) );


        }
    }

    //--------------------------------------------------------------------------METHODS:

    //--------------------------------------------------------------------------HELPERS:

    private void vLog( string message )
    {
        if( VERBOSE ) LOG_TAG.TPrint( message );
    }
}