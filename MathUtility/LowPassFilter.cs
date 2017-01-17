using UnityEngine;
using System.Collections;

namespace MyUtility
{
    //TODO LowPassFilter<T>
    public class LowPassFilter : Filter<float>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "LowPassFilter";
    
	private const float DEFAULT_SMOOTHING_WEIGHT = 0.4f;
    
//---------------------------------------------------------------------------FIELDS:

//---------------------------------------------------------------------CONSTRUCTORS:

    public LowPassFilter()
	{
		SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;
	}

	public LowPassFilter( float smoothingWeight )
	{
		SmoothingWeight = smoothingWeight;
	}

//--------------------------------------------------------------------------METHODS:

    /// <summary>
    /// Updates Raw and Smoothed values
    /// </summary>    
    public override void UpdateValue( float newValue, float deltaT )
	{
        if( isBadValue( newValue ) )
        {
            if( VERBOSE )   Utility.Print( LOG_TAG, "Bad value" );
            return;
        }
		Raw = newValue; 
		// Set the current velocity to be a combination of raw reading and the 
		// smoothed reading from previous Update.
		Smoothed = Mathf.Lerp( Smoothed, Raw, SmoothingWeight );	
	}
}    
}