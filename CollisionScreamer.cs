using UnityEngine;
using System.Collections;
using MyUtility;

public class CollisionScreamer : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "CollisionScreamer";

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
    
	void OnCollisionExit( Collision collision )
	{
		Utility.Print( LOG_TAG, "exit collision with " + collision.gameObject.tag );
	}

	void OnTriggerExit( Collider collider )
	{
		Utility.Print( LOG_TAG, "exit trigger with " + collider.gameObject.tag );
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}
