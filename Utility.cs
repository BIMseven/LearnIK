using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// in mscorlib.dll so should not need to include extra references
using System.Runtime.CompilerServices;

namespace MyUtility
{
    public class Utility : MonoBehaviour
    {
    //------------------------------------------------------------------------CONSTANTS:

        public static int TAG_SPACE = 20;

    //--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Returns true if given collider is inside any other objects with colliders
        /// in the scene.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="rayLength"></param>
        /// <returns></returns>
        public static bool ColliderInsideObject( Collider collider,
                                                 float rayLength = 1000 )
        {
            return ObjColliderIsInside( collider, rayLength ) != null;
        }


        /// <summary>
        /// Enables or disables the renderers in this game object and all children
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enable"></param>
        public static void EnableRenderersInChildren( GameObject obj, bool enable )
        {
            if( obj == null )
            {
                return;
            }

            Renderer thisRenderer = obj.GetComponent<Renderer>();
            if( thisRenderer != null )
            {
                thisRenderer.enabled = enable;
            }
            Renderer[] components = obj.GetComponentsInChildren<Renderer>();
            foreach( Renderer renderer in components )
            {
                renderer.enabled = enable;
            }
        }

        /// <summary>
        /// Returns the bounds of given GameObject and its children
        /// </summary>    
        public static Bounds GetBounds( GameObject gameObject )
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            Bounds bounds = new Bounds( gameObject.transform.position, Vector3.zero );

            foreach( Renderer renderer in renderers )
            {
                float maxX = Mathf.Max( renderer.bounds.max.x, bounds.max.x );
                float maxY = Mathf.Max( renderer.bounds.max.y, bounds.max.y );
                float maxZ = Mathf.Max( renderer.bounds.max.z, bounds.max.z );

                float minX = Mathf.Min( renderer.bounds.min.x, bounds.min.x );
                float minY = Mathf.Min( renderer.bounds.min.y, bounds.min.y );
                float minZ = Mathf.Min( renderer.bounds.min.z, bounds.min.z );

                bounds.SetMinMax( new Vector3( minX, minY, minZ ),
                                  new Vector3( maxX, maxY, maxZ ) );
            }
            return bounds;
        }

        /// <summary>
        /// Returns all values in enum T
        /// </summary>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>();
        }

        /// <summary>
        /// Returns the middle extent of bounding box of given gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static float GetMidExtent( GameObject gameObject )
        {
            Vector3 extents = GetBounds( gameObject ).extents;
            if( extents.x > extents.y && extents.x < extents.z )
            {
                return extents.x;
            }
            if( extents.x < extents.y && extents.x > extents.z )
            {
                return extents.x;
            }
            if( extents.y > extents.x && extents.y < extents.z )
            {
                return extents.y;
            }
            if( extents.y < extents.x && extents.y > extents.z )
            {
                return extents.y;
            }
            return extents.z;
        }

        /// <summary>
        /// Returns the smallest extent of bounding box of given gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static float GetMinExtent( GameObject gameObject )
        {
            Bounds bounds = GetBounds( gameObject );
            float minExtent = bounds.extents.x;
            minExtent = Mathf.Min( minExtent, bounds.extents.y );
            minExtent = Mathf.Min( minExtent, bounds.extents.z );
            return minExtent;
        }

        // Warning: Assumes you have the key saved
        public static bool GetPlayerPrefBool( string tag )
        {
            int prefValue = PlayerPrefs.GetInt( tag );
            return prefValue > 0;
        }

        /// <summary>
        /// Returns the GameObject of the object the collider is inside if the collider
        /// is inside an object
        /// in the scene.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="rayLength"></param>
        /// <returns></returns>
        public static GameObject ObjColliderIsInside( Collider collider,
                                                      float rayLength = 1000 )
        {
            // Shoot two rays, one from way ahead and one from way behind.  We can
            // decide if we're in an object based on the order of collisions of these
            // two rays
            Vector3 colliderPos = collider.transform.position;
            Vector3 ahead = collider.transform.forward;

            Collider[] fromAhead = lastTwoHitsFromDirection( colliderPos, ahead, rayLength );
            if( fromAhead == null ) return null;
            Collider lastThingHitFromAhead = fromAhead[0];
            Collider secondToLastThingHitFromAhead = fromAhead[1];

            Collider[] fromBehind = lastTwoHitsFromDirection( colliderPos, -ahead, rayLength );
            if( fromBehind == null ) return null;
            Collider lastThingHitFromBehind = fromBehind[0];
            Collider secondToLastThingHitFromBehind = fromBehind[1];

            if( lastThingHitFromAhead != collider )
            {
                return lastThingHitFromAhead.gameObject;
            }
            if( lastThingHitFromBehind != collider )
            {
                return lastThingHitFromBehind.gameObject;
            }
            if( secondToLastThingHitFromAhead == secondToLastThingHitFromBehind )
            {
                return secondToLastThingHitFromAhead.gameObject;
            }
            return null;
        }

        /// <summary>
        /// Plays the given AudioSource's clip at the source's position
        /// </summary>
        /// <param name="audioSource"></param>
        public static void PlayAtSource( AudioSource audioSource )
        {
            if( audioSource != null )
            {
                AudioSource.PlayClipAtPoint( audioSource.clip,
                                             audioSource.transform.position );
            }
        }

        /// <summary>
        /// Prints given message preceded by given tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void Print( string tag, string message )
        {
            MonoBehaviour.print( tag + " --- " + message );
        }

        /// <summary>
        /// Prints error with given message and tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void PrintError( string tag, string message )
        {
            Debug.LogError( tag + " --- " + message );
        }

        // Generate a random point within the given Bounds
        public static Vector3 RandomPointInBounds( Bounds bounds )
        {
            return RandomVectorInRange( bounds.min, bounds.max );
        }

        public static Vector3 RandomVectorInRange( Vector3 min, Vector3 max )
        {
            return new Vector3( UnityEngine.Random.Range( min.x, max.x ),
                                UnityEngine.Random.Range( min.y, max.y ),
                                UnityEngine.Random.Range( min.z, max.z ) );
        }
        
        public static void SetPlayerPref( string tag, bool isTrue )
        {
            if( isTrue )
            {
                PlayerPrefs.SetInt( tag, 1 );
            }
            else
            {
                PlayerPrefs.SetInt( tag, 0 );
            }
        }
 
        public static bool SignsAgree( float num, float otherNum )
        {
            return ( num >= 0  &&  otherNum >= 0 ) ||
                   ( num < 0   &&  otherNum < 0 );
        }

        // Returns a vector2 made from the x and z components of given vector
        public static Vector2 XZ( Vector3 vector )
        {
            return new Vector2( vector.x, vector.z );
        }

    //--------------------------------------------------------------------------HELPERS:

        /// <summary>
        /// Returns the last two things hit by ray.  
        /// Index 0 is last thing hit, Index 1 is second to last thing hit
        /// </summary>
        /// <param name="colliderPos"></param>
        /// <param name="rayDirection">Direction of </param>
        /// <param name="rayLength"></param>
        /// <returns></returns>
        private static Collider[] lastTwoHitsFromDirection( Vector3 colliderPos,
                                                        Vector3 rayDirection,
                                                        float rayLength )
    {
        Vector3 rayOrigin = colliderPos + rayDirection * rayLength;
        RaycastHit[] hits = Physics.RaycastAll( rayOrigin, -rayDirection, rayLength );

        if( hits.Length == 0 ) Debug.LogError( "that makes no sense" );
        if( hits.Length <= 1 ) return null;
        // Order the hits by distance so the last thing hit will be the collider
        hits = hits.OrderBy( h => h.distance ).ToArray();

        //Debug.DrawLine( rayOrigin, colliderPos, Color.cyan );

        Collider lastThingHit = hits[hits.Length - 1].collider;
        Collider secondToLastThingHit = hits[hits.Length - 2].collider;

        Collider[] hitColliders = new Collider[2];
        hitColliders[0] = lastThingHit;
        hitColliders[1] = secondToLastThingHit;

        return hitColliders;
    }
}
}
