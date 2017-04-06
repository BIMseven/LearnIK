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

    [Range( 0, 1 )]
    public float Transparency;
		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        GetComponent<Renderer>().material = TransparentDiffuseMaterial;
    }
		
	void Update()
	{
        Color newColor = TransparentDiffuseMaterial.color;
        newColor.a = 1 - Transparency;
        TransparentDiffuseMaterial.color = newColor;
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}