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
                return filter.RawVelocity;
            }
        }

        public Vector3 SmoothedVelocity
        {
            get
            {
                return filter.SmoothedVelocity;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return filter.Acceleration;
            }
        }

        public float MaxSecondsBetweenUpdates = 0.1f;

        private VelocityFilter filter;

        private Vector3 lastPosition;

        private float sinceLastUpdate;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            filter = new VelocityFilter( transform.position, SmoothingWeight );
            lastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            updateFilter();
        }

//--------------------------------------------------------------------------METHODS:

        public void Clear()
        {
            filter.Clear( transform.position );
        }

//--------------------------------------------------------------------------HELPERS:

        private void updateFilter()
        {
            sinceLastUpdate += Time.fixedDeltaTime;

            if( transform.position != lastPosition ||
                sinceLastUpdate >= MaxSecondsBetweenUpdates )
            {
                filter.Update( transform.position, sinceLastUpdate );
                sinceLastUpdate = 0;
                lastPosition = transform.position;
            }
        }

    }
}
