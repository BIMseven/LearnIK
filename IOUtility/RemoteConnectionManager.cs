using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class RemoteConnectionManager : Singleton<RemoteConnectionManager> 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "RemoteConnectionManager";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
    //TODO temp for test
    public Button ConnectToServerButton;
    public Button StartServerButton;
    public Button SendMessageButton;
    public Button ReceiveMessageButton;
    public Text messageText;

    public string IP = "127.0.0.1";
    public int Port = 38300;

    public RemoteConnection connection;

//---------------------------------------------------------------------MONO METHODS:

	void Start()
    {
        ConnectToServerButton.onClick.AddListener( delegate { connection.ConnectToServer(); } );

        StartServerButton.onClick.AddListener( delegate {
            connection.ListenForConnections();
            messageText.text = "Listening for connections";
        } );

        SendMessageButton.onClick.AddListener( delegate { connection.sayHello(); } );

        ReceiveMessageButton.onClick.AddListener( delegate { connection.receiveHello(); } );

        //TODO will call constructor
        connection.IP = IP;
        connection.Port = Port;
    }

    void Update()
	{
        if( connection.IsConnected )
        {
            messageText.text = "Connected!";
        }
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}