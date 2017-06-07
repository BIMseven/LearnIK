using UnityEngine;
using System.Collections;

public class AxisDrawer : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "AxisDrawer";
	public bool VERBOSE = false;

    private const float AXIS_LINE_LENGTH = 10000;

//---------------------------------------------------------------------------FIELDS:
    	
//---------------------------------------------------------------------MONO METHODS:

	void OnDrawGizmos()
    {
        if( ! enabled )   return;

        // Draw X Axis
        Gizmos.color = Color.red;
        Gizmos.DrawLine( transform.position, transform.right * AXIS_LINE_LENGTH );

        // Draw Y Axis
        Gizmos.color = Color.green;
        Gizmos.DrawLine( transform.position, transform.up * AXIS_LINE_LENGTH );

        // Draw Z Axis
        Gizmos.color = Color.blue;
        Gizmos.DrawLine( transform.position, transform.forward * AXIS_LINE_LENGTH );        
    }

    void Update()
    {
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:

}