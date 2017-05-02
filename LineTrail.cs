using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class LineTrail : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "LineTrail";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    public GameObject LinePrefab;
        
    public Vector3 LastAddedOrigin
    {
        get
        {
            if( lastAddedSegment != null )
            {
                return lastAddedSegment.Origin;
            }
            return Vector3.zero;
        }
    }

    public Vector3 LastAddedTarget
    {
        get
        {
            if( lastAddedSegment != null )
            {
                return lastAddedSegment.Target;
            }
            return Vector3.zero;
        }
    }

    public Color Color
    {
        get
        {
            Renderer renderer = GetComponent<Renderer>();
            if( renderer != null )   return renderer.material.color;
            return Color.black;
        }
    }

    private List<StretchyThing> oldSegments;
    private StretchyThing lastAddedSegment;
    		
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        oldSegments = new List<StretchyThing>();
	}

    //--------------------------------------------------------------------------METHODS:

    public void AddSegment( Vector3 from, Vector3 to )
    {
        AddSegment( from, to, Vector3.one );
    }

    public void AddSegment( Vector3 from, Vector3 to, Vector3 scale )
    {
        if( lastAddedSegment != null )
        {
            oldSegments.Add( lastAddedSegment );
        }
        lastAddedSegment = createNewLineSegement( scale );
        lastAddedSegment.Stretch( from, to );
        lastAddedSegment.transform.parent = transform;
    }

    public void EnableCollidersInOldSegmentsOnly()
    {
        if( lastAddedSegment != null )
        {
            lastAddedSegment.gameObject.EnableCollidersInChildren( false );
        }
        foreach( StretchyThing thing in oldSegments )
        {
            thing.gameObject.EnableCollidersInChildren( true );
        }
    }

    public List<Vector3> GetAllPointsInLine()
    {
        List<Vector3> points = new List<Vector3>();
        if( lastAddedSegment == null )   return null;
        foreach( StretchyThing segment in oldSegments )
        {
            points.Add( segment.Origin );
        }
        if( points.Count == 0 )  points.Add( lastAddedSegment.Origin );
        points.Add( lastAddedSegment.Target );
        return points;
    }

    public bool GoesOverPoint( Vector3 point )
    {
        foreach( StretchyThing segment in oldSegments )
        {
            if( segment.gameObject.GetBounds().Contains( point ) )
            {
                return true;
            }
        }
        if( lastAddedSegment != null  &&
            lastAddedSegment.gameObject.GetBounds().Contains( point ) )
        {
            return true;
        }
        return false;
    }
    
    public void RemoveLastAddedSegment()
    { 
        if( lastAddedSegment != null )
        {
            Destroy( lastAddedSegment.gameObject );
        }
        lastAddedSegment = oldSegments.Pop<StretchyThing>();
    }

    public void UpdateLastAddedSegmentEndPoint( Vector3 newEndPoint )
    {
        lastAddedSegment.UpdateTarget( newEndPoint );
    }

//--------------------------------------------------------------------------HELPERS:

    private StretchyThing createNewLineSegement( Vector3 scale )
    {
        GameObject newLine = Instantiate( LinePrefab );
        newLine.transform.localScale = scale;
        StretchyThing newLineStretchyThing = newLine.GetComponent<StretchyThing>();
        if( newLineStretchyThing != null )
        {
            return newLineStretchyThing;
        }
        return newLine.AddComponent<StretchyThing>();
    }	
}