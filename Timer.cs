using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUtility
{ 
public class Timer 
{
//------------------------------------------------------------------------CONSTANTS:

	private static string LOG_TAG = "Timer";
	public static bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	private static Dictionary<string, float> startTimes = 
                                                  new Dictionary<string, float>();

//--------------------------------------------------------------------------METHODS:

	public static void Start( string taskName )
	{
		startTimes[taskName] = Time.realtimeSinceStartup;
	}

	public static void PrintTime( string taskName )
	{
		float time = TimeSinceStart( taskName );

		if( time >= 0 )
		{
			Utility.Print( LOG_TAG, 
						   "Task " + taskName + " running for " + time + " seconds" );
		}
	}

	public static float TimeSinceStart( string taskName )
	{
		float startTime; 
		if( startTimes.TryGetValue( taskName, out startTime ) ) 
		{
			return Time.realtimeSinceStartup - startTime;
		}
		Utility.Print( LOG_TAG, "Task " + taskName + " has not been started!" );
		return -1.0f;
	}
}
}