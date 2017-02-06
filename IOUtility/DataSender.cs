using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using SDebug = System.Diagnostics.Debug;



public class DataSender
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "DataSender";
    public bool VERBOSE = false;
    
//---------------------------------------------------------------------------FIELDS:

    private PacketWriter writer;
    private Stream stream;
    
//---------------------------------------------------------------------CONSTRUCTORS:

    public DataSender( Stream stream )
    {
        this.stream = stream;
        writer = new PacketWriter();
    }    

//--------------------------------------------------------------------------METHODS:

    public void SendHello()
    {
        writer.BeginMessage( RemoteMessages.Hello );
        writer.Write( "UnityRemote" );
        writer.Write( (uint)0 );
        writer.EndMessage( stream );
    }


    public void SendOptions()
    {
        // Add Screen size information
        // TODO: only send when changed
        writer.BeginMessage( RemoteMessages.Options );
        writer.Write( Screen.width );
        writer.Write( Screen.height );
        writer.EndMessage( stream );
    }


    public void SendDeviceOrientation()
    {
        writer.BeginMessage( RemoteMessages.DeviceOrientation );
        writer.Write( (int)Input.deviceOrientation );
        writer.EndMessage( stream );
    }


    public void SendAccelerometerInput()
    {
        writer.BeginMessage( RemoteMessages.AccelerometerInput );
        writer.Write( Input.acceleration.x );
        writer.Write( Input.acceleration.y );
        writer.Write( Input.acceleration.z );
        writer.Write( Time.deltaTime );
        writer.EndMessage( stream );
    }


    public void SendGyroscopeSettings()
    {
        Gyroscope gyro = Input.gyro;
        writer.BeginMessage( RemoteMessages.GyroSettings );
        writer.Write( gyro.enabled ? 1 : 0 );
        writer.Write( gyro.updateInterval );
        writer.EndMessage( stream );
    }


    public void SendGyroscopeInput()
    {
        // TODO: check updateInterval here..
        Gyroscope gyro = Input.gyro;
        writer.BeginMessage( RemoteMessages.GyroInput );
        writer.Write( gyro.rotationRate.x );
        writer.Write( gyro.rotationRate.y );
        writer.Write( gyro.rotationRate.z );
        writer.Write( gyro.rotationRateUnbiased.x );
        writer.Write( gyro.rotationRateUnbiased.y );
        writer.Write( gyro.rotationRateUnbiased.z );
        writer.Write( gyro.gravity.x );
        writer.Write( gyro.gravity.y );
        writer.Write( gyro.gravity.z );
        writer.Write( gyro.userAcceleration.x );
        writer.Write( gyro.userAcceleration.y );
        writer.Write( gyro.userAcceleration.z );
        writer.Write( gyro.attitude.x );
        writer.Write( gyro.attitude.y );
        writer.Write( gyro.attitude.z );
        writer.Write( gyro.attitude.w );
        writer.EndMessage( stream );
    }


    public void SendTouchInput()
    {
        for( int i = 0; i < Input.touchCount; ++i )
        {
            Touch touch = Input.GetTouch( i );
            writer.BeginMessage( RemoteMessages.TouchInput );
            writer.Write( touch.position.x );
            writer.Write( touch.position.y );
            writer.Write( (long)Time.frameCount );
            writer.Write( touch.fingerId );
            writer.Write( (int)touch.phase );
            writer.Write( (int)touch.tapCount );
            writer.EndMessage( stream );
        }
    }

    
}
