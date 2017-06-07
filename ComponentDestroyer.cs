using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    public class ComponentDestroyer<T> : MonoBehaviour where T : Behaviour
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "ComponentDestroyer";

//---------------------------------------------------------------------------FIELDS:
	
        private T component;
		
//---------------------------------------------------------------------MONO METHODS:

//--------------------------------------------------------------------------METHODS:

        public void Destroy( float delay )
        {
            this.component = GetComponent<T>();
            Invoke( "DestroyHelper", delay );
        }

//--------------------------------------------------------------------------HELPERS:

        void DestroyHelper()
        {
            Destroy( component );
        }

    }
}