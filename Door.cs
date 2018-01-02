using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    //TODO inverse slerp to fix starting position

    public class Door : MonoBehaviour
    {

        public enum States
        {
            Open, Closed, Opening, Closing
        }

//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "Door";
        public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

        public Vector3 ClosedLocalEuler, OpenLocalEuler;
        public float SecondsToOpenOrClose = 0.5f;

        private Quaternion closedRot, openedRot;

        private float timeStartedMoving;
        public States state;
        
//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            closedRot = Quaternion.Euler( ClosedLocalEuler );
            openedRot = Quaternion.Euler( OpenLocalEuler );

            // Find starting state
            if( Quaternion.Angle( transform.localRotation, closedRot ) >
                Quaternion.Angle( transform.localRotation, openedRot ) )
            {
                state = States.Open;
            }
            else
            {
                state = States.Closed;
            }
            //////////////////////

        }
        
//--------------------------------------------------------------------------METHODS:

        public void Open( bool open )
        {
            if( open  &&  state == States.Closed )
            {
                if( VERBOSE )  LOG_TAG.TPrint( "Opening" );
                timeStartedMoving = Time.realtimeSinceStartup;
                state = States.Opening;
                StartCoroutine( "open" );
            }
            else if( ! open  &&  state == States.Open )
            {
                if( VERBOSE ) LOG_TAG.TPrint( "Closing" );
                timeStartedMoving = Time.realtimeSinceStartup;
                state = States.Closing;
                StartCoroutine( "close" );
            }
        }

//--------------------------------------------------------------------------HELPERS:

        private float calcT()
        {
            float secsMoving = Time.realtimeSinceStartup - timeStartedMoving;
            return secsMoving / SecondsToOpenOrClose;
        }

        private IEnumerator close()
        {
            while( state == States.Closing )
            {
                continueClosing();
                yield return new WaitForEndOfFrame();
            }
        }

        private void continueClosing()
        {
            float t = calcT();
            if( t > 1 )
            {
                transform.localRotation = closedRot;
                state = States.Closed;
            }
            else
            {
                transform.localRotation = Quaternion.Slerp( openedRot, closedRot, t );
            }
        }

        private void continueOpening()
        {
            float t = calcT();
            if( t > 1 )
            {
                transform.localRotation = openedRot;
                state = States.Open;
            }
            else
            {
                transform.localRotation = Quaternion.Slerp( closedRot, openedRot, t );
            }
        }

        private IEnumerator open()
        {
            while( state == States.Opening )
            {
                continueOpening();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
