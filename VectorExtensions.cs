using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public static class VectorExtensions 
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "VectorExtensions";
        
//--------------------------------------------------------------------------METHODS:
        
        public static Vector3 ClampComponents( this Vector3 vector, 
                                               float min, float max )
        {
            return new Vector3( Mathf.Clamp( vector.x, min, max ),
                                Mathf.Clamp( vector.y, min, max ),
                                Mathf.Clamp( vector.z, min, max ) );
        }

        public static Vector3 ClampMag( this Vector3 vector,
                                        float minMag, float maxMag )
        {
            if( vector.magnitude < minMag )    return vector.normalized * minMag;

            if( vector.magnitude > maxMag )    return vector.normalized * maxMag;

            return vector;
        }

        public static uint IndexOfLongestComponent( this Vector3 vector )
        {
            float absX = Mathf.Abs( vector.x );
            float absY = Mathf.Abs( vector.y );
            float absZ = Mathf.Abs( vector.z );

            if( absX > Mathf.Max( absY, absZ ) )   return 0;
            if( absY > Mathf.Max( absX, absZ ) )   return 1;
            return 2;
        }
    
        // Returns a vector2 made from the x and z components of given vector
        public static Vector2 XY( this Vector3 vector )
        {
            return new Vector2( vector.x, vector.y );
        }

        // Returns a vector2 made from the x and z components of given vector
        public static Vector2 XZ( this Vector3 vector )
        {
            return new Vector2( vector.x, vector.z );
        }

        
    }
}
