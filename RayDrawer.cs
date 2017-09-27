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
            if( PrintTextureCoordinate )
            {
                LOG_TAG.TPrint( "Coordinate: " + hit.textureCoord.ToString( "F4" ) );
            }
            rayLen = ( hit.point - transform.position ).magnitude;

            if( Input.GetKeyDown( KeyCode.D ) )
            {
                print( "clicking at: " + hit.textureCoord.ToString( "F4" ) );
                BrowserInputManager.Instance.SetMousePosition( hit.textureCoord.x, hit.textureCoord.y );
                BrowserInputManager.Instance.LeftClickDown();
            }
            if( Input.GetKeyDown( KeyCode.U ) )
            {
                print( "clicking at: " + hit.textureCoord.ToString( "F4" ) );
                BrowserInputManager.Instance.SetMousePosition( hit.textureCoord.x, hit.textureCoord.y );
                BrowserInputManager.Instance.LeftClickUp();
            }
            if( Input.GetKeyDown( KeyCode.C ) )
            {
                print( "clicking at: " + hit.textureCoord.ToString( "F4" ) );
                BrowserInputManager.Instance.LeftClick( hit.textureCoord.x, hit.textureCoord.y );
            }
        }
        else
        {
            hitSphere.Visible = false;
        }
        hitSphere.GetComponent<Renderer>().material.color = RayColor;
        Utility.DrawRay( ray, RayColor, rayLen );
    }
}