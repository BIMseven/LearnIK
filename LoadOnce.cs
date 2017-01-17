using UnityEngine;
using System.Collections;

// TODO: make isFirstInstance a hash map based on class type so we can have more than one of these
public class LoadOnce : MonoBehaviour {

//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "LoadOnce";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:
	
	private static bool isFirstInstance = true;

//---------------------------------------------------------------------MONO METHODS:

	void Awake()
	{
		if( ! isFirstInstance )
		{
			Object.Destroy( gameObject );
			return;
		}
		isFirstInstance = false;		
	}

	void Start()
	{
		DontDestroyOnLoad( gameObject );
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}
