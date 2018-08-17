using UnityEngine;
using System.Collections;
using MyUtility;

public class AxisSwaperDrawer : MonoBehaviour
{
    //------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "AxisDrawer";
    public bool VERBOSE = false;

    private const float AXIS_LINE_LENGTH = 10000;

    public AxisSwapper Swapper;

    //---------------------------------------------------------------------------FIELDS:

    //---------------------------------------------------------------------MONO METHODS:

    void OnDrawGizmos()
    {
        if( !enabled ) return;

        if( Swapper == null )
            Swapper = GetComponent<AxisSwapper>();
        // Draw X Axis
        Gizmos.color = Color.red;
        Gizmos.DrawLine( transform.position, Swapper.Right * AXIS_LINE_LENGTH );

        // Draw Y Axis
        Gizmos.color = Color.green;
        Gizmos.DrawLine( transform.position, Swapper.Up * AXIS_LINE_LENGTH );

        // Draw Z Axis
        Gizmos.color = Color.blue;
        Gizmos.DrawLine( transform.position, Swapper.Forward * AXIS_LINE_LENGTH );
    }

    void Update()
    {
    }

    //--------------------------------------------------------------------------METHODS:

    //--------------------------------------------------------------------------HELPERS:

}