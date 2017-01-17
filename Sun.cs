using UnityEngine;
using System.Collections;
using MyUtility;
// TODO: setTime

[RequireComponent( typeof( Light ) )]
public class Sun : Singleton<Sun>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Sun";
	public bool VERBOSE = false;

	private const float SECONDS_PER_MINUTE  = 60.0f;
	private const float SECONDS_PER_DAY 	= SECONDS_PER_MINUTE * 60.0f * 24.0f;
	private const float DEGREES_PER_SECOND  = 360.0f / SECONDS_PER_DAY;

//---------------------------------------------------------------------------FIELDS:
		
	public float MinutesPerDay = 8.0f;

	private Light sun;		// directional light

	private bool dayHasBegun;
	private float degreesToRotate;
	private float currentTime;

//---------------------------------------------------------------------MONO METHODS:

	void Awake() 
	{
		sun = GetComponent<Light>();

		dayHasBegun = false;
	}
		
	void Update()
	{
		if( dayHasBegun )
		{
			Vector3 targetRotation = new Vector3( degreesToRotate, 0.0f, 0.0f );
			sun.transform.Rotate( targetRotation * Time.deltaTime );
			currentTime += Time.deltaTime;
		}
	}

//--------------------------------------------------------------------------METHODS:

	public void beginDay()
	{
		if( VERBOSE )    Utility.Print( LOG_TAG, "Beginning day" );
		currentTime = 0.0f;
		degreesToRotate = ( DEGREES_PER_SECOND * SECONDS_PER_DAY ) / 
						  ( MinutesPerDay * SECONDS_PER_MINUTE ); 
		dayHasBegun = true;
	}

	public bool isNight()
	{
		return sun.transform.eulerAngles.x >= 270.0f;
	}

//--------------------------------------------------------------GETTERS AND SETTERS:

	public float getIntensity()
	{
		return sun.intensity;
	}

	public float getTime()
	{
		return currentTime;
	}

	public void setDayDuration( float minutes )
	{
		MinutesPerDay = minutes;
	}

	public void setIntensity( float intensity )
	{
		sun.intensity = intensity;
	}

	public void setTime( float time )
	{
		currentTime = time;
	}
}