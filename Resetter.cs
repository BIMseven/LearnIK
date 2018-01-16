using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Resetter";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    #if RESETTER_DEBUG_MODE
    public KeyCode ResetPoitionKey;
    #endif

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
    private Transform initialParent;
    private Vector3 initialScale;

    private bool wasKinematic;
    private bool usedGravity;

//---------------------------------------------------------------------MONO METHODS:

    #if RESETTER_DEBUG_MODE
    void Start() 
	{
        RememberPosition();
    }

	void Update()
	{
        if( Input.GetKeyDown( ResetPoitionKey ) )
        {
            ResetObject();
        }
	}
    #endif

//--------------------------------------------------------------------------METHODS:

    public void RememberState()
    {
        initialScale = transform.localScale;

        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        initialParent = transform.parent;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if( rigidbody != null )
        {
            wasKinematic = rigidbody.isKinematic;
            usedGravity = rigidbody.useGravity;
        }
    }

    /// <summary>
    /// Returns object to initial global or local (depending on field UseGlobalPos)
    /// position and rotation.  Rigidbody's positional and rotational speed are 
    /// set to zero, and the object goes back to its initial parent.
    /// </summary>
    public void ResetObject()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if( rigidbody != null )
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        transform.parent = initialParent;
        transform.localScale = initialScale;

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

}