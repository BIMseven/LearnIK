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
    public AxisSwapper LeftElbow;
    public AxisSwapper LeftShoulder;
    public AxisSwapper RightElbow;
    public AxisSwapper RightShoulder;

    public AxisSwapper LeftTargetForearm;
    public AxisSwapper RightTargetForearm;
    public Transform LeftTargetHand;
    public Transform RightTargetHand;
    #endregion

    private float shoulderToElbowDistance;
    private float elbowToWristDistance;

//---------------------------------------------------------------------MONO METHODS:

    void Start()
    {
        //TODO: add safeguards to ensure that the LeapHands don't go further than our avatar's reach;
        if( LeftTargetForearm != null && LeftElbow != null && LeftShoulder != null )
        {
            LeftShoulder.LocalLookRotation( LeftTargetForearm.Forward,
                                            LeftTargetForearm.Up );

            Vector3 armUp = Vector3.Cross( LeftShoulder.Forward, LeftTargetHand.forward );

            LeftElbow.LocalLookRotation( LeftTargetForearm.Forward, armUp );

            elbowToWristDistance = Vector3.Distance( LeftElbow.transform.position,
                                                     LeftTargetForearm.transform.position );
        }

        if( RightTargetForearm != null && RightElbow != null && RightShoulder != null )
        {
            RightShoulder.LocalLookRotation( RightTargetForearm.transform.forward
                                           , RightTargetForearm.transform.up );

            Vector3 armUp = Vector3.Cross( RightTargetForearm.transform.forward , RightShoulder.Forward);

            RightElbow.LocalLookRotation( RightTargetForearm.transform.forward, armUp );
            
            if(elbowToWristDistance == 0)
            {
                elbowToWristDistance = Vector3.Distance( RightElbow.transform.position,
                                                         RightTargetHand.position );
            }
        }
    }

    void Update()
    {
        moveArm( LeftShoulder, LeftElbow, LeftTargetForearm,LeftTargetHand , false );
        moveArm( RightShoulder, RightElbow, RightTargetForearm,RightTargetHand, true );

    }

    //--------------------------------------------------------------------------METHODS:

    //--------------------------------------------------------------------------HELPERS:

    private void moveArm( AxisSwapper upperArm,
                          AxisSwapper forearm,
                          AxisSwapper targetForearm,
                          Transform targetHand, 
                          bool isRightArm )
    {


        var targetElbowPos = targetHand.position - targetForearm.Forward * elbowToWristDistance;
        Vector3 shoulderToElbow = targetElbowPos - upperArm.transform.position;
        Vector3 elbowToTarget = targetHand.position - targetElbowPos;

        // Reversed order if ! left
        Vector3 up;
        if( isRightArm )
        {
            up = Vector3.Cross( elbowToTarget , shoulderToElbow  );
        }
        else
        {
            up = Vector3.Cross( shoulderToElbow , elbowToTarget );
        }

        upperArm.Look( targetElbowPos - upperArm.transform.position, up );
        forearm.Look( elbowToTarget, up );
    }

    private void vLog( string message )
    {
        if( VERBOSE ) LOG_TAG.TPrint( message );
    }
}