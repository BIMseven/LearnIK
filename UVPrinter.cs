using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    public class UVPrinter : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "UVPrinter";
        public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

        public Camera Cam;
        public Vector2 UV;
        public Vector2 Pixel;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {

        }

        void Update()
        {
            Pixel = Cam.WorldToScreenPoint( transform.position );
            UV.x = Pixel.x / Cam.pixelWidth;
            UV.y = Pixel.y / Cam.pixelHeight;
            if( VERBOSE )
            {
                LOG_TAG.TPrint( "UV: " + UV );
                LOG_TAG.TPrint( "Pixel: " + Pixel );
            }

        }

//--------------------------------------------------------------------------METHODS:
        
//--------------------------------------------------------------------------HELPERS:

    }
}