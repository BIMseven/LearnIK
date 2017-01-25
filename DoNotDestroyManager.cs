using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroyManager : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "DoNotDestroyManager";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    public GameObject[] Objects;
		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        foreach( GameObject obj in Objects )
        {
            Object.DontDestroyOnLoad( obj );
        }
	}
		
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}