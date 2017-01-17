using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour 
{
	public enum PointerModes { FOLLOW_TARGET, IMITATE_TARGET };

//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Spotlight";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:
	
	public Transform target;
	public PointerModes mode;

//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		if( target == null )
		{
			enabled = false;
		}
	}
		
	void Update()
	{
		switch( mode )
		{
		case PointerModes.FOLLOW_TARGET:
			transform.LookAt( target );
			break;

		case PointerModes.IMITATE_TARGET:
			transform.localEulerAngles = target.localEulerAngles;
			break;
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}