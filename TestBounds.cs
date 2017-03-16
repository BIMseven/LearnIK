using UnityEngine;
using System.Collections;
using MyUtility;

public class TestBounds : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "TestBounds";
	public bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

    public Bounds Bounds;

    public bool UseColliders;

//---------------------------------------------------------------------MONO METHODS:


    void OnDrawGizmosSelected()
    {
        if( UseColliders )
        {
            Bounds = Utility.GetBoundsFromColliders( gameObject );
        }
        else
        {
            Bounds = Utility.GetBounds( gameObject );
        }
        //Bounds = GetComponent<TactaiAsset>().GetBounds();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube( Bounds.center, Bounds.extents * 2 );
    }

    void Update()
    {

    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}