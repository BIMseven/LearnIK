using UnityEngine;
using System.Collections;

namespace MyUtility
{ 
/// <summary>
/// Applies a simple low pass filter to smooth rotation
/// </summary>
public class QuatFilter : Filter<Quaternion>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "QuatFilter";
    
	private const float DEFAULT_SMOOTHING_WEIGHT = 0.4f;
    
//---------------------------------------------------------------------------FIELDS:
	
        
//---------------------------------------------------------------------CONSTRUCTORS:

    public QuatFilter()
    {
        SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;
    }

    public QuatFilter( float smoothingWeight )
    {
        SmoothingWeight = smoothingWeight;
    }

//--------------------------------------------------------------------------METHODS:
	
    /// <summary>
	/// Updates the current quat based on position reading
	/// </summary>    
	public override void UpdateValue( Quaternion newQuat, float deltaT )
    {

        if( isBadValue( newQuat ) )
        {
            if( VERBOSE ) Utility.Print( LOG_TAG, "Bad value" );
            return;
        }

        Raw = newQuat;

        //Smoothed = Quaternion.Lerp( Smoothed, Raw, SmoothingWeight );

        Smoothed = Quaternion.Slerp( Smoothed, Raw, SmoothingWeight );
    }
}   
}