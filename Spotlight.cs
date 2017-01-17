using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Light ) )]
public class Spotlight : Pointer 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Spotlight";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	private Light myLight;

//---------------------------------------------------------------------MONO METHODS:

	void Awake()
	{
		myLight = GetComponent<Light>();
	}

//--------------------------------------------------------------------------METHODS:
	
	public void switchLight( bool on )
	{
		if( on ) 
		{
			myLight.enabled = true;
		}
		else 
		{
			myLight.enabled = false;
		}
	}

//--------------------------------------------------------------------------HELPERS:
	
}