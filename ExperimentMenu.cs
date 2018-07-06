using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public class ExperimentMenu : MonoBehaviour 
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "BeginExperimentGUI";
	    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

        private ExperimenterCamera experimenterCamera;

//---------------------------------------------------------------------MONO METHODS:

	    void Awake()
        {
            Utility.ActivateDisplays( 1 );
            if( ExperimenterCamera.InstanceExists )
            {
                experimenterCamera = ExperimenterCamera.Instance;
                ShowMenu( true );
                vLog( "Found ExperimenterCamera in scene" );
            }
            else
            {
                vLog( "No ExperimenterCamera in scene" );
            }
	    }		

//--------------------------------------------------------------------------METHODS:
        
        public void ShowMenu( bool show )
        {
            if( experimenterCamera != null )
            {
                experimenterCamera.gameObject.SetActive( ! show );
            }
            enableChildren( show );
        }

//--------------------------------------------------------------------------HELPERS:
	
        private void enableChildren( bool enabled )
        {
            var children = transform.GetChildren();
            foreach( Transform child in children )
            {
                child.gameObject.SetActive( enabled );
            }
        }

        private void vLog( string message )
        {
            if( VERBOSE )   LOG_TAG.TPrint( message );        
        }
    }
}