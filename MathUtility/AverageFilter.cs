using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFilter
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "AverageFilter";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    public float Average
    {
        get
        {
            if( QueueSize == 0 )   return 0;

            float[] values = rawValues.ToArray();

            float sum = 0;
            float lastValueInQueue = 0;

            foreach( float value in values )
            {
                sum += value;
                lastValueInQueue = value;
            }

            int numValuesToBuffer = QueueSize - values.Length;
            for( int i = 0; i < numValuesToBuffer; i++ )
            {
                sum += lastValueInQueue;
            }
            return sum / QueueSize;
        }
    }

    private Queue<float> rawValues;
    public int QueueSize;
		
//---------------------------------------------------------------------CONSTRUCTORS:
	
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="numSamples">Number of previous raw values to average</param>
	public AverageFilter( int numSamples )
    {
        rawValues = new Queue<float>();
        QueueSize = numSamples;
    }

//--------------------------------------------------------------------------METHODS:

    public void Update( float rawValue )
    {
        rawValues.Enqueue( rawValue );
        if( rawValues.Count > QueueSize )
        {
            rawValues.Dequeue();
        }
    }

//--------------------------------------------------------------------------HELPERS:
    
}