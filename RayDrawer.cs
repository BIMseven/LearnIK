using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class RayDrawer : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "RayDrawer";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    public bool Active = true;
    public bool PrintTextureCoordinate;
    public Color RayColor = Color.cyan;
    public float HitSize = 0.01f;

    public Vector2 LastHitCoordinate { get; private set; }

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
        if( Active )
        {
            drawRay();

            if( VERBOSE )
            {
                LOG_TAG.TPrint( "Coordinate: " + LastHitCoordinate.ToString( "F4" ) );
            }
            
        }        

	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
    private void drawRay()
    {
        float rayLen = 1000;
        RaycastHit hit;
        Ray ray = new Ray( transform.position, transform.forward * rayLen );
        if( Physics.Raycast( ray, out hit ) )
        {
            hitSphere.Visible = true;
            hitSphere.transform.position = hit.point;
            LastHitCoordinate = hit.textureCoord;
            
            rayLen = ( hit.point - transform.position ).magnitude;
            
        }
        else
        {
            hitSphere.Visible = false;
        }
        hitSphere.GetComponent<Renderer>().material.color = RayColor;
        Utility.DrawRay( ray, RayColor, rayLen );
    }
}