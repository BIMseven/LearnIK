using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    public class OnLoadDoer : MonoBehaviour
    {
    //------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "OnLoadDoer";
        public bool VERBOSE = false;

    //---------------------------------------------------------------------------FIELDS:

        public bool DestroyOnStart;

    //---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            if( DestroyOnStart )
            {
                if( VERBOSE )
                {
                    Utility.Print( LOG_TAG, "Destorying " + gameObject + " on Start" );
                }
                Object.Destroy( gameObject );
            }
        }

    //--------------------------------------------------------------------------METHODS:

    //--------------------------------------------------------------------------HELPERS:

    }
}