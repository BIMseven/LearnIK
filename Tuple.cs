using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grabbed from http://gamedevrocks.com/tuples-for-unity/
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
[System.Serializable]
public class Tuple<T1, T2>
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Tuple";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    public T1 First;
    public T2 Second;

    private static IEqualityComparer item1Comparer;
    private static IEqualityComparer item2Comparer;

//---------------------------------------------------------------------CONSTRUCTORS:

    public Tuple( T1 first, T2 second )
    {
        item1Comparer = EqualityComparer<T1>.Default;
        item2Comparer = EqualityComparer<T2>.Default;
        this.First = first;
        this.Second = second;
    }

//--------------------------------------------------------------------------METHODS:

    public static bool operator ==( Tuple<T1, T2> a, Tuple<T1, T2> b )
    {
        if( Tuple<T1, T2>.IsNull( a )  &&  ! Tuple<T1, T2>.IsNull( b ) )
            return false;

        if( !Tuple<T1, T2>.IsNull( a )  &&  Tuple<T1, T2>.IsNull( b ) )
            return false;

        if( Tuple<T1, T2>.IsNull( a )  &&  Tuple<T1, T2>.IsNull( b ) )
            return true;

        return
            a.First.Equals( b.First ) &&
            a.Second.Equals( b.Second );
    }

    public static bool operator !=( Tuple<T1, T2> a, Tuple<T1, T2> b )
    {
        return !( a == b );
    }

    public override bool Equals( object obj )
    {
        var other = obj as Tuple<T1, T2>;

        if( object.ReferenceEquals( other, null ) ) return false;

        return item1Comparer.Equals( First, other.First ) &&
               item2Comparer.Equals( Second, other.Second );
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + First.GetHashCode();
        hash = hash * 23 + Second.GetHashCode();
        return hash;
    }

    private static bool IsNull( object obj )
    {
        return object.ReferenceEquals( obj, null );
    }
   
    public override string ToString()
    {
        return string.Format( "<{0}, {1}>", First, Second );
    }

//--------------------------------------------------------------------------HELPERS:

}