using UnityEngine;
using MyUtility;
using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MyUtility;

// This class will establish a serial connection with an Arduino and read in data
// from the attached IMU and EMU sensors. Whenever the sensors are updated, the
// Arduino will send a message with fresh data. Each message will have a header (a
// single byte, 0xFE), followed by a message of messageSize bytes
//
// For this class to work, Unity's Api Compatability Level must be set to ".NET 2.0"
// instead of ".NET 2.0 subset."  To set, go to Edit > Project Settings > Player
// and select "other settings" tab.

public class Arduino
{
//------------------------------------------------------------------------CONSTANTS:
	
	const string LOG_TAG = "Arduino";
	public bool VERBOSE = false;
	
	// SerialPort communication settings
	const string DEFAULT_PORT_NAME 	  = "/dev/cu.usbmodem1431";
	const int DEFAULT_BAUD 			  = 9600;
	const int DEFAULT_SERIAL_TIMEOUT  = 100;		// Timeout before reporting error
	const Parity DEFAULT_PARITY 	  = Parity.None;
	const int DEFAULT_DATA_BITS 	  = 8;
	const StopBits DEFAULT_STOP_BITS  = StopBits.One;
	const byte DEFAULT_MESSAGE_HEADER = 0xFE;
	const int MESSAGE_BUFFER_SIZE 	  = 1024;

//---------------------------------------------------------------------------FIELDS:

    public bool IsConnected
    {
        get
        {
            return serialPort != null   &&   serialPort.IsOpen;
        }
    }

    public bool MessageInQueue
    {
        get
        {
            return messageQueue != null  &&
                   messageQueue.Count <= ( messageSize + 1 );
        }
    }
    
	private string portName;
	private int baud;
	private Parity parity;
	private int dataBits;
	private StopBits stopBits;
	private byte messageHeader;
	private int messageSize;
	private int serialTimeout;

	// The SerialPort object we will use to establish a connection with Arduino
	private SerialPort serialPort;
	// The structure in which we will store incoming bytes
	private Queue<byte> messageQueue;

//---------------------------------------------------------------------CONSTRUCTORS:

	public Arduino( string portName, int messageSize, int baud )
	{
		init( portName, 
		      baud, 
		      DEFAULT_PARITY, 
		      DEFAULT_DATA_BITS, 
		      DEFAULT_STOP_BITS,
		      DEFAULT_SERIAL_TIMEOUT, 
		      DEFAULT_MESSAGE_HEADER, 
		      messageSize );
	}

	private void init( string portName, 
	                   int baud, 
	                   Parity parity,
	                   int dataBits,
		               StopBits stopBits,  
		               int serialTimeout,
	                   byte messageHeader, 
		               int messageSize )
	{
		this.portName = portName;
		this.baud = baud;
		this.parity = parity;
		this.dataBits = dataBits;
		this.stopBits = stopBits;
		this.messageHeader = messageHeader;
		this.messageSize = messageSize;
		this.serialTimeout = serialTimeout;

		messageQueue = new Queue<byte>();
	}


//--------------------------------------------------------------------------METHODS:

	/// <summary>
    /// Opens a serial connection with Arduino on serialPort
    /// </summary>
	public void Connect()	
	{			
		if( VERBOSE )  LOG_TAG.TPrint( "Attempting to connect" );

		if( serialPort != null )   serialPort.Close();

		openSerialPort();
		
		if( VERBOSE )  LOG_TAG.TPrint( "Connected: " + serialPort.IsOpen );
	}

    /// <summary>
    /// Closes serial connection with Arduino 
    /// </summary>
    public void Disconnect()
	{
		if( serialPort != null )    serialPort.Close();
	}
    
    /// <summary>
    /// Returns the next complete message sitting in our data queue if available. 
    /// To fill the data queue, you must call streamInData
    /// </summary>
    /// <returns></returns>
    public byte[] ReadMessage()
	{
		// Ensure that all data at the front of the queue is the beginning of a message
		seekMessageHeader();

		// Check if the queue has enough elements to hold an entire message (and a message header)
		if( messageQueue.Count <= (messageSize + 1) )
		{
			return null;
		}
		// DQ the message header
		messageQueue.Dequeue();

		byte[] message = new byte[messageSize];

		for( int i = 0; i < message.Length; i++ ) 
		{
			message[i] = messageQueue.Dequeue();
		}
		return message;
	}

    /// <summary>
    /// Stream and data from our serial connection with Arduino
    /// </summary>
    public void StreamInData()
	{
		// Break out of function if serialPort is not open
		if( serialPort == null || ! serialPort.IsOpen )    return;
		byte[] buffer = new byte[128];
		Action kickOffRead = null;
		kickOffRead = delegate 
		{
			try
			{
				serialPort.BaseStream.BeginRead( 
	                buffer, 
	                0,
	                buffer.Length, 
	                delegate( IAsyncResult result ) 
	                {
						try 
						{
							int actualLen = serialPort.BaseStream.EndRead( result );
							byte[] received = new byte[actualLen];
							Buffer.BlockCopy( buffer, 0, received, 0, actualLen );					
							updateMessageBuffer( received );
						}							
						catch( IOException exception ) 
						{
							Utility.Print ( LOG_TAG, "" + exception );
						}		
					}, 
					null );
			}
			catch( TimeoutException exception )
			{
				if( VERBOSE )   Utility.Print ( LOG_TAG, "Still waiting for data" );
			}			
		};
		kickOffRead();
	}

//--------------------------------------------------------------------------HELPERS:

	// Establishes connection with Arduino, returning true if arduino is connected
	private bool openSerialPort()
	{
		if( serialPort == null ) 
		{
			serialPort = new SerialPort( portName, 
			                             baud,
			                             parity,
			                             dataBits,
			                             stopBits );
		}

		if( serialPort.IsOpen )   return true;
		
		try 
		{
			serialPort.Open();
			serialPort.ReadTimeout = serialTimeout;
		} 
		catch( System.Exception ) 
		{
			return false;
		}
		return serialPort.IsOpen;
	}

	// Clears all data at the front of the queue until a message header is at front
	private void seekMessageHeader()
	{
		while( messageQueue.Count > 0  &&
		      messageQueue.Peek() != messageHeader )
		{
			messageQueue.Dequeue();
		}
	}

	// Enqueues all elements in messageChunk
	private void updateMessageBuffer( byte[] messageChunk )
	{
		for( int i = 0; i < messageChunk.Length; i++ ) 
		{
			messageQueue.Enqueue( messageChunk[i] );
		}		
	}

//--------------------------------------------------------------GETTERS AND SETTERS:

	// Returns true if we have an open serial port talking to Arduino

}
