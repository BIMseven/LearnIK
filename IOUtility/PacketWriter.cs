using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyUtility;
using SDebug = System.Diagnostics.Debug;

/// <summary>
/// See RemoteConnection for details
/// </summary>
public class PacketWriter
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "PacketWriter";
	public bool VERBOSE = false;
    
//---------------------------------------------------------------------------FIELDS:
	
    private BinaryWriter writer;
    private MemoryStream packet;
    private RemoteMessages message;
    private byte[] buffer = new byte[128 * 1024];
    
//---------------------------------------------------------------------CONSTRUCTORS:

    public PacketWriter()
    {
        packet = new MemoryStream();
        writer = new BinaryWriter( packet );
        message = RemoteMessages.Invalid;
    }

//--------------------------------------------------------------------------METHODS:

    public void BeginMessage( RemoteMessages message )
    {
        SDebug.Assert( message == RemoteMessages.Invalid );

        this.message = message;
        packet.Position = 0;
        packet.SetLength( 0 );
    }

    public void EndMessage( Stream stream )
    {
        SDebug.Assert( message != RemoteMessages.Invalid );

        // Write message header (one byte for type of message, four bytes for size)
        stream.WriteByte( (byte)message );
        uint len = (uint)packet.Length;
        stream.WriteByte( (byte)( len & 0xFF ) );
        stream.WriteByte( (byte)( ( len >> 8 ) & 0xFF ) );
        stream.WriteByte( (byte)( ( len >> 16 ) & 0xFF ) );
        stream.WriteByte( (byte)( ( len >> 24 ) & 0xFF ) );

        // Write the message
        packet.Position = 0;
        IOUtility.CopyToStream( packet, stream, buffer, (int)packet.Length );

        message = RemoteMessages.Invalid;
    }

    public void Write( bool value ) { writer.Write( value ); }
    public void Write( int value ) { writer.Write( value ); }
    public void Write( uint value ) { writer.Write( value ); }
    public void Write( long value ) { writer.Write( value ); }
    public void Write( ulong value ) { writer.Write( value ); }
    public void Write( float value ) { writer.Write( value ); }
    public void Write( double value ) { writer.Write( value ); }
    public void Write( byte[] value ) { writer.Write( value ); }
    public void Write( string value )
    {
        writer.Write( (uint)value.Length );
        writer.Write( Encoding.UTF8.GetBytes( value ) );
    }

}

