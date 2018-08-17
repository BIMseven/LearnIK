using UnityEngine;
using System.Collections;
using System;
using MyUtility;
//using MyLeapUtility;
//using MyViveUtility;

public class IKHeadMover : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "IKHeadMover";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
    [SerializeField]
    AxisSwapper myHead;

    [SerializeField]
    Transform HeadTarget;

    [SerializeField]
    Transform LeftHandTarget;

    [SerializeField]
    Transform RightHandTarget;

    Animator Avatar;

    public float LookWeight = 1f;
    public float RotationWeight = 1f;
    //---------------------------------------------------------------------MONO METHODS:

    private void Awake()
    {
        Avatar = GetComponent<Animator>();
    }

    //private void Update()
    //{
    //    myLeft.transform.rotation = myLeft.LookRotation( LeftHandTarget.transform.rotation );
    //}

    void OnAnimatorIK()
    {
        if( Avatar )
        {
            if( HeadTarget != null )
            {
                var adjustedRot = myHead.LocalLookRotation( HeadTarget.rotation );

                Avatar.SetBoneLocalRotation( HumanBodyBones.Head, adjustedRot );
                //Avatar.SetBoneLocalRotation(HumanBodyBones.Head,adjustedRot);
            }
            if( LeftHandTarget != null )
            {

                //Avatar.SetIKPositionWeight( AvatarIKGoal.LeftHand, LookWeight );
                //Avatar.SetIKRotationWeight( AvatarIKGoal.LeftHand, RotationWeight );
                Avatar.SetIKPositionWeight( AvatarIKGoal.LeftHand, 1 );
                Avatar.SetIKRotationWeight( AvatarIKGoal.LeftHand, 1 );


                Avatar.SetIKPosition( AvatarIKGoal.LeftHand, LeftHandTarget.position );
                Avatar.SetIKRotation( AvatarIKGoal.LeftHand,LeftHandTarget.rotation  );
            }
            if( RightHandTarget != null )
            {
                Avatar.SetIKPositionWeight( AvatarIKGoal.RightHand, 1 );
                Avatar.SetIKRotationWeight( AvatarIKGoal.RightHand, 1 );
                //Avatar.SetIKPositionWeight( AvatarIKGoal.RightHand, LookWeight );
                //Avatar.SetIKRotationWeight( AvatarIKGoal.RightHand, RotationWeight );

                Avatar.SetIKPosition( AvatarIKGoal.RightHand, RightHandTarget.position );
                Avatar.SetIKRotation( AvatarIKGoal.RightHand, RightHandTarget.rotation );
            }
        }
    }


    //--------------------------------------------------------------------------METHODS:

}