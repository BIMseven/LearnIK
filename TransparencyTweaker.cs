using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyTweaker : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "TransparencyTweaker";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    /// <summary>
    /// This Material (set in the inspector) must use the Transparent/Diffuse
    /// shader
    /// </summary>
    public Material TransparentDiffuseMaterial;

    private float transparency;
    public float Transparency
    {
        get
        {
            return transparency;
        }
        set
        {
            Color newColor = TransparentDiffuseMaterial.color;
            newColor.a = 1 - Transparency;
            TransparentDiffuseMaterial.color = newColor;
            transparency = value;
        }
    }
		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        GetComponent<Renderer>().material = TransparentDiffuseMaterial;
    }		

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}