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

    public bool UseGlobalPosition;
    public float DistanceFromStartPosition
    {
        get
        {
            if( UseGlobalPosition )
            {
                return ( transform.position - initialPosition ).magnitude;
            }
            return ( transform.localPosition - initialLocalPosition ).magnitude;
        }
    }
    
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private bool wasKinematic;
    private bool usedGravity;

//---------------------------------------------------------------------MONO METHODS:

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
        }
        if( UseGlobalPosition )
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        else
        {
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
        }
    }

//--------------------------------------------------------------------------HELPERS:

    private void rememberStartingState()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        initialPosition = transform.position;
        initialRotation = transform.rotation;


        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if( rigidbody != null )
        {
            wasKinematic = rigidbody.isKinematic;
            usedGravity = rigidbody.useGravity;
        }
    }	
}