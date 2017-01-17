// KDNode.cs - A Stark, September 2009.
// original: http://forum.unity3d.com/threads/point-nearest-neighbour-search-class.29923/

using UnityEngine;
using System.Collections;


public class KDNode 
{
//------------------------------------------------------------------------CONSTANTS:
	
	private const string LOG_TAG = "KDNode";
	private const bool VERBOSE = true;

	private const int DIMENSIONS = 3;

	private const int LEFT = 0;
	private const int RIGHT = 1;

//---------------------------------------------------------------------------FIELDS:

	public KDNode[] Children { get; private set; }
	public Vector3 Pivot;
	public int PivotIndex;
	public int Axis;
	
//---------------------------------------------------------------------CONSTRUCTORS:
	
	private KDNode() 
	{
		Children = new KDNode[2];
	}
	
//--------------------------------------------------------------------------METHODS:
	
	//	Make a new tree from a list of points.
	public static KDNode Create( params Vector3[] points ) 
	{
		int[] indices = MyMath.Iota( points.Length );
        return makeFromPointsInner( 0, 0, points.Length - 1, points, indices );
    }

	//	Find the nearest point in the set to the supplied point. Returns the point's
	//  index (into the array from which this tree was generated)
	public int FindNearest( Vector3 point ) 
	{
		float bestSqDist = float.MaxValue;
		int bestIndex = -1;
		
		search( point, ref bestSqDist, ref bestIndex );
		
		return bestIndex;
	}

//--------------------------------------------------------------------------HELPERS:

	//	Get a point's distance from an axis-aligned plane.
	private float distFromSplitPlane( Vector3 point, Vector3 planePoint, int axis ) 
	{
		return point[axis] - planePoint[axis];
	}
	
	//	Simple "median of three" heuristic to find a reasonable splitting plane.
	private static int findSplitPoint( Vector3[] points, 
			                           int[] indices, 
			                           int start, int end, 
			                           int axis ) 
	{
		float a = points[indices[start]][axis];
		float b = points[indices[end]][axis];
		int midIndex = (start + end) / 2;
		float m = points[indices[midIndex]][axis];
		
		if (a > b) 
		{
			if (m > a)   return start;
			if (b > m)   return end;
			else         return midIndex;
		} 
		else 
		{
			if (a > m)   return start;
			if (m > b)   return end;
			else 		 return midIndex;
		}
	}
	
	
	//	Find a new pivot index from the range by splitting the points that fall either side
	//	of its plane.
	private static int findPivotIndex( Vector3[] points, 
	                                   int[] indices, 
	                                   int start, int end, 
	                                   int axis ) 
	{
		int splitPoint = findSplitPoint( points, indices, start, end, axis );
		
		Vector3 pivot = points[indices[splitPoint]];
		swapElements( indices, start, splitPoint );
		
		int currPt = start + 1;
		int endPt = end;
		
		while( currPt <= endPt ) 
		{
			Vector3 current = points[indices[currPt]];
			
			if( current[axis] > pivot[axis] ) 
			{
				swapElements( indices, currPt, endPt );
				endPt--;
			} 
			else 
			{
				swapElements( indices, currPt - 1, currPt );
				currPt++;
			}
		}		
		return currPt - 1;
	}

	//	Recursively build a tree by separating points at plane boundaries.
	private static KDNode makeFromPointsInner( int depth,
											   int start, int end,
											   Vector3[] points,
											   int[] indices ) 
	{		
		KDNode root = new KDNode();
		root.Axis = depth % DIMENSIONS;
		int splitPoint = findPivotIndex( points, indices, start, end, root.Axis );

		root.PivotIndex = indices[splitPoint];
		root.Pivot = points[root.PivotIndex];
		
		int leftEndIndex = splitPoint - 1;
		
		if( leftEndIndex >= start ) 
		{
			root.Children[LEFT] = makeFromPointsInner( depth + 1, 
				                                       start, leftEndIndex, 
				                                       points, 
				                                       indices);
		}
		
		int rightStartIndex = splitPoint + 1;
		
		if( rightStartIndex <= end ) 
		{
			root.Children[RIGHT] = makeFromPointsInner( depth + 1, 
				                                        rightStartIndex, end, 
				                                        points, 
				                                        indices );
		}		
		return root;
	}

	private static void swapElements( int[] array, int a, int b ) 
	{
		int temp = array[a];
		array[a] = array[b];
		array[b] = temp;
	}

	//	Recursively search the tree.
	private void search( Vector3 pt, ref float bestSqSoFar, ref int bestIndex ) 
	{
		float mySqDist = (Pivot - pt).sqrMagnitude;
		
		if( mySqDist < bestSqSoFar ) 
		{
			bestSqSoFar = mySqDist;
			bestIndex = PivotIndex;
		}
		
		float planeDist = pt[Axis] - Pivot[Axis]; //DistFromSplitPlane(pt, pivot, axis);
		
		int selector = planeDist <= 0 ? 0 : 1;
		
		if( Children[selector] != null) 
		{
			Children[selector].search( pt, ref bestSqSoFar, ref bestIndex );
		}		
		selector = (selector + 1) % 2;
		
		float sqPlaneDist = planeDist * planeDist;

		if( (Children[selector] != null) && (bestSqSoFar > sqPlaneDist) ) 
		{
			Children[selector].search( pt, ref bestSqSoFar, ref bestIndex );
		}
	}
	
}
