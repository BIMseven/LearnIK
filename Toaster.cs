using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Toaster";
	public bool VERBOSE = false;

    private const float METERS_TO_SCALE         = 0.02f;
    private const float DEFAULT_DISPLAY_TIME    = 3.0f;

//---------------------------------------------------------------------------FIELDS:
			
    // Length of text "Hello World" in meters
    public float ToastSize = 1.0f;

    [HideInInspector]
    public GameObject TextPrefab;

    private GameObject toastDisplayed;

    private bool checkToDestroyToast = false;
    private float timeDisplayed, toastDuration;

//---------------------------------------------------------------------MONO METHODS:
		
	void Update()
	{
        destroyExpiredToasts();
	}

//--------------------------------------------------------------------------METHODS:

    /// <summary>
    /// Creates a 3D text with given message at this Toaster's position with this
    /// Toaster's rotation.  This text will persist until DestoryDisplayedText is
    /// called
    /// </summary>
    /// <param name="message"></param>
    public void CreateText( string message )
    {
        CreateText( message, transform.position, transform.rotation );
    }

    /// <summary>
    /// Creates a 3D text with given message at this Toaster's position.  This 
    /// text will persist until DestoryDisplayedText is called
    /// </summary>
    /// <param name="message"></param>
    public void CreateText( string message, Vector3 location )
    {
        CreateText( message, location, transform.rotation );
    }

    /// <summary>
    /// Creates a 3D text with given message at given location.  This text will 
    /// persist until DestoryDisplayedText is called
    /// </summary>
    /// <param name="message"></param>
    /// <param name="location"></param>
    public void CreateText( string message, Vector3 location, Quaternion rotation )
    {
        DestroyDisplayedText();
        toastDisplayed = GameObject.Instantiate( TextPrefab );

        toastDisplayed.transform.localScale = Vector3.one * ToastSize * METERS_TO_SCALE;
        toastDisplayed.transform.position = location;
        toastDisplayed.transform.rotation = rotation;
        toastDisplayed.GetComponent<TextMesh>().text = message;
    }

    /// <summary>
    /// Destroys the Text or Toast (temp text) being displayed
    /// </summary>
    public void DestroyDisplayedText()
    {
        if( toastDisplayed != null )
        {
            GameObject.Destroy( toastDisplayed );
        }
        checkToDestroyToast = false;
    }

    /// <summary>
    /// Creates a Text at given location and destroys it after given duration 
    /// seconds
    /// </summary>
    /// <param name="message"></param>
    /// <param name="duration"></param>
    /// <param name="location"></param>
    public void MakeToast( string message, 
                           float duration, 
                           Vector3 location,
                           Quaternion rotation )
    {
        CreateText( message, location, rotation );

        checkToDestroyToast = true;
        timeDisplayed = Time.timeSinceLevelLoad;
        toastDuration = duration;
    }

    /// <summary>
    /// Creates a Text at this Toaster's position and destroys it after given 
    /// duration 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="duration"></param>
    public void MakeToast( string message, float duration = DEFAULT_DISPLAY_TIME )
    {
        MakeToast( message, duration, transform.position, transform.rotation );
    }
    

//--------------------------------------------------------------------------HELPERS:

    private void destroyExpiredToasts()
    {
        if( checkToDestroyToast )
        {
            if( Time.timeSinceLevelLoad - timeDisplayed >= toastDuration )
            {
                DestroyDisplayedText();
            }
        }
    } 
}