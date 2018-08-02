using UnityEngine;
using System.Collections;

namespace MyUtility
{
    // It also enables limits to be set on the motion in any given axis
    public class AxisSwapper : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "AxisSwapper";
        private const bool VERBOSE = true;
        
//---------------------------------------------------------------------------FIELDS:

        #region Inspector Fields
        public Vector3 LocalForward     = Vector3.forward;
        public Vector3 LocalUp          = Vector3.up;
        #endregion

        public float Roll
        {
            get
            {
                return transform.eulerAngles[rollComp];
            }

            set
            {
                Vector3 angles = transform.eulerAngles;
                SetAttitude( value % 360, angles[pitchComp], angles[yawComp] );
            }
        }

        public float Pitch
        {
            get
            {
                return transform.eulerAngles[pitchComp];
            }

            set
            {
                Vector3 angles = transform.eulerAngles;
                SetAttitude( angles[rollComp], value % 360, angles[yawComp] );
            }
        }

        public float Yaw
        {
            get
            {
                return transform.eulerAngles[yawComp];
            }

            set
            {
                Vector3 angles = transform.eulerAngles;
                SetAttitude( angles[rollComp], angles[pitchComp], value % 360 );
            }
        }

        public Vector3 Forward
        {
            get
            {
                return transform.TransformDirection( LocalForward );
            }
        }

        public Vector3 Up
        {
            get
            {
                return transform.TransformDirection( LocalUp );
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Cross( Up, Forward );                
            }
        }
        
        public Quaternion LocalRotation
        {
            get
            {
                return transform.parent.rotation.Inverse() * Rotation;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return Quaternion.LookRotation( Forward, Up );
            }
        }

        // These fields specify the components of localEulerAngles to which the roll, 
        // pitch, and yaw apply. Each will be a Vector3 index
        protected int rollComp = 2;
        protected int pitchComp = 0;
        protected int yawComp = 1;
        private Quaternion toAdjustedLocal;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            initAxisSwapper();            
        }
        
//--------------------------------------------------------------------------METHODS:

        public void LookAt( Transform target, Vector3 up )
        {
            Vector3 toTarget = target.position - transform.position;
            transform.rotation = LookRotation( toTarget, up );
        }

        public void LookAt( Transform target )
        {
            Vector3 toTarget = target.position - transform.position;
            transform.rotation = LookRotation( toTarget, Vector3.up );
        }

        public void LookAt( Vector3 forward, Vector3 up )
        {
            transform.rotation = LookRotation( forward,up );
        }

        public void LookAt( Vector3 forward )
        {
            transform.rotation = LookRotation( forward, Vector3.up );
        }

        /// <summary>
        /// Gives the "real" local rotation of given local rotation
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public Quaternion LocalLookRotation( Quaternion unadjustedLocalRotation )
        {

            Vector3 forward = toAdjustedLocal * unadjustedLocalRotation * Vector3.forward;
            Vector3 up      = toAdjustedLocal * unadjustedLocalRotation * Vector3.up;
            return Quaternion.LookRotation( forward, up );
        }

        /// <summary>
        /// Gives the "real" local rotation of given local forward and up Vectors
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public Quaternion LocalLookRotation( Vector3 forward, Vector3 up )
        {
            forward = toAdjustedLocal * forward;
            up      = toAdjustedLocal * up;
            return Quaternion.LookRotation( forward, up );
        }

        /// <summary>
        /// Gives the "real" local rotation of given local forward and up Vectors
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <returns></returns>
        public Quaternion LocalLookRotation( Vector3 forward )
        {
            forward     = toAdjustedLocal * forward;
            Vector3 up  = toAdjustedLocal * Vector3.up;
            return Quaternion.LookRotation( forward, up );
        }

        public Quaternion LookRotation( Vector3 forward  )
        {
            var targetRot = Quaternion.LookRotation( forward );
            forward = targetRot * toAdjustedLocal * Vector3.forward;
            Vector3 up = targetRot * toAdjustedLocal * Vector3.up;
            return Quaternion.LookRotation( forward, up );
        }

        public Quaternion LookRotation( Vector3 forward, Vector3 up )
        {
            var targetRot = Quaternion.LookRotation( forward, up );
            forward = targetRot * toAdjustedLocal * Vector3.forward;
            up      = targetRot * toAdjustedLocal * Vector3.up;
            return Quaternion.LookRotation( forward, up );
        }

        public Quaternion LookRotation( Quaternion targetRotation )
        {
            Vector3 forward = targetRotation * toAdjustedLocal * Vector3.forward;
            Vector3 up      = targetRotation * toAdjustedLocal * Vector3.up;
            return Quaternion.LookRotation( forward, up ); 
        }

        public void SetAttitude( float roll, float pitch, float yaw )
        {
            Vector3 eulers = new Vector3( pitch, yaw, roll );
            SetAttitude( Quaternion.Euler( eulers ) );
        }

        public void SetAttitude( Quaternion attitude )
        {
            Vector3 forward = attitude * toAdjustedLocal * Vector3.forward;
            Vector3 up      = attitude * toAdjustedLocal * Vector3.up;
            transform.rotation = Quaternion.LookRotation( forward, up ); 
        }
        
        public void SetLocalAttitude( float roll, float pitch, float yaw )
        {
            Vector3 eulers = new Vector3( pitch, yaw, roll );
            SetLocalAttitude( Quaternion.Euler( eulers ) );
        }

        public void SetLocalAttitude( Quaternion attitude )
        {
            Vector3 forward = attitude * toAdjustedLocal * Vector3.forward;
            Vector3 up      = attitude * toAdjustedLocal * Vector3.up;
            transform.localRotation = Quaternion.LookRotation( forward, up ); 
        }

        public void SetLocalPosition( Vector3 position )
        {
            transform.localPosition = toAdjustedLocal * position;
        }

//--------------------------------------------------------------------------HELPERS:

        protected void initAxisSwapper()
        {
            // find roll
            if( LocalForward.x != 0 )       rollComp = 0;
            else if( LocalForward.y != 0 )  rollComp = 1;
            else                            rollComp = 2;
            
            // find yaw
            if( LocalUp.x != 0 )            yawComp = 0;
            else if( LocalUp.y != 0 )       yawComp = 1;
            else                            yawComp = 2;

            // find pitch
            if( rollComp != 0   && yawComp != 0 )         pitchComp = 0;
            else if( rollComp != 1   && yawComp != 1 )    pitchComp = 1;
            else                                          pitchComp = 2;

            LocalForward = LocalForward.normalized;
            LocalUp = LocalUp.normalized;
            
            toAdjustedLocal = Quaternion.LookRotation( LocalForward, LocalUp );
            toAdjustedLocal = Quaternion.Inverse( toAdjustedLocal );            
        }

    }
}