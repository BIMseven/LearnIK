using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to be attached to a 3D Text object.  It will shrink on collisions
/// with other Text Meshes until it's no longer colliding
/// </summary>

[RequireComponent( typeof(TextMesh) )]
public class ShrinkingText : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ShrinkingText";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    // By default will shrink 2.5% each frame colliding
    public float ShrinkRate = 0.025f;
		
//---------------------------------------------------------------------MONO METHODS:

    void OnTriggerStay( Collider collider )
    {
        if( collider.GetComponent<TextMesh>() != null )
        {
            transform.localScale = ( 1.0f - ShrinkRate ) * transform.localScale;
        }
    }
    
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}