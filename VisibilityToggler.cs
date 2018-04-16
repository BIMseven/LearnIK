using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class VisibilityToggler : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "VisibilityToggler";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    public bool StartVisible = false;

    private bool isVisible;
    public bool Visible
    {
        get
        {
            return isVisible;
        }
        set
        {
            isVisible = value;
            Utility.EnableRenderersInChildren( gameObject, isVisible );
        }
    }
    
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        Visible = StartVisible;
    }    

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}