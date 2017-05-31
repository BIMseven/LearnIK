using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public class GameObjectDestroyer : MonoBehaviour 
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "GameObjectDestroyer";

//---------------------------------------------------------------------------FIELDS:
		
//---------------------------------------------------------------------MONO METHODS:

//--------------------------------------------------------------------------METHODS:

        public void Destroy( float delay )
        {
            Invoke( "DestroyHelper", delay );
        }

//--------------------------------------------------------------------------HELPERS:

        void DestroyHelper()
        {
            Destroy( gameObject );
        }
    }
}