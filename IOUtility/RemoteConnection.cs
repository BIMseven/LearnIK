using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using MyUtility;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.Text;

// State object for receiving data from remote device.  
public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

/// <summary>
/// 
/// </summary>
public class RemoteConnection : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "RemoteConnection";
	public bool VERBOSE = false;

    // The maximum length of the pending connections queue for socket
    private const int BACKLOG = 32;

    private const int STREAM_BUFFER_SIZE = 10240;

//---------------------------------------------------------------------------FIELDS:
	
    public Socket Socket { get; private set; }
    
    public bool IsConnected
    {
        get
        {
            return Socket != null  &&  Socket.Connected;
        }
    }

    public bool IsListening, IsSending;

    private MemoryStream readStream;
    private MemoryStream writeStream;
    private byte[] readBuffer;
    //private byte[] copyBuffer;

    private TcpClient client;

    private string ip;
    private int port;

    private DataSender dataSender;
    private DataReceiver dataReceiver;

    protected bool isServer;


//---------------------------------------------------------------------MONO METHODS:

	void Update()
    {
        // Listen for connections (If ListenForConnections() was called)
        if( isServer  &&  ! IsConnected )
        {
            // TODO BeginAcceptIncoming
            if( ( client = acceptIncoming( Socket ) ) != null )
            {
                Socket = client.Client;
                receive( Socket );
            }
        }

        if( ! IsConnected )   return;

        try
        {
            if( IsSending)     sendMessages();
        }
        catch( Exception exception )
        {
            Debug.LogError( "EX: " + exception );
        }
    }

//--------------------------------------------------------------------------METHODS:

    /// <summary>
    /// Connect to a server
    /// </summary>
    public void ConnectToServer( string ip, int port )
    {
        this.ip = ip;
        this.port = port;
        Socket = createConnectingSocket( ip, port );
    }

    public void Init( DataSender sender, DataReceiver receiver )
    {
        readBuffer = new byte[STREAM_BUFFER_SIZE];
        readStream = new MemoryStream( readBuffer );
        readStream.Position = 0;
        readStream.SetLength( 0 );

        writeStream = new MemoryStream( STREAM_BUFFER_SIZE );
        
        dataSender = sender;
        dataReceiver = receiver;

        dataSender.Init( writeStream );
    }

    /// <summary>
    /// Listen for connections from client (this will be a server)
    /// </summary>
    public void ListenForConnections( int port )
    {
        this.port = port;
        Socket = createListeningSocket( port );
    }

//--------------------------------------------------------------------------HELPERS:

    // Accepts any incoming connection attempts on given lisetening socket and 
    // returns the client
    private TcpClient acceptIncoming( Socket listeningSocket )
    {
        try
        {
            Socket tcpSocket = listeningSocket.Accept();
            tcpSocket.Blocking = true;
            return new TcpClient { Client = tcpSocket };
        }
        catch( SocketException ex )
        {
            // Standard return for non-blocking socket calls
            if( ex.ErrorCode == (int)SocketError.WouldBlock )
            {
                // Nobody wants to connect
                return null;
            }                
            Debug.LogError( "SocketException in AcceptIncoming: " + ex );
            throw ( ex );
        }
    }

    // Creates streaming TCP socket trying to connect to given ipAddress
    private Socket createConnectingSocket( string ipAddress, int port )
    {
        isServer = false;

        IPEndPoint endPoint = new IPEndPoint( IPAddress.Parse( ipAddress ), port );

        AsyncCallback callback = delegate { onConnected(); };

        Socket socket = new Socket( endPoint.Address.AddressFamily,
                                    SocketType.Stream,
                                    ProtocolType.Tcp );

        //socket.Blocking = false;

        // Must connect asynchronously with non-blocking socket
        socket.BeginConnect( endPoint, callback, Socket );

        return socket;
    }

    // Creaates streaming TCP socket listening for connections
    private Socket createListeningSocket( int port )
    {
        if( VERBOSE )   Utility.Print( LOG_TAG, "Listening on port " + port );
        
        isServer = true;

        Socket socket = new Socket( AddressFamily.InterNetwork,
                                    SocketType.Stream,
                                    ProtocolType.Tcp );

        // Will specify port number
        IPEndPoint endPoint = new IPEndPoint( IPAddress.Any, port );

        // Allows the socket to be bound to an address that's already in use
        socket.SetSocketOption( SocketOptionLevel.Socket,
                                SocketOptionName.ReuseAddress,
                                true );

        socket.Blocking = false;
        socket.Bind( endPoint );
        socket.Listen( BACKLOG );

        return socket;
    }
    
    private void onConnected()
    {
        if( VERBOSE )
        {
            Utility.Print( LOG_TAG, "Socket Connected" );
        }
    }

    // Receive stuff being send through socket
    private void receive( Socket socket )
    {
        try
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = socket;

            // Begin receiving the data from the remote device.  
            socket.BeginReceive( state.buffer, 
                                 0, 
                                 StateObject.BufferSize, 
                                 0,
                                 new AsyncCallback( receiveCallback ), 
                                 state );
        }
        catch( Exception e )
        {
            Console.WriteLine( e.ToString() );
        }
    }

    private void receiveCallback( IAsyncResult ar )
    {
        try
        {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket socket = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = socket.EndReceive( ar );

            if( bytesRead > 0 )
            {
                dataReceiver.AppendData( new MemoryStream( state.buffer ), bytesRead );

                // We may have received a full message
                dataReceiver.ProcessMessages();

                // Get the rest of the data.  
                socket.BeginReceive( state.buffer, 
                                     0, 
                                     StateObject.BufferSize, 
                                     0,
                                     new AsyncCallback( receiveCallback ), 
                                     state );
            }
            else
            {
                print( "received nothing! " );
            }
        }
        catch( Exception e )
        {
            Console.WriteLine( e.ToString() );
        }
    }
    
    private void send( Socket socket, String data )
    {
        // Convert the string data to byte data using ASCII encoding.  
        send( socket, Encoding.ASCII.GetBytes( data ) );
    }

    private void send( Socket socket, byte[] byteData )
    {
        if( socket.Connected )
        {
            // Begin sending the data to the remote device.  
            socket.BeginSend( byteData,
                              0,
                              byteData.Length,
                              0,
                              new AsyncCallback( sendCallback ),
                              socket );
        }
    }

    private void sendCallback( IAsyncResult ar )
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend( ar );
        }
        catch( Exception e )
        {
            Debug.LogError( e.ToString() );
        }
    }

    private void sendMessages()
    {
        // Write messages to stream
        dataSender.SendMessages();

        // Send contents of stream to socket
        MemoryStream dataStream = dataSender.Stream;
        if( dataStream.CanRead )
        {
            send( Socket, dataSender.Stream.ToArray() );
        }
        dataSender.Stream.Flush();
        dataSender.Stream.Position = 0;
    }    
}