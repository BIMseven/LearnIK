using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyUtility;

/// <summary>
/// Streams web cam onto RawImage.
/// </summary>

namespace MyUtility
{
    public class WebCamTexturePlayer : MonoBehaviour
    {
    //------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "WebCamTexturePlayer";
        public bool VERBOSE = false;

    //---------------------------------------------------------------------------FIELDS:

        public RawImage RawImage;

        public bool PlayOnAwake = true;

        // If RawImage is setting a Material attached to a plane, we can scale it to 
        // fill the camera's view frustum
        public bool ScalePlaneToFillScreen;
        public Camera ActiveCamera;
        
        public WebCamTexture WebCamTexture { get; private set; }

    //---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            if( RawImage == null )
            {
                RawImage = GetComponent<RawImage>();
            }
            if( RawImage == null )
            {
                Utility.PrintError( LOG_TAG, "Cannot find RawImage for WebCamera" );
            }
            WebCamTexture = new WebCamTexture();
            RawImage.texture = WebCamTexture;
            RawImage.material.mainTexture = WebCamTexture;

            if( PlayOnAwake )  Play();

            if( ScalePlaneToFillScreen )   scalePlane();
        }

    //--------------------------------------------------------------------------METHODS:

        public void Play()
        {
            WebCamTexture.Play();
        }

    //--------------------------------------------------------------------------HELPERS:
    
        // Assumes landscape plane
        private void scalePlane()
        {
            //RectTransform rect = GetComponent<RectTransform>();
            Transform rect = transform;
            Vector3 toCamera = ActiveCamera.transform.position - rect.position;
            float distance = toCamera.magnitude;
            float frustumHeight = 2.0f * distance * Mathf.Tan( ActiveCamera.fieldOfView * 
                                                               0.5f * Mathf.Deg2Rad );
            float frustumWidth = frustumHeight * ( 1.0f / ActiveCamera.aspect );

            Utility.Print( LOG_TAG, "SETTING HEIGHT AND WIDTH!" );
            Utility.Print( LOG_TAG, "Width: " + frustumWidth );
            Utility.Print( LOG_TAG, "Height: " + frustumHeight );

            // Planes are by default 10m x 10m
            rect.localScale = new Vector3( frustumHeight / 10.0f,
                                           1,
                                           frustumWidth / 10.0f );
        }
    }
}