using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using SDebug = System.Diagnostics.Debug;

public abstract class DataSender
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "DataSender";
    public bool VERBOSE = false;
    
//---------------------------------------------------------------------------FIELDS:

    public MemoryStream Stream { get; private set; }

    protected PacketWriter writer;
    
//---------------------------------------------------------------------CONSTRUCTORS:

    public DataSender()
    {

    }

    public DataSender( MemoryStream stream )
    {
        Init( stream );
    }    

//--------------------------------------------------------------------------METHODS:

    public void Init( MemoryStream stream )
    {
        Stream = stream;
        writer = new PacketWriter();
    }

    public void SendHello()
    {
        writer.BeginMessage( RemoteMessages.Hello );
        writer.Write( "Howdy" );
        writer.Write( (uint)0 );
        writer.EndMessage( Stream );
    }

    public abstract void SendMessages();        
}
