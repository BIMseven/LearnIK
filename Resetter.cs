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
                return ( transform.position - InitialPosition ).magnitude;
            }
            return ( transform.localPosition - InitialLocalPosition ).magnitude;
        }
    }
    
    public Vector3 InitialPosition { get; private set; }
    public Quaternion InitialRotation { get; private set; }
    public Vector3 InitialLocalPosition { get; private set; }
    public Quaternion InitialLocalRotation { get; private set; }
    public Transform initialParent;
    public Vector3 initialScale;

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

        InitialLocalPosition = transform.localPosition;
        InitialLocalRotation = transform.localRotation;

        InitialPosition = transform.position;
        InitialRotation = transform.rotation;

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
            transform.position = InitialPosition;
            transform.rotation = InitialRotation;
        }
        else
        {
            transform.localPosition = InitialLocalPosition;
            transform.localRotation = InitialLocalRotation;
        }
    }
        
//--------------------------------------------------------------------------HELPERS:

}