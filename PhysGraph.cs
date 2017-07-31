using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyUtility
{
    public class PhysGraph : Singleton<PhysGraph> {
        
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "PhysGraph";
	    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
        public Transform[] DataSpheres;

        private VisibilityToggler[] sphereTogglers;
		
//---------------------------------------------------------------------MONO METHODS:

	    void Start() 
	    {
            sphereTogglers = new VisibilityToggler[DataSpheres.Length];
            for( int i = 0; i < DataSpheres.Length; i++ )
            {
                Transform sphere = DataSpheres[i];
                sphereTogglers[i] = sphere.GetComponent<VisibilityToggler>();
            }
	    }
		

//--------------------------------------------------------------------------METHODS:

        public void HideGraph( int graphNum )
        {
            if( graphNum < 0  ||  graphNum >= sphereTogglers.Length )
            {
                return;
            }
            sphereTogglers[graphNum].Visible = false;
        }

        public void SetValue( int graphNum, float value )
        {
            if( graphNum < 0 || graphNum >= sphereTogglers.Length )
            {
                return;
            }
            sphereTogglers[graphNum].Visible = true;
            DataSpheres[graphNum].SetLocalPosY( value );
        }

//--------------------------------------------------------------------------HELPERS:
	
    }

}


