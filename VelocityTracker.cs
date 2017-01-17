using UnityEngine;
using System.Collections;

namespace MyUtility
{ 
public class VelocityTracker : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "VelocityTracker";
	public bool VERBOSE = false;
    
    // 0 is conservative, 1 is confident in position
    [Range( 0, 1f )]
    private const float SMOOTHING_WEIGHT = 0.2f;

//---------------------------------------------------------------------------FIELDS:
	
    public Vector3 RawVelocity
    {
        get
        {
            return velocitySmoother.RawVelocity;
        }
    }

    public Vector3 SmoothedVelocity
    {
        get
        {
            return velocitySmoother.SmoothedVelocity;
        }
    }

    private VelocityFilter velocitySmoother;
    
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        velocitySmoother = new VelocityFilter( SMOOTHING_WEIGHT );
    }
		
	void FixedUpdate()
    {
        velocitySmoother.Update( transform.position, Time.fixedDeltaTime );
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
}
}
