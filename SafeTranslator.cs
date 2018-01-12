using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;
/// <summary>
/// Has a Translate() method that checks for collisions
/// </summary>
public class SafeTranslator : MonoBehaviour 
{
    public enum Probes
    {
        ColliderBounds
            // TODO:
            //, Raycast, SphereCast
    }

//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "SafeTranslator";
	public bool VERBOSE = false;

    public float EPSILON = 0.001f;

//---------------------------------------------------------------------------FIELDS:
	
    public Probes Probe = Probes.ColliderBounds;

    public bool IgnoreTriggerColliders = true;

    public float MinMoveDist = 0.02f;
    private Collider probeCollider;

    public float TempSpeed;
        
//---------------------------------------------------------------------MONO METHODS:

    void Start()
    {
        probeCollider = GetComponent<Collider>();
    }

    //void Update()
    //{
    //    float dist = TempSpeed;
    //    if( Input.GetKey( KeyCode.UpArrow ) )
    //    {
    //        Translate( Vector3.forward * dist );
    //    }
    //    if( Input.GetKey( KeyCode.DownArrow ) )
    //    {
    //        Translate( Vector3.back * dist );
    //    }
    //    if( Input.GetKey( KeyCode.LeftArrow ) )
    //    {
    //        Translate( Vector3.left * dist );
    //    }
    //    if( Input.GetKey( KeyCode.RightArrow ) )
    //    {
    //        Translate( Vector3.right * dist );
    //    }
    //}

//--------------------------------------------------------------------------METHODS:

    public void Translate( Vector3 toTranslate, Space relativeTo = Space.Self )
    {
        switch( Probe )
        {
            case Probes.ColliderBounds:
                float moveDist = farthestCanMoveCollider( toTranslate );
                transform.Translate( toTranslate.normalized * moveDist );
                break;
        }

    }

    public void UseColliderForProbe( Collider collider )
    {
        probeCollider = collider;
        Probe = Probes.ColliderBounds;
    }

//--------------------------------------------------------------------------HELPERS:
	
    private float farthestCanMoveCollider( Vector3 toTranslate )
    {
        Bounds bounds = probeCollider.bounds;
        float distCanMove = toTranslate.magnitude;

        Vector3[] boundPoints = bounds.CornersAndCenter();

        // Go through each point in the bounds, shoot a ray from current center of
        // bounds to where they'd go if we moved by toTranslate
        foreach( Vector3 point in boundPoints )
        {
            Vector3 futurePoint = point + toTranslate;
            if( hitsCollider( bounds.center, futurePoint, IgnoreTriggerColliders ) )
            {
                return 0;
            }
        }
        return distCanMove;
    }

    // Returns the distance you can shoot a ray between from and to before hitting 
    // something
    private float distToClosestCollider( Vector3 from, Vector3 to, 
                                         bool ignoreTriggerColliders = true )
    {
        Vector3 proposedTranslation = to - from;
        Vector3 direction = proposedTranslation.normalized;

        Vector3 origin = from - direction.normalized * EPSILON;
        float dist = proposedTranslation.magnitude + 2 * EPSILON;
                
        RaycastHit[] hits = Physics.RaycastAll( origin, direction, dist );

        Debug.DrawRay( origin, direction * dist ) ;
        float minDist = dist;

        foreach( RaycastHit hit in hits )
        {
            if( hit.collider == probeCollider )  continue;

            if( ! ignoreTriggerColliders  ||  ! hit.collider.isTrigger )
            {
                print( "min: " + minDist );
                minDist = Mathf.Min( hit.distance, minDist );
            }
        }
        return minDist;
    }

    // Returns true if ray between from and to will hit a collider
    private bool hitsCollider( Vector3 from, Vector3 to,
                               bool ignoreTriggerColliders = true )
    {
        Vector3 proposedTranslation = to - from;
        Vector3 direction = proposedTranslation.normalized;

        Vector3 origin = from - direction.normalized * EPSILON;
        float dist = proposedTranslation.magnitude + 2 * EPSILON;

        RaycastHit[] hits = Physics.RaycastAll( origin, direction, dist );
        
        foreach( RaycastHit hit in hits )
        {
            if( hit.collider == probeCollider ) continue;

            if( ! ignoreTriggerColliders  ||  ! hit.collider.isTrigger )
            {
                return true;
            }
        }
        return false;
    }
}