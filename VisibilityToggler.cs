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
	
    public bool Visible;

    private bool wasVisible;	

//---------------------------------------------------------------------MONO METHODS:

    void OnEnable()
    {
        updateVisibility();
        wasVisible = Visible;
        updateVisibility();
    }

	void Start() 
	{
        updateVisibility();
        wasVisible = Visible;
        updateVisibility();
    }

	void Update()
	{
        if( wasVisible != Visible )
        {
            updateVisibility();
        }
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
    private void updateVisibility()
    {
        Utility.EnableRenderersInChildren( gameObject, Visible );
        wasVisible = Visible;
    }
}