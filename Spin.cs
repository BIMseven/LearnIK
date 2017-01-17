using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Spin";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	public float spinSpeed;
	public Vector3 spinAxis;
		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		spinAxis = transform.InverseTransformDirection( spinAxis.normalized );
	}
		
	void FixedUpdate()
	{
		transform.Rotate( spinAxis * spinSpeed * Time.deltaTime );
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}