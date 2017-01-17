using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public abstract class Filter<T> : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Filter";
	public bool VERBOSE = false;

    // TODO find alternative
    // We were getting calls to Update more frequently than the game was updating
    // transform.  To keep it from happening again, we ignore updates with same
    // position unless it keeps happining (in which case the object is probably
    // stopped)
    private const int MAX_DUPLICATES_BEFORE_ACCEPTING = 2;

//---------------------------------------------------------------------------FIELDS:
	
	public T Raw { get; protected set; }
	public T Smoothed { get; protected set; }
		
    // 0 puts more weight towards smoothed value, 1 puts more weight/trust into 
    // the raw value
    [Range( 0, 1.0f )]
    public float SmoothingWeight;
        
    #region PRIVATE_FIELDS
    // Helps find duplicate/bad values in isBadValue()
    private T previousValue;
	// Helper for isBadvalue, compared against UPDATES_PER_VALUE
	private uint timesValueMatchedPrevious;
    #endregion

//--------------------------------------------------------------------------METHODS:

    public abstract void UpdateValue( T newValue, float deltaT );

//--------------------------------------------------------------------------HELPERS:
	
    // This can be used in Update to check if newValue is a repeat/bad value
    protected bool isBadValue( T value )
    {
        // Sometimes user feeds the same position.  We need to count how many times
        // position was equal to previousPosition because we might actually be 
        // stopped, but we don't want a bum value to affect our speed if it isn't
        if( value.Equals( previousValue ) &&
            ++timesValueMatchedPrevious < MAX_DUPLICATES_BEFORE_ACCEPTING )
        {
            return true;
        }
        timesValueMatchedPrevious = 0;
        return false;
    }
}
}