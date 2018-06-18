using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class LocalVelocityTracker : VelocityTracker
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "LocalVelocityTracker";

//---------------------------------------------------------------------------FIELDS:
	
    public Transform TargetSpace;

    private Vector3 localPosition
    {
        get
        {
            if( TargetSpace == null )   return transform.position;

            return TargetSpace.InverseTransformPoint( transform.position );
        }
    }


//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        if( TargetSpace == null )   TargetSpace = transform.parent;
    }
		
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
    protected override void updateFilter()
    {
        sinceLastUpdate += Time.fixedDeltaTime;

        Vector3 position = localPosition;

        if( position != lastPosition ||
            sinceLastUpdate >= MaxSecondsBetweenUpdates )
        {
            filter.Update( position, sinceLastUpdate );
            sinceLastUpdate = 0;
            lastPosition = position;
        }
    }

    private void vLog( string message )
    {
        if( VERBOSE )   LOG_TAG.TPrint( message );        
    }
}