using UnityEngine;
using System.Collections;

namespace MyUtility
{
    // Keeps track of smoothed velocity (position must be updated regularly).  Uses a 
    // simple low pass filter.
    public class VelocityFilter
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "VelocityTracker";
        public bool VERBOSE = false;

        private const float EPSILON = 0.001f;

        private const float DEFAULT_SMOOTHING_WEIGHT = 0.5f;
        
//---------------------------------------------------------------------------FIELDS:

        public Vector3 RawVelocity { get; private set; }
        public Vector3 SmoothedVelocity { get; private set; }
        public Vector3 Acceleration { get; private set; }

        // 0 puts more weight towards smoothed value, 1 puts more weight/trust into 
        // the raw value
        [Range( 0, 1.0f )]
        public float SmoothingWeight;

        public Vector3 PreviousPosition;
        
        // TODO If we want to save older smoothed velocities, use queue:
        //https://msdn.microsoft.com/en-us/library/7977ey2c(v=vs.110).aspx

//---------------------------------------------------------------------CONSTRUCTORS:
        
        public VelocityFilter( float smoothingWeight = DEFAULT_SMOOTHING_WEIGHT )
        {
            SmoothingWeight = smoothingWeight;
        }

        public VelocityFilter( Vector3 StartingPosition, 
                               float smoothingWeight = DEFAULT_SMOOTHING_WEIGHT )
        {
            SmoothingWeight = smoothingWeight;
            Clear( StartingPosition );
        }

//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Sets velocity to zero
        /// </summary>
        /// <param name="restingPosition">The place the object is stopped</param>
        public void Clear( Vector3 restingPosition )
        {
            RawVelocity = Vector3.zero;
            SmoothedVelocity = Vector3.zero;
            PreviousPosition = restingPosition;
        }

        /// <summary>
        /// Updates the current velocity based on position reading
        /// </summary>    
        public void Update( Vector3 position, float deltaT )
        {
            Vector3 velocityLastFrame = SmoothedVelocity;

            // Get a raw reading of velocity
            RawVelocity = ( position - PreviousPosition ) / deltaT;
            

            // Set the current velocity to be a combination of raw reading and the 
            // smoothed reading from previous frame.
            SmoothedVelocity = Vector3.Lerp( velocityLastFrame,
                                             RawVelocity,
                                             SmoothingWeight );
            
            Acceleration = SmoothedVelocity - velocityLastFrame;

            // We will need this for the next update
            PreviousPosition = position;
        }

//--------------------------------------------------------------------------HELPERS:

//--------------------------------------------------------------GETTERS AND SETTERS:
    }
}