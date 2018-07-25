using UnityEngine;
using System.Collections;

namespace MyUtility
{
    [RequireComponent( typeof( Camera ) )]
    public class OnlyCamera : Singleton<OnlyCamera>
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "OnlyCamera";
        private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

        private Camera myCamera;

//---------------------------------------------------------------------MONO METHODS:

        new void Awake()
        {
            base.Awake();
            myCamera = GetComponent<Camera>();
        }

//--------------------------------------------------------------------------METHODS:

        public Camera getCamera()
        {
            return myCamera;
        }

//--------------------------------------------------------------------------HELPERS:

    }
}
