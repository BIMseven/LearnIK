using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public class VelocityTracker : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "VelocityTracker";
        public bool VERBOSE = false;

        // 0 is conservative, 1 is confident in position

        private const float DEFAULT_SMOOTHING_WEIGHT = 0.2f;

//---------------------------------------------------------------------------FIELDS:

        [Range( 0, 1f )]
        public float SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;

        public Vector3 RawVelocity
        {
            get
            {
                return velocitySmoother.RawVelocity;
            }
        }

        public Vector3 SmoothedVelocity
        {
            get
            {
                return velocitySmoother.SmoothedVelocity;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return velocitySmoother.Acceleration;
            }
        }

        private VelocityFilter velocitySmoother;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            velocitySmoother = new VelocityFilter( SmoothingWeight );
        }

        void FixedUpdate()
        {
            velocitySmoother.Update( transform.position, Time.fixedDeltaTime );
        }

//--------------------------------------------------------------------------METHODS:

        public void Clear( Vector3 restingPosition )
        {
            velocitySmoother.Clear( restingPosition );
        }

//--------------------------------------------------------------------------HELPERS:
    }
}
