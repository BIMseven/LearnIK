using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Drift";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    public Vector3 Direction;
    public float Speed;

    public Vector3 RotateAxis;
    public float AngularVelocity;	

//---------------------------------------------------------------------MONO METHODS:

    void FixedUpdate()
    {
        transform.Translate( Direction.normalized * Speed * Time.deltaTime );
        transform.Rotate( RotateAxis.normalized, AngularVelocity * Time.deltaTime );
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}