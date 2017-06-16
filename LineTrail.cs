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
        
    public Vector3 ActiveSegmentOrigin
    {
        get
        {
            if( activeSegment != null )
            {
                return activeSegment.Origin;
            }
            return Vector3.zero;
        }
    }

    public Vector3 EndPoint
    {
        get
        {
            if( activeSegment != null )
            {
                return activeSegment.Target;
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
    private StretchyThing activeSegment;
    		
//---------------------------------------------------------------------MONO METHODS:

    void Awake()
    {
        Clear();
    }
    
//--------------------------------------------------------------------------METHODS:

    public void AddSegment( Vector3 from, Vector3 to )
    {
        AddSegment( from, to, Vector3.one );
    }

    public void AddSegment( Vector3 from, Vector3 to, Vector3 scale )
    {
        // lastAddedSegment is now old and will be replaced with new segment
        if( activeSegment != null )
        {
            oldSegments.Add( activeSegment );
        }
        activeSegment = createNewLineSegement( scale );
        activeSegment.Stretch( from, to );
        activeSegment.transform.parent = transform;
    }

    public void Clear()
    {
        if( oldSegments != null )
        {
            foreach( StretchyThing segment in oldSegments )
            {
                GameObject.Destroy( segment.gameObject );
            }
        }
        if( activeSegment != null )
        {
            GameObject.Destroy( activeSegment.gameObject );
        }
        oldSegments = new List<StretchyThing>();
        activeSegment = null;
    }

    public void EnableCollidersInOldSegmentsOnly()
    {
        if( activeSegment != null )
        {
            activeSegment.gameObject.EnableCollidersInChildren( false );
        }
        foreach( StretchyThing thing in oldSegments )
        {
            thing.gameObject.EnableCollidersInChildren( true );
        }
    }

    public List<Vector3> GetAllPointsInLine()
    {
        List<Vector3> points = new List<Vector3>();
        if( activeSegment == null )   return null;
        foreach( StretchyThing segment in oldSegments )
        {
            points.Add( segment.Origin );
        }
        if( points.Count == 0 )  points.Add( activeSegment.Origin );
        points.Add( activeSegment.Target );
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
        if( activeSegment != null  &&
            activeSegment.gameObject.GetBounds().Contains( point ) )
        {
            return true;
        }
        return false;
    }
    
    public void RemoveLastAddedSegment()
    { 
        if( activeSegment != null )
        {
            Destroy( activeSegment.gameObject );
        }
        activeSegment = oldSegments.Pop<StretchyThing>();
    }

    /// <summary>
    /// Updates the end point of the trail
    /// </summary>
    /// <param name="newEndPoint"></param>
    public void UpdateEndPoint( Vector3 newEndPoint )
    {
        if( activeSegment != null )
        {
            activeSegment.UpdateTarget( newEndPoint );
        }
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