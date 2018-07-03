using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUtility
{
    /// <summary>
    /// This class is designed to present a GUI to a monitor while administering 
    /// a VR experiment.  Its parent should be the GameCamera
    /// </summary>
    [RequireComponent(typeof( Camera ))]
    public class ExperimenterCamera : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "ExperimenterCamera";
        public bool VERBOSE = false;

        private const int RIGHT_CLICK = 1;

//---------------------------------------------------------------------------FIELDS:

        private Camera experimenterCamera;

        public Text[] ConsoleTexts;
        public GameObject ConsolePanel;


        private bool controllingCamera;
        private Transform originalParent;


        private Vector3 initialLocalPos;
        private Quaternion initialLocalRot;


        // For mouse look
        private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        private RotationAxes axes = RotationAxes.MouseXAndY;
        private float sensitivityX = 15F;
        private float sensitivityY = 15F;
        private float minimumX = -360F;
        private float maximumX = 360F;
        private float minimumY = -60F;
        private float maximumY = 60F;
        private float moveSpeed = 1;
        float rotationX = 0F;
        float rotationY = 0F;
        private Quaternion startRot;
        //////

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            experimenterCamera = GetComponent<Camera>();
            originalParent = transform.parent;
            initialLocalRot = transform.localRotation;
            initialLocalPos = transform.localPosition;
        }

        void Update()
        {
            if( ! ConsolePanel.activeInHierarchy )   return;


            if( Input.GetMouseButtonDown( RIGHT_CLICK )   &&  ! controllingCamera )
            {
                controllingCamera = true;
                startRot = transform.rotation;
                transform.parent = null;
            }
            if( controllingCamera )
            {
                float scroll = Input.GetAxis( "Mouse ScrollWheel" );
                var movement = scroll * transform.forward;
                transform.Translate( movement );
                if( Input.GetMouseButton( 1 ) ) rotateCameraWithMouse();
            }
        }

//--------------------------------------------------------------------------METHODS:

        public void Print( string message )
        {
            for( int i = ConsoleTexts.Length - 1; i > 0; i-- )
            {
                int prev = i - 1;
                if( ConsoleTexts[prev].enabled )
                {
                    ConsoleTexts[i].enabled = true;
                    ConsoleTexts[i].text = ConsoleTexts[prev].text;
                }
            }
            ConsoleTexts[0].text = message;
            ConsoleTexts[0].enabled = true;
        }

        public void ResetCamera()
        {
            transform.parent = originalParent;
            transform.localPosition = initialLocalPos;
            transform.localRotation = initialLocalRot;
            controllingCamera = false   ;
        }

        public void ToggleCamera()
        {
            vLog( "Toggle camera" );
            experimenterCamera.enabled = ! experimenterCamera.enabled;
        }

        public void ToggleConsole()
        {
            ConsolePanel.SetActive( ! ConsolePanel.activeInHierarchy );
        }

//--------------------------------------------------------------------------HELPERS:

        private float clampAngle( float angle, float min, float max )
        {
            if( angle < -360F)   angle += 360F;
            if( angle > 360F)    angle -= 360F;
            return Mathf.Clamp( angle, min, max );
        }
        private void rotateCameraWithMouse()
        {
            if( axes == RotationAxes.MouseXAndY )
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis( "Mouse X" ) * sensitivityX;
                rotationY += Input.GetAxis( "Mouse Y" ) * sensitivityY;
                rotationX = clampAngle( rotationX, minimumX, maximumX );
                rotationY = clampAngle( rotationY, minimumY, maximumY );
                Quaternion xQuaternion = Quaternion.AngleAxis( rotationX, Vector3.up );
                Quaternion yQuaternion = Quaternion.AngleAxis( rotationY, -Vector3.right );
                transform.rotation = startRot * xQuaternion * yQuaternion;
            }
            else if( axes == RotationAxes.MouseX )
            {
                rotationX += Input.GetAxis( "Mouse X" ) * sensitivityX;
                rotationX = clampAngle( rotationX, minimumX, maximumX );
                Quaternion xQuaternion = Quaternion.AngleAxis( rotationX, Vector3.up );
                transform.rotation = startRot * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis( "Mouse Y" ) * sensitivityY;
                rotationY = clampAngle( rotationY, minimumY, maximumY );
                Quaternion yQuaternion = Quaternion.AngleAxis( -rotationY, Vector3.right );
                transform.rotation = startRot * yQuaternion;
            }
        }

        private void init()
        {
            experimenterCamera.targetDisplay = 2;
            experimenterCamera.enabled = false;

            foreach( Text text in ConsoleTexts )
            {
                text.enabled = false;
            }
        }

        private void vLog( string message )
        {
            if( VERBOSE ) LOG_TAG.TPrint( message );
        }




    }

}