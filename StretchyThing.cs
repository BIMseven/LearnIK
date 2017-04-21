using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 startingScale = transform.localScale;
        Quaternion startingRot = transform.rotation;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        Renderer renderer = GetComponentInChildren<Renderer>();
        Bounds bounds = new Bounds();
        if( renderer != null )
        {
            bounds = renderer.bounds;
        }
        else
        {
            Collider collider = GetComponentInChildren<Collider>();
            if( collider == null )
            {
                Debug.LogError( "No Renderer or Collider attached to object!" );
                transform.rotation = startingRot;
                transform.localScale = startingScale;
                return 1;
            }
            else
            {
                bounds = collider.bounds;
            }
        }
        float unscaledLength = bounds.extents.z * 2;
        transform.rotation = startingRot;
        transform.localScale = startingScale;
        return unscaledLength;
    }

    private void lookAtTarget( Vector3 targetPosition )
    {
        Vector3 relativePos = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation( relativePos );
        transform.rotation = rotation;
    }
}