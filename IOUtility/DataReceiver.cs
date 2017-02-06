using System;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using MyUtility;

public enum EditorMessage
{
	Invalid = 0,

	Hello = 1,
	GyroSettings = 2,
	ScreenOrientation = 3,

	ScreenStream = 10,

	WebCamStartStream = 20,
	WebCamStopStream = 21,

	Reserved = 255,
};


public class DataReceiver
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
        Profiler.BeginSample( "AppendData" );

        data.Position = data.Length;
        Utils.CopyToStream( stream, data, buffer, available );

        Profiler.EndSample();
    }

    //TODO will be abstract
    public void ProcessMessage( Stream stream )
    {
        Profiler.BeginSample( "ProcessMessage" );

        BinaryReader reader = new BinaryReader( data );
        byte msg = reader.ReadByte();
        uint size = reader.ReadUInt32();
        if( Enum.IsDefined( typeof( EditorMessage ), (EditorMessage)msg ) )
        {
            switch( (EditorMessage)msg )
            {
                case EditorMessage.Hello: HandleHello( reader ); break;
                //case EditorMessage.ScreenStream: HandleScreenStream( reader ); break;
                //case EditorMessage.GyroSettings: HandleGyroSettings( reader ); break;
                //case EditorMessage.ScreenOrientation: HandleScreenOrientation( reader ); break;
                //case EditorMessage.WebCamStartStream: HandleWebCamStartStream( reader ); break;
                //case EditorMessage.WebCamStopStream: HandleWebCamStopStream( reader ); break;
            }
        }
        else
        {
            //Console.WriteLine("Unknown message: " + msg);
            reader.ReadBytes( (int)size );
        }

        Profiler.EndSample();
    }

    public void ReceiveMessages()
    {
        data.Position = 0;

        while( hasFullMessage( data ) )
        {
            Utility.Print( LOG_TAG, "FULL MESSAGE!" );
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
        string magic = reader.ReadCustomString();
        if( magic != "UnityRemote" )
            throw new ApplicationException( "Handshake failed" );
        else
            Utility.Print( LOG_TAG, "SUCCESSFUL HANDSHAKE!" );

        uint version = reader.ReadUInt32();
        if( version != 0 )
        {
            throw new ApplicationException( "Unsupported protocol version: " + version );
        }
    }
        
//--------------------------------------------------------------------------HELPERS:

    private static bool hasFullMessage( Stream stream )
    {
        BinaryReader reader = new BinaryReader( stream );
        long oldPosition = stream.Position;
        bool result = true;

        // TODO why 5?
        if( stream.Length - stream.Position < 5 )     result = false;

        if( result )
        {
            reader.ReadByte();
            uint size = reader.ReadUInt32();
            if( stream.Length - stream.Position < size )
                result = false;
        }

        stream.Position = oldPosition;
        return result;
    }


}
