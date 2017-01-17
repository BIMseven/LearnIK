using UnityEngine;
using System.Collections;
using MyUtility;

public class CollisionScreamer : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "CollisionScreamer";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:
	
		
//---------------------------------------------------------------------MONO METHODS:

	void OnCollisionEnter( Collision collision )
	{
		Utility.Print( LOG_TAG, "collision with " + collision.gameObject.tag );
	}

	void OnTriggerEnter( Collider collider )
	{
		Utility.Print( LOG_TAG, "trigger with " + collider.gameObject.tag );
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}
