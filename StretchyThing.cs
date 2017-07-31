using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class StretchyThing : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "StretchyThing";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	        
    public Vector3 Origin { get; private set; }
    public Vector3 Target { get; private set; }

    // Else pivot assumed to be at center
    public bool PivotAtOrigin = false;

    private float unscaledLength = 1;

//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        unscaledLength = findUnscaledLength();
    }
		
//--------------------------------------------------------------------------METHODS:

    public void Stretch( Vector3 origin, Vector3 target )
    {
        Stretch( origin, target, Vector3.up );
    }

    public void Stretch( Vector3 origin, Vector3 target, Vector3 upwards )
    {
        Origin = origin;
        Target = target;
        // set position to from if not anchored at joint
        transform.position = origin;
        // look at target
        lookAtTarget( target, upwards );
        Vector3 toTarget = target - origin;
        // move halfway to target if not anchored at joint
        if( ! PivotAtOrigin )
        {
            transform.position = transform.position + 0.5f * toTarget;
        }
        // scale
        float desiredLength = toTarget.magnitude;
        float desiredScale = desiredLength / unscaledLength;
        Vector3 scale = transform.localScale;
        scale.z = desiredScale;
        if( float.IsNaN( scale.z ) || float.IsInfinity( scale.z ) )
        {
            scale = Vector3.zero;
        }
        transform.localScale = scale;
    }

    public void UpdateTarget( Vector3 target )
    {
        Stretch( Origin, target );
        Target = target;
    }

//--------------------------------------------------------------------------HELPERS:
	
    private float findUnscaledLength()
    {
        Bounds unscaledBounds = transform.UnscaledAndUnrotatedBounds();
        if( float.IsNaN( unscaledBounds.extents.z ) ||
            float.IsInfinity( unscaledBounds.extents.z ) ||
            unscaledBounds.extents.z == 0 )
        {
            LOG_TAG.TPrint( "Unable to find unscaled length for " + name );
            return 1;
        }
        return unscaledBounds.extents.z * 2;
    }

    private void lookAtTarget( Vector3 targetPosition, Vector3 upwards )
    {
        Vector3 relativePos = targetPosition - transform.position;
        if( ! relativePos.magnitude.NearlyEquals( 0 ) )
        {
            transform.rotation = Quaternion.LookRotation( relativePos, upwards ); 
        }
    }
}