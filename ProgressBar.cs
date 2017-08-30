using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

namespace MyUtility
{
    public class ProgressBar : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "ProgressBar";
	    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
        [HideInInspector]
        public Transform ZeroPercentComplete, OneHundredPercentComplete;

        [HideInInspector]
        public StretchyThing Bar;

        [Range(0, 100)]
        public float PercentComplete;

        private float percentCompleteLastFrame;
        private VisibilityToggler visibilityToggler;
		
//---------------------------------------------------------------------MONO METHODS:
		
        void Start()
        {
            visibilityToggler = gameObject.AddComponent<VisibilityToggler>();
            visibilityToggler.Visible = true;
        }

	    void Update()
	    {
            if( ! visibilityToggler.Visible )   return;
            
            if( percentCompleteLastFrame != PercentComplete )
            {
                updateProgressBar( PercentComplete );
            }
            percentCompleteLastFrame = PercentComplete;
        }

//--------------------------------------------------------------------------METHODS:

        public void Hide()
        {
            visibilityToggler.Visible = false;
        }

        public void Show()
        {
            visibilityToggler.Visible = true;
            updateProgressBar( PercentComplete );
        }
        
//--------------------------------------------------------------------------HELPERS:
	
        private void updateProgressBar( float percent )
        {
            percent = Mathf.Clamp( percent, 0.001f, 100 );
            Vector3 origin = ZeroPercentComplete.position;
            Vector3 completePos = OneHundredPercentComplete.position;
            Vector3 stretchDir = completePos - origin;
            Vector3 stretchTarget = origin + stretchDir * ( percent / 100 );

            Bar.Stretch( origin, stretchTarget );
            Bar.transform.SetLocalEulerZ( 0 );
        }
    }
}