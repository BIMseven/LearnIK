using UnityEngine;
using System.Collections;
using MyUtility;

public class Blank : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ChangeMe";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {

    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
    private void vLog( string message )
    {
        if( VERBOSE )   LOG_TAG.TPrint( message );        
    }
}