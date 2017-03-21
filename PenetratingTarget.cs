using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    public class PenetratingTarget : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "PenetratingTarget";
        public bool VERBOSE = false;

        private const float DEFAULT_SMOOTHING_WEIGHT = 0.3f;

//---------------------------------------------------------------------------FIELDS:

        [Range( 0, 1 )]
        public float SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;

        public bool IsInsideObject { get; private set; }
        public Vector3 TargetVelocity { get; private set; }

        public Vector3 SurfacePoint { get; private set; }
        public Vector3 SurfacePointVelocity { get; private set; }
        public Vector2 SurfacePointUV { get; private set; }

        private VelocityFilter surfacePointFilter;
        private VelocityFilter targetFilter;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            surfacePointFilter = new VelocityFilter( SmoothingWeight );
            targetFilter = new VelocityFilter( SmoothingWeight );
        }

        void Update()
        {
            surfacePointFilter.SmoothingWeight = SmoothingWeight;
            targetFilter.SmoothingWeight = SmoothingWeight;
        }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:

    }
}