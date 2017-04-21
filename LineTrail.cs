using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if( lastAddedSegment != null )
        {
            oldSegments.Add( lastAddedSegment );
        }
        lastAddedSegment = createNewLineSegement();
        lastAddedSegment.Stretch( from, to );
        lastAddedSegment.transform.parent = transform;
    }

    public void Init( Vector3 from, Vector3 to )
    {
        lastAddedSegment = createNewLineSegement();
        lastAddedSegment.Stretch( from, to );
    }

    public void UpdateLastAddedSegmentEndPoint( Vector3 newEndPoint )
    {
        lastAddedSegment.UpdateTarget( newEndPoint );
    }

//--------------------------------------------------------------------------HELPERS:

    private StretchyThing createNewLineSegement()
    {
        GameObject newLine = Instantiate( LinePrefab );
        StretchyThing newLineStretchyThing = newLine.GetComponent<StretchyThing>();
        if( newLineStretchyThing != null )
        {
            return newLineStretchyThing;
        }
        return newLine.AddComponent<StretchyThing>();
    }	
}