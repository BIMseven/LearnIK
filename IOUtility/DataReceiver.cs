using System;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using MyUtility;
using System.Text;

public abstract class DataReceiver
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "DataReceiver";
    public bool VERBOSE = false;
    
//---------------------------------------------------------------------------FIELDS:

	protected MemoryStream data = new MemoryStream();
	protected byte[] buffer = new byte[4096];
    
//---------------------------------------------------------------------CONSTRUCTORS:

//--------------------------------------------------------------------------METHODS:

    public void AppendData( Stream stream, int available )
    {
        data.Position = data.Length;
        IOUtility.CopyToStream( stream, data, buffer, available );
    }
    
    public abstract void ProcessMessage( Stream stream );

    public void ProcessMessages()       
    {
        data.Position = 0;
        // Process all full messages
        while( hasFullMessage( data ) )
        {
            ProcessMessage( data );
        }
        // Copy leftover bytes
        long left = data.Length - data.Position;
        byte[] buffer = data.GetBuffer();
        Array.Copy( buffer, data.Position, buffer, 0, left );
        data.Position = 0;
        data.SetLength( left );
    }

    /// <summary>
    /// Handles handshake
    /// </summary>
    /// <param name="reader"></param>
    public void HandleHello( BinaryReader reader )
    {
        string magic = readCustomString( reader );
         
        if( magic != "Howdy" )
        {
            throw new ApplicationException( "Handshake failed" );
        }
        else if( VERBOSE )
        {
            Utility.Print( LOG_TAG, "Received handshake" );
        }
        uint version = reader.ReadUInt32();
        if( version != 0 )
        {
            throw new ApplicationException( "Unsupported protocol version: " + version );
        }
    }
        
//--------------------------------------------------------------------------HELPERS:

    private static bool hasFullMessage( Stream stream )
    {
        bool hasFullMessage;
        BinaryReader reader = new BinaryReader( stream );
        long oldPosition = stream.Position;

        // One byte for header, four bytes for message size
        bool headerAndSizePresent = stream.Length - stream.Position >= 5;
        
        // If header and message size present, compare message size to stream length
        if( headerAndSizePresent )
        {
            reader.ReadByte();
            uint size = reader.ReadUInt32();
            hasFullMessage = stream.Length - stream.Position >= size;                
        }
        else
        {
            hasFullMessage = false;
        }
        // Return stream to previous position
        stream.Position = oldPosition;
        return hasFullMessage;
    }

    protected static string readCustomString( BinaryReader reader )
    {
        int length = (int)reader.ReadUInt32();
        byte[] bytes = reader.ReadBytes( length );
        return Encoding.UTF8.GetString( bytes );
    }
}
