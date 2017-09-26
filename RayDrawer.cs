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
    public Color RayColor;
    public float HitSize = 0.1f;

    private VisibilityToggler hitSphere;
		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        var obj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
        obj.GetComponent<Collider>().enabled = false;
        hitSphere = obj.AddComponent<VisibilityToggler>();
	}
		
	void Update()
    {
        hitSphere.transform.localScale = Vector3.one * HitSize;
        if( Active )
        {
            RaycastHit hit;
            Ray ray = new Ray( transform.position, transform.forward * 1000 );
            if( Physics.Raycast( ray, out hit ) )
            {
                hitSphere.Visible = true;
                hitSphere.transform.position = hit.point;
            }
            else
            {
                hitSphere.Visible = false;
            }
            hitSphere.GetComponent<Renderer>().material.color = RayColor;
            Utility.DrawRay( ray, RayColor );
        }

	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}