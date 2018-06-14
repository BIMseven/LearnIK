using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    /// <summary>
    /// Very simple Staircase method to find the threshold of human perception.  
    /// Assumes that the greater the value tested, the more likely a subject will 
    /// respond true.
    /// </summary>
    public class Staircase 
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "Staircase";
	    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	        
        public float StepSize { get; private set; }

        public List<Tuple<float, bool>> Responses
        {
            get
            {
                var tuples = new List<Tuple<float, bool>>();
                for( int i = 0; i < values.Count; i++ )
                {
                    var response = new Tuple<float, bool>( values[i], responses[i] );
                    tuples.Add( response );
                }
                return tuples;
            }
        }

        public float ValueToTest { get; private set; }

        private List<float> values;
        private List<bool> responses;

//---------------------------------------------------------------------CONSTRUCTORS:

        public Staircase( float initialValue, float stepSize )
        {
            values = new List<float>();
            responses = new List<bool>();
            StepSize = stepSize;
            ValueToTest = initialValue;
        }

//--------------------------------------------------------------------------METHODS:

        public void RegisterResponse( bool response )
        {
            values.Add( ValueToTest );
            responses.Add( response );

            vLog( "Reponse at " + ValueToTest + ": " + response );

            if( response )
            {
                ValueToTest -= StepSize;
            }
            else
            {
                ValueToTest += StepSize;
            }
        }

//--------------------------------------------------------------------------HELPERS:
	
        private void vLog( string message )
        {
            if( VERBOSE )   LOG_TAG.TPrint( message );        
        }
    }
}