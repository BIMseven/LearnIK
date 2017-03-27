using UnityEngine;
using System.Collections;

public static class MyMath
{	
//-------------------------------------------------------------CONSTANTS AND FIELDS:
	
	public static float EPSILON = 0.0001f;

    // Euler's number
    public static float E = 2.71828182845904523536f;

    private static int X = 0, Y = 1, Z = 2;

	private static float MAX_RAY_LENGTH = 1000.0f;

	// For Intersection( Ray a, Ray b )
	private static float MAX_INTERSECTION_DISTANCE = 100;
	
//--------------------------------------------------------------------------METHODS:

	public static float Average( float[] numbers )
	{
		float sum = 0.0f;
		for( int i = 0; i < numbers.Length; i++ ) 
		{
			sum += numbers[i];
		}
		return sum / numbers.Length;
	}

	public static float Difference( float a, float b )
	{
		return Mathf.Abs( Mathf.Abs( a ) - Mathf.Abs( b ) );
	}

	// TODO: optimize
	public static float DifferenceOfWrapped( float a, float b, float min, float max )
	{
		float larger = Mathf.Max( a, b );
		float smaller = Mathf.Min( a, b );
		return Mathf.Min( Difference( larger, max ) + Difference( smaller, min ),
		                  Difference( a, b ) );
	}
    
	/// <summary>
	/// Returns the point of intersection of given Rays if it exists.  Else, returns
	/// Vector2.zero
	/// </summary>
	/// <param name="a">The first Ray.</param>
	/// <param name="b">The second Ray.</param>
	public static Vector2 Intersection( Vector2 aPos, Vector2 aDir,
										Vector2 bPos, Vector2 bDir )
	{		
		aDir = aDir.normalized * MAX_RAY_LENGTH;
		bDir = bDir.normalized * MAX_RAY_LENGTH;

		float t = IntersectionT( aPos, aDir, bPos, bDir );

		// If t is between 0 and 1, we've hit!
		if( t > 0 )   
			return aPos + t * aDir;

		// Else, we won't hit
		return Vector2.zero;
	}

	/// <summary>
	/// Returns the t value of intersection point for given Ray a.  That is, the
	/// point of collision will be aPos + t * aDir
	/// </summary>
	/// <param name="a">The first Ray.</param>
	/// <param name="b">The second Ray.</param>
	public static float IntersectionT( Vector2 aPos, Vector2 aDir,
									   Vector2 bPos, Vector2 bDir )
	{
		//http://stackoverflow.com/questions/2931573/determining-if-two-rays-intersect

		float denom = ( aDir.x * bDir.y - aDir.y * bDir.x );

		if( denom == 0 )  return -1;

		return ( aPos.y * bDir.x + bDir.y * bPos.x - 
				 bPos.y * bDir.x - bDir.y * aPos.x )  /  denom;				  
	}
	/*
	 * Returns an array of consecutive integers from 0 up to length
	 */
	public static int[] Iota( int length ) 
	{
		int[] iota = new int[length];
		
		for( int i = 0; i < length; i++ ) 
		{
			iota[i] = i;
		}		
		return iota;
	}
    
    /// <summary>
    /// Returns true if given number is Even
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool IsEven( this int num )
    {
        return num % 2 == 0;
    }

    /// <summary>
    /// Returns true if given number is Odd
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool IsOdd( this int num )
    {
        return num % 2 == 1;
    }

	/**
	 * Logarithmically interpolates between x1 and x2 for given f (float between
	 * 0.0f and 1.0f)
	 */
	public static float LogarithmicallyInterpolate( float x1, float x2, float f )
	{
		// Derivation: http://www.cmu.edu/biolphys/deserno/pdf/log_interpol.pdf
		return Mathf.Pow( x1, f ) * Mathf.Pow( x2, 1 - f );
	}

    /// <summary>
    /// Applies a Logistic function to given input x.  This is a Sigmoid function.
    /// https://en.wikipedia.org/wiki/Logistic_function
    /// </summary>
    /// <param name="x"></param>
    /// <param name="maxValue"></param>
    /// <param name="midpoint"></param>
    /// <param name="steepness"></param>
    /// <returns></returns>
    public static float Logistic( float x, 
                                  float maxValue, 
                                  float midpoint, 
                                  float steepness )
    {
        // L / ( 1 + e ^ -k(x-x0) )
        return maxValue / ( 1 + Mathf.Pow( E, -steepness * ( x - midpoint ) ) );
    }

	/**
	 * Maps given number, which is between oldMin and oldMax, to a value between 
	 * newMin and newMax
	 */
	public static float Map( float number, 
	                         float oldMin, float oldMax, 
	                         float newMin, float newMax )
	{
		float oldRange = oldMax - oldMin;
		float newRange = newMax - newMin;
		return ( ( (number - oldMin) * newRange ) / oldRange ) + newMin;
	}

	/**
	 * Returns the index of the primary component of given vector (0 for x,
	 * 1 for y, 2 for z)
	 */
	public static int MostSignificantAxis( Vector3 vector )
	{
		float x = Mathf.Abs( vector.x );
		float y = Mathf.Abs( vector.y );
		float z = Mathf.Abs( vector.z );

		if( x >= y  &&  x >= z )  return X;
		if( y >= x  &&  y >= z )  return Y;
		return Z;
	}

	public static bool NearlyEqual( Vector3 a, Vector3 b )
	{
		return NearlyEqual( a.x, b.x ) &&
			   NearlyEqual( a.y, b.y ) &&
			   NearlyEqual( a.z, b.z );
	}

	public static bool NearlyEqual( float a, float b )
	{
		return NearlyEqual( a, b, EPSILON );
	}
	
	/* From Norm Badler's slides */
	public static bool NearlyEqual( float a, float b, float epsilon )
	{
		float absA = Mathf.Abs( a );
	    float absB = Mathf.Abs( b );
	    float diff = Mathf.Abs( a - b );

		// shortcut
		if( a == b ) 
		{ 	
			return true;
	    } 
		else if( a * b == 0 ) 
		{ 	// a or b or both are zero -- relative error is not meaningful here
			return diff < (epsilon * epsilon);
	    } 
		else 
		{ 	// use relative error
			return diff / ( absA + absB ) < epsilon;
	    }
	}
		
	public static Vector3 PositionWS( GameObject thing )
	{
		return thing.transform.TransformPoint( Vector3.zero );
	}

    /// <summary>
    /// Returns the length of the third side of right triangle specified by a and b
    /// </summary>
    /// <param name="a">Side A length</param>
    /// <param name="b">Side B length</param>
    /// <returns>Length of side C</returns>
    public static float Pythatgorize( float a, float b )
    {
        return Mathf.Sqrt( a * a + b * b );
    }

	/**
	 * Returns -1 if num is negative, 1 if positive, 0 if 0
	 */
	public static int Sign( float num )
	{
		if( num == 0 )    return 0;
		if( num > 0 )    return 1;
		return -1;
	}

	public static Vector3 PositionTargetSpace( GameObject thing, 
	                                           Transform targetSpace )
	{
		return targetSpace.InverseTransformPoint( PositionWS( thing ) );
	}
    
    // Generate a random point within the given Bounds
    public static Vector3 RandomPointInBounds( Bounds bounds )
    {
        return RandomVectorInRange( bounds.min, bounds.max );
    }

    public static Vector2 RandomVectorInRange( Vector2 min, Vector2 max )
    {
        return new Vector2( UnityEngine.Random.Range( min.x, max.x ),
                            UnityEngine.Random.Range( min.y, max.y ) );
    }

    public static Vector3 RandomVectorInRange( Vector3 min, Vector3 max )
    {
        return new Vector3( UnityEngine.Random.Range( min.x, max.x ),
                            UnityEngine.Random.Range( min.y, max.y ),
                            UnityEngine.Random.Range( min.z, max.z ) );
    }

    public static Vector3 RoundComponents( Vector3 vector )
	{
		return new Vector3( Mathf.Round( vector.x ),
		                    Mathf.Round( vector.y ),
		                    Mathf.Round( vector.z ) );
	}

	public static Vector3 RoundDirection( Vector3 vector )
	{
		int axis = MostSignificantAxis( vector );
		Vector3 roundedVector = Vector3.zero;

		if( vector[axis] >= 0 )
		{
			roundedVector[axis] = 1.0f;
		}
		else
		{
			roundedVector[axis] = -1.0f;
		}
		return roundedVector;
	}

	/*
	 * 
	 */
	public static int ToInt( float num )
	{
		return (int)( num + EPSILON );
	}

	/*
	 * Returns true if all of the given values are close to given target
	 */
	public static bool ValuesCloseToTarget( float[] values, float target, float close )
	{
		for( int i = 0; i < values.Length; i++ ) 
		{
			if( Mathf.Abs( target - values[i] ) > close )
			{
				return false;
			}
		}
		return true;
	}
    
    // Returns a vector2 made from the x and z components of given vector
    public static Vector2 XZ( Vector3 vector )
    {
        return new Vector2( vector.x, vector.z );
    }

}
