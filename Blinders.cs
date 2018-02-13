using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public class Blinders : MonoBehaviour 
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "Blinders";
	    public bool VERBOSE = false;

        private const float EPSILON = 0.0001f;

//---------------------------------------------------------------------------FIELDS:
	
        public Camera CameraToCover;

        // Should be attached to child plane with surface normal aligned with this
        // transform's forward vector
        public FillScreen ScreenFiller;

        // Will be added to this object at run time
        private VisibilityToggler visibilityToggler;

//---------------------------------------------------------------------MONO METHODS:

	    void Start() 
	    {
            visibilityToggler = gameObject.EnsureComponent<VisibilityToggler>();
            visibilityToggler.Visible = false;
        }		

//--------------------------------------------------------------------------METHODS:

        public void BlindersOn( bool on )
        {
            if( VERBOSE )  LOG_TAG.TPrint( "Blinders on: " + on );

            ScreenFiller.FillScreenOfCamera( CameraToCover );
            transform.parent = CameraToCover.transform;
            visibilityToggler.Visible = on;
        }
        
//--------------------------------------------------------------------------HELPERS:
	
    }
}