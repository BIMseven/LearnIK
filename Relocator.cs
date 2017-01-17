using UnityEngine;
using System.Collections;

public class Relocator : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Relocator";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	public string levelLoadTargetName = "none";

//---------------------------------------------------------------------MONO METHODS:

	void OnLevelWasLoaded( int level )
	{
		if( levelLoadTargetName != "none" )
		{
			attachToObjectWithTag( levelLoadTargetName );
		}
	}

	void Start() 
	{
		if( levelLoadTargetName != "none" )
		{
			attachToObjectWithTag( levelLoadTargetName );
		}
	}

//--------------------------------------------------------------------------METHODS:

	public void attachToObjectWithTag( string objectName )
	{
		GameObject newParent = GameObject.Find( objectName );
		if( newParent != null )
		{			
			transform.parent = newParent.transform;
			transform.localPosition = Vector3.zero;
		}
	}

//--------------------------------------------------------------------------HELPERS:
	
}
