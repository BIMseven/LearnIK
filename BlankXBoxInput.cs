using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class BlankXBoxInput : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "BlankXBoxInput";
	public bool VERBOSE = false;

    // These are based off the InputManager.asset file I created
    protected const string A_BUTTON                 = "A";
    protected const string B_BUTTON                 = "B";
    protected const string X_BUTTON                 = "X";
    protected const string Y_BUTTON                 = "Y";
    protected const string RIGHT_BUMPER             = "RB";
    protected const string LEFT_BUMPER              = "LB";
    protected const string BACK_BUTTON              = "V";
    protected const string START_BUTTON             = "M";
    protected const string RIGHT_STICK_BUTTON       = "RSB";
    protected const string LEFT_STICK_BUTTON        = "LSB";
    protected const string LEFT_STICK_HORIZONTAL    = "LSH";
    protected const string LEFT_STICK_VERTICAL      = "LSV";
    protected const string RIGHT_STICK_HORIZONTAL   = "RSH";
    protected const string RIGHT_STICK_VERTICAL     = "RSV";
    protected const string D_PAD_HORIZONTAL         = "DH";
    protected const string D_PAD_VERTICAL           = "DV";
    protected const string LEFT_TRIGGER             = "LT";
    protected const string RIGHT_TRIGGER            = "RT";
    
//--------------------------------------------------------------------------HELPERS:
	
    protected void printButtonsPressed()
    {
        testButton( "A" );
        testButton( "B" );
        testButton( "X" );
        testButton( "Y" );
        testButton( "LB" );
        testButton( "RB" );
        testButton( "LSB" );
        testButton( "RSB" );
        testButton( "V" );
        testButton( "M" );

        testAxis( "DH" );
        testAxis( "DV" );
        testAxis( "LT" );
        testAxis( "RT" );
        testAxis( "DH" );
        testAxis( "DV" );
        testAxis( "RSH" );
        testAxis( "RSV" );
        testAxis( "LSH" );
        testAxis( "LSV" );
    }

    private void testAxis( string axis )
    {
        float value = Input.GetAxis( axis );
        if( value > 0.1f  ||  value < -0.1f )
        {
            LOG_TAG.TPrint( axis + ": " + value );
        }
    }

    private void testButton( string button )
    {
        if( Input.GetButtonDown( button ) )
        {
            LOG_TAG.TPrint( button + " down" );
        }
        if( Input.GetButtonUp( button ) )
        {
            LOG_TAG.TPrint( button + " up" );
        }
    }
}