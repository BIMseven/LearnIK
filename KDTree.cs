// KDTree.cs - A Stark, September 2009.
// original: http://forum.unity3d.com/threads/point-nearest-neighbour-search-class.29923/

using UnityEngine;
using System.Collections;


public class KDTree 
{
//------------------------------------------------------------------------CONSTANTS:
	
	private const string LOG_TAG = "KDTree";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	private KDNode tree;
	private Vector3[] points;
	
//---------------------------------------------------------------------CONSTRUCTORS:
	
	public KDTree( Vector3[] points ) 
	{
		this.points = points;
		tree = KDNode.Create( points );	
	}
	
//--------------------------------------------------------------------------METHODS:

	//	Find the nearest point in the set to the supplied point.
	public Vector3 FindNearest( Vector3 point ) 
	{
		int nearestIndex = tree.FindNearest( point );
		return points[nearestIndex];
	}

}
