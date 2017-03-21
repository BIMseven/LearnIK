using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    public class DistanceTracker : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "DistanceTracker";

//---------------------------------------------------------------------------FIELDS:

        public float TotalDistanceTraveled { get; private set; }
        private Vector3 previousPosition;

//---------------------------------------------------------------------MONO METHODS:

        void FixedUpdate()
        {
            Vector3 movement = previousPosition - transform.position;
            TotalDistanceTraveled += movement.magnitude;
            previousPosition = transform.position;
        }

        void Start()
        {
            previousPosition = transform.position;
        }
        
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
    }
}