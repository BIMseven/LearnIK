 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// in mscorlib.dll so should not need to include extra references
using System.Runtime.CompilerServices;

namespace MyUtility
{
    public static class Utility
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

        public static Bounds CombineBounds( Bounds[] allBounds )
        {
            Bounds bounds = new Bounds( Vector3.zero, Vector3.zero );

            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;

            foreach( Bounds objBounds in allBounds )
            {
                maxX = Mathf.Max( maxX, objBounds.max.x );
                maxY = Mathf.Max( maxY, objBounds.max.y );
                maxZ = Mathf.Max( maxZ, objBounds.max.z );

                minX = Mathf.Min( minX, objBounds.min.x );
                minY = Mathf.Min( minY, objBounds.min.y );
                minZ = Mathf.Min( minZ, objBounds.min.z );
            }
            bounds.SetMinMax( new Vector3( minX, minY, minZ ),
                              new Vector3( maxX, maxY, maxZ ) );
            return bounds;
        }



        /// <summary>
        /// Scales this transform and none of its children
        /// </summary>
        /// <param name="and"></param>
        /// <param name="of"></param>
        /// <param name="children"></param>
        /// <returns></returns>        
        public static void DetatchAndScale( this Transform transform, Vector3 scale )
        {
            Transform[] children = transform.GetChildren();
            transform.DetachChildren();
            transform.localScale = scale;
            foreach( Transform child in children )
            {
                child.parent = transform;
            }
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
        /// Returns the bounds of given GameObject and its children's Renderers
        /// </summary>    
        public static Bounds GetBounds( GameObject gameObject )
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            if( renderers.Length == 0 )
            {
                return GetBoundsFromColliders( gameObject );
            }
            Bounds[] allBounds = renderers.Select( x => x.bounds ).ToArray();
            return CombineBounds( allBounds );
        }

        /// <summary>
        /// Returns the bounds of a given GameObject and it's children's Colliders
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Bounds GetBoundsFromColliders( GameObject gameObject )
        {
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            Bounds[] allBounds = colliders.Select( x => x.bounds ).ToArray();
            return CombineBounds( allBounds );
        }

        /// <summary>
        /// Returns an array of the children of this transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform[] GetChildren( this Transform transform )
        {
            Transform[] children = new Transform[transform.childCount];
            int i = 0;
            foreach( Transform T in transform )
            {
                children[i++] = T;
            }
            return children;
        }

        /// <summary>
        /// Returns all values in enum T
        /// </summary>
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>();
        }

        /// <summary>
        /// Returns the largest extent of bounding box of given gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static float GetMaxExtent( this GameObject gameObject )
        {
            Bounds bounds = GetBounds( gameObject );
            float maxExtent = bounds.extents.x;
            maxExtent = Mathf.Max( maxExtent, bounds.extents.y );
            maxExtent = Mathf.Max( maxExtent, bounds.extents.z );
            return maxExtent;
        }

        /// <summary>
        /// Returns the middle extent of bounding box of given gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static float GetMidExtent( this GameObject gameObject )
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
        public static float GetMinExtent( this GameObject gameObject )
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
        /// Returns true if given text is only letters (a-z or A-Z)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsAllLetters( this string text )
        {
            foreach( char character in text )
            {
                if( ! character.IsLetter() )
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if given character is a letter (a-z or A-Z)
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsLetter( this char character )
        {
            int ascii = (int)character;
            return ( ascii >= 65  &&  ascii <= 90 )  ||     // Is uppercase letter
                   ( ascii >= 97  &&  ascii <= 122 );       // Is lowercase letter
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
        /// Prints given message with the caller's string used as a tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void TPrint( this string tag, string message )
        {
            Print( tag, message );
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

        public static void Shuffle<T>( this IList<T> list )
        {
            int listLength = list.Count;

            for( int i = 0; i < listLength; i++ )
            {
                int indexToSwapWith = UnityEngine.Random.Range( 0, listLength );
                T value = list[indexToSwapWith];
                list[indexToSwapWith] = list[i];
                list[i] = value;
            }
        }

        /// <summary>
        /// Returns true if given numbers are both positive or are both negative
        /// </summary>
        /// <param name="num"></param>
        /// <param name="otherNum"></param>
        /// <returns></returns>
        public static bool SignsAgree( float num, float otherNum )
        {
            return ( num >= 0  &&  otherNum >= 0 ) ||
                   ( num < 0   &&  otherNum < 0 );
        }

        /// <summary>
        /// Returns the given text with quotes around it
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string WithQuotes( this string text )
        {
            return "\"" + text + "\"";
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
