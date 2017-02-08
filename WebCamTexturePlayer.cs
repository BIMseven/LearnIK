using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyUtility;

/// <summary>
/// Streams web cam onto RawImage
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

        public bool PlayOnAwake;

        private WebCamTexture webCamTexture;

    //---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            if( RawImage == null )
            {
                RawImage = GetComponent<RawImage>();
            }
            if( RawImage == null )
            {
                MyUtility.Utility.PrintError( LOG_TAG,
                                              "Cannot find RawImage for WebCamera" );
            }
            webCamTexture = new WebCamTexture();
            RawImage.texture = webCamTexture;
            RawImage.material.mainTexture = webCamTexture;
            
            print( "Device: " + SystemInfo.deviceName );
            WebCamDevice[] devices = WebCamTexture.devices;
            print( "" + devices.Length + " devices: " );
            foreach( WebCamDevice device in devices )
            {
                print( "name: " + device.name );
            }
            

            if( PlayOnAwake ) Play();
        }

    //--------------------------------------------------------------------------METHODS:

        public void Play()
        {
            webCamTexture.Play();
        }

    //--------------------------------------------------------------------------HELPERS:

    }
}