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
        private const int CULL_ONLY_UI = 32;
        
//---------------------------------------------------------------------------FIELDS:
	
        public bool DoNotHideUI = true;

        private Camera cameraToCover;

        private CameraClearFlags initialClearFlag;
        private int initialCullingMask;

        public bool BlindersActive { get; private set; }
                

//---------------------------------------------------------------------MONO METHODS:

	    void Start() 
	    {
            cameraToCover = GetComponent<Camera>();
            initialClearFlag = cameraToCover.clearFlags;
            initialCullingMask = cameraToCover.cullingMask;
        }		

//--------------------------------------------------------------------------METHODS:

        public void SetVisible( bool visible )
        {
            vLog( "Blinders on: " + visible );

            BlindersActive = visible;

            if( visible )
            {
                cameraToCover.cullingMask = 0;
                if( DoNotHideUI )  cameraToCover.cullingMask = CULL_ONLY_UI;
                cameraToCover.clearFlags = CameraClearFlags.SolidColor;
            }
            else
            {
                cameraToCover.clearFlags = initialClearFlag;
                cameraToCover.cullingMask = initialCullingMask;
            }
        }
        
        public void Toggle()
        {
            SetVisible( ! BlindersActive );
        }

//--------------------------------------------------------------------------HELPERS:
	
        private void vLog( string message )
        {
            if( VERBOSE )   LOG_TAG.TPrint( message );        
        }
    }
}