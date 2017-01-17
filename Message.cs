using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Message";
	private const bool VERBOSE = true;
	
//---------------------------------------------------------------------------FIELDS:

	public string message;
	public Text textSample;	
	public float displayTime;
		
//---------------------------------------------------------------------CONSTRUCTORS:

	public Message( string message, Text textSample, float displayTime )
	{
		this.message = message;
		this.textSample = textSample;
		this.displayTime = displayTime;
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}