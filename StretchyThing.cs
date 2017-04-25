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

    private float unscaledLength = 1;	

//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        unscaledLength = findUnscaledLength();
        print( "Collider: " + GetComponentInChildren<Collider>() );
        print( "unscaled Length: " + unscaledLength );
        if( float.IsNaN( unscaledLength )  ||  unscaledLength == 0 )
        {
            Debug.LogError( "Unable to fin unscaled length for " + name );
        }
    }
		
	void Update()
	{
        //lookAtTarget( Target.position );
        //Stretch( ShoulderJoint.position, ElbowJoint.position );
    }

//--------------------------------------------------------------------------METHODS:

    public void Stretch( Vector3 origin, Vector3 target )
    {
        Origin = origin;
        Target = target;
        // set position to from if not anchored at joint
        transform.position = origin;
        // look at target
        lookAtTarget( target );
        // move halfway to target if not anchored at joint
        Vector3 toTarget = target - origin;
        transform.position = transform.position + 0.5f * toTarget;
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
        Bounds bounds = transform.UnscaledBounds();
        if( ! float.IsNaN( bounds.extents.z )       ||
            ! float.IsInfinity( bounds.extents.z )  ||
            bounds.extents.z == 0 )
        {
            return 1;
        }
        return bounds.extents.z * 2;


        //Vector3 startingScale = transform.localScale;
        //Quaternion startingRot = transform.rotation;
        //transform.rotation = Quaternion.identity;
        //transform.localScale = Vector3.one;

        //Collider collider = GetComponentInChildren<Collider>();
        //Bounds bounds = new Bounds();
        //if( collider != null )
        //{
        //    bounds = collider.bounds;
        //}
        //else
        //{
        //    Renderer renderer = GetComponentInChildren<Renderer>();
        //    if( renderer == null )
        //    {
        //        Debug.LogError( "No Renderer or Collider attached to object!" );
        //        transform.rotation = startingRot;
        //        transform.localScale = startingScale;
        //        return 1;
        //    }
        //    else
        //    {
        //        bounds = renderer.bounds;
        //    }
        //}
        //float unscaledLength = bounds.extents.z * 2;
        //transform.rotation = startingRot;
        //transform.localScale = startingScale;
        //return unscaledLength;
    }

    private void lookAtTarget( Vector3 targetPosition )
    {
        Vector3 relativePos = targetPosition - transform.position;
        if( relativePos.magnitude > 0 )
        {
            transform.rotation = Quaternion.LookRotation( relativePos ); 
        }
    }
}