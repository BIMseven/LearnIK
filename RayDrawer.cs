using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class RayDrawer : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "RayDrawer";

//---------------------------------------------------------------------------FIELDS:

    public bool PrintTextureCoordinate;
    public bool PrintDistance;
    public bool PrintScreenSpaceCoordinate;

    public Camera CameraOfInterest;

    public Color RayColor = Color.cyan;
    public float HitSize = 0.01f;

    public Vector2 LastHitCoordinate { get; private set; }

    public GameObject LastHitObject { get; private set; }

    public float LastHitDistance { get; private set; }
    public Vector3 LastHitPoint
    {
        get
        {
            return hitSphere.transform.position;
        }
    }

    private Ray lastCastRay;
    private VisibilityToggler hitSphere;
        
//---------------------------------------------------------------------MONO METHODS:

    void Start() 
    {
        var obj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
        obj.GetComponent<Collider>().enabled = false;
        obj.name = "RayDrawer Hit";
        hitSphere = obj.AddComponent<VisibilityToggler>();
    }
        
    void Update()
    {
        hitSphere.transform.localScale = Vector3.one * HitSize;

        drawRay();

        if( PrintTextureCoordinate )
        {
            LOG_TAG.TPrint( "Tex Coordinate: " + LastHitCoordinate.ToString( "F4" ) );
        }

        if( PrintDistance )
        {
            LOG_TAG.TPrint( "Distance: " + LastHitDistance.ToString( "F4" ) );  
        }
        
        if( PrintScreenSpaceCoordinate  &&  CameraOfInterest != null )
        {
            var screenSpace = CameraOfInterest.ToScreenSpace( lastCastRay );
            LOG_TAG.TPrint( "Screen Space Coordinate: " + screenSpace.ToString( "F4" ) );
        }


    }

    
//--------------------------------------------------------------------------HELPERS:

    private void drawRay()
    {
        float rayLen = 1000;
        RaycastHit hit;
        lastCastRay = new Ray( transform.position, transform.forward * rayLen );
        if( Physics.Raycast( lastCastRay, out hit ) )
        {
            hitSphere.Visible = true;
            hitSphere.transform.position = hit.point;
            LastHitCoordinate = hit.textureCoord;
            LastHitObject = hit.collider.gameObject;
            LastHitDistance = hit.distance;
            rayLen = ( hit.point - transform.position ).magnitude;
        }
        else
        {
            hitSphere.Visible = false;
        }
        hitSphere.GetComponent<Renderer>().material.color = RayColor;
        Utility.DrawRay( lastCastRay, RayColor, rayLen );
    }
}