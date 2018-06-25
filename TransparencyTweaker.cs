using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyTweaker : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "TransparencyTweaker";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    private Material transparantMaterial;
    
    public float Transparency
    {
        get
        {
            return transparantMaterial.color.a;
        }
        set
        {
            Color newColor = transparantMaterial.color;
            newColor.a = value;
            transparantMaterial.color = newColor;
        }
    }

//---------------------------------------------------------------------MONO METHODS:

    void Start() 
	{
        transparantMaterial = GetComponent<Renderer>().material;        
    }
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}