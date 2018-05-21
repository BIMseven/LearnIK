using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    [RequireComponent( typeof( Camera ) )]
    public class Blinders : MonoBehaviour 
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "Blinders";
	    public bool VERBOSE = false;

        private const float EPSILON = 0.0001f;

//---------------------------------------------------------------------------FIELDS:
	
        private Camera cameraToCover;

        private float initialNearClip, initialFarClip;

        private bool blindersVisible;

//---------------------------------------------------------------------MONO METHODS:

	    void Start() 
	    {
            cameraToCover = GetComponent<Camera>();
            initialNearClip = cameraToCover.nearClipPlane;
            initialFarClip = cameraToCover.farClipPlane;
        }		

//--------------------------------------------------------------------------METHODS:

        public void SetVisible( bool visible )
        {
            vLog( "Blinders on: " + visible );

            blindersVisible = visible;

            if( visible )
            {
                cameraToCover.nearClipPlane = EPSILON;
                cameraToCover.farClipPlane = 2 * EPSILON;
            }
            else
            {
                cameraToCover.nearClipPlane = initialNearClip;
                cameraToCover.farClipPlane = initialFarClip;
            }
        }
        
        public void Toggle()
        {
            SetVisible( ! blindersVisible );
        }

//--------------------------------------------------------------------------HELPERS:
	
        private void vLog( string message )
        {
            if( VERBOSE )   LOG_TAG.TPrint( message );        
        }
    }
}