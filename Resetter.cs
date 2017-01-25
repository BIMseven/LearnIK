using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Resetter";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    public KeyCode ResetPoitionKey;

    private Vector3 initialLocalPosition;
    private Quaternion initialRotation;

    private bool wasKinematic;
    private bool usedGravity;

//---------------------------------------------------------------------MONO METHODS:

    void Reset()
    {
        Debug.Log( "Resetting ressetter!" );
        GoToInitialPosition();
    }

    void Start() 
	{
        rememberStartingState();
    }
		
	void Update()
	{
        if( Input.GetKeyDown( ResetPoitionKey ) )
        {
            GoToInitialPosition();
        }
	}

//--------------------------------------------------------------------------METHODS:

    public void GoToInitialPosition()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if( rigidbody != null )
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = wasKinematic;
            rigidbody.useGravity = usedGravity;
        }
        transform.localPosition = initialLocalPosition;
        transform.rotation = initialRotation;
    }

//--------------------------------------------------------------------------HELPERS:

    private void rememberStartingState()
    {
        initialLocalPosition = transform.localPosition;
        initialRotation = transform.rotation;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if( rigidbody != null )
        {
            wasKinematic = rigidbody.isKinematic;
            usedGravity = rigidbody.useGravity;
        }
    }	
}