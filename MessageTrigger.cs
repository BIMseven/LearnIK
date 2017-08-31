using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyUtility;

[RequireComponent( typeof( Text ) )]
public class MessageTrigger : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "MessageTrigger";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
	public string messageText;
	public float displayDuration;
	public bool destroyOnDelivery;

	private Text textSample;
	private MessageDisplayer messageDisplayer;

//---------------------------------------------------------------------MONO METHODS:

	void Start()
	{
		textSample = GetComponent<Text>();
		messageDisplayer = MessageDisplayer.Instance;
	}

	void OnTriggerEnter( Collider other )
	{
		if( VERBOSE )    Utility.Print ( LOG_TAG, "OnTriggerEnter" );

		Message message = new Message( messageText,
		                               textSample,
		                               displayDuration );

		messageDisplayer.queueMessage( message );

		if( destroyOnDelivery )
		{
			Object.Destroy( gameObject );
		}
	}

//--------------------------------------------------------------------------METHODS:


//--------------------------------------------------------------------------HELPERS:
	
}