using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public static class TransformExtensions
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "TransformExtensions";
        
//--------------------------------------------------------------------------METHODS:
        
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
        
        public static void SetEulerX( this Transform transform, float x )
        {
            Vector3 rot = transform.eulerAngles;
            transform.eulerAngles = new Vector3( x, rot.y, rot.z );
        }

        public static void SetEulerY( this Transform transform, float y )
        {
            Vector3 rot = transform.eulerAngles;
            transform.eulerAngles = new Vector3( rot.x, y, rot.z );
        }

        public static void SetEulerZ( this Transform transform, float z )
        {
            Vector3 rot = transform.eulerAngles;
            transform.eulerAngles = new Vector3( rot.x, rot.y, z );
        }

        public static void SetLocalEulerX( this Transform transform, float x )
        {
            Vector3 rot = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3( x, rot.y, rot.z );
        }

        public static void SetLocalEulerY( this Transform transform, float y )
        {
            Vector3 rot = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3( rot.x, y, rot.z );
        }

        public static void SetLocalEulerZ( this Transform transform, float z )
        {
            Vector3 rot = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3( rot.x, rot.y, z );
        }

        public static void SetLocalPosX ( this Transform transform, float x )
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3( x, pos.y, pos.z );
        }

        public static void SetLocalPosY( this Transform transform, float y )
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3( pos.x, y, pos.z );
        }

        public static void SetLocalPosZ( this Transform transform, float z )
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3( pos.x, pos.y, z );
        }

        public static void SetPosX( this Transform transform, float x )
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3( x, pos.y, pos.z );
        }

        public static void SetPosY( this Transform transform, float y )
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3( pos.x, y, pos.z );
        }

        public static void SetPosZ( this Transform transform, float z )
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3( pos.x, pos.y, z );
        }

        public static void SetScaleX( this Transform transform, float x )
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3( x, scale.y, scale.z );
        }

        public static void SetScaleY( this Transform transform, float y )
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3( scale.x, y, scale.z );
        }

        public static void SetScaleZ( this Transform transform, float z )
        {
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3( scale.x, scale.y, z );
        }
        
        public static Bounds UnrotatedBounds( this Transform transform )
        {
            Quaternion startingRot = transform.rotation;
            transform.rotation = Quaternion.identity;

            Collider collider = transform.GetComponentInChildren<Collider>();
            Bounds bounds = new Bounds();
            if( collider != null && collider.bounds.extents.magnitude > 0 )
            {
                bounds = collider.bounds;
            }
            else
            {
                Renderer renderer = transform.GetComponentInChildren<Renderer>();
                if( renderer == null )
                {
                    Debug.LogError( "No Renderer or Collider attached to object!" );
                    transform.rotation = startingRot;
                    return new Bounds();
                }
                else
                {
                    bounds = renderer.bounds;
                }
            }
            transform.rotation = startingRot;
            return bounds;
        }

        //public static Bounds UnscaledBounds( this Transform transform )
        //{
        //    //TOOD
        //    return new Bounds();
        //}

        /// <summary>
        /// Returns the unrotated, unscaled bounds of given transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Bounds UnscaledAndUnrotatedBounds( this Transform transform )
        {
            Quaternion startingRot = transform.rotation;
            transform.rotation = Quaternion.identity;
            Vector3 startingScale = transform.localScale;
            transform.localScale = Vector3.one;

            Collider collider = transform.GetComponentInChildren<Collider>();
            Bounds bounds = new Bounds();
            if( collider != null  &&  collider.bounds.extents.magnitude > 0 )
            {
                bounds = collider.bounds;
            }
            else
            {
                Renderer renderer = transform.GetComponentInChildren<Renderer>();
                if( renderer == null )
                {
                    Debug.LogError( "No Renderer or Collider attached to object!" );
                    transform.rotation = startingRot;
                    transform.localScale = startingScale;
                    return new Bounds();
                }
                else
                {
                    bounds = renderer.bounds;
                }
            }
            transform.rotation = startingRot;
            transform.localScale = startingScale;
            return bounds;
        }


//--------------------------------------------------------------------------HELPERS:

    }
}