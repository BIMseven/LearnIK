using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUtility
{ 
public class AudioManager : Singleton<AudioManager> 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "AudioManager";
	private const bool VERBOSE = true;

	private const float AMBIENT_SOUNDS_VOLUME = 0.5f;

//---------------------------------------------------------------------------FIELDS:

	public AudioClip[] Clips;
	public string[] ClipNames;
	public float[] ClipVolumes;

	// Background music
	public AudioSource[] BackgroundTracks;
	public string[] BackgroundTrackNames;

    #region PRIVATE_FIELDS
    private Dictionary<string, Sound> mySounds;	
	private Dictionary<string, AudioSource> myBackgroundTracks;
    #endregion

//---------------------------------------------------------------------MONO METHODS:

    void Awake()
	{
		mySounds = new Dictionary<string, Sound>();

		for( int i = 0; i < Clips.Length; i++ ) 
		{			
			mySounds.Add( ClipNames[i], 
			              new Sound( Clips[i], ClipVolumes[i] ) );			
		}

		myBackgroundTracks = new Dictionary<string, AudioSource>();

		for( int i = 0; i < BackgroundTracks.Length; i++ )
		{
			myBackgroundTracks.Add( BackgroundTrackNames[i], BackgroundTracks[i] );
		}
	}

//--------------------------------------------------------------------------METHODS:

	// Plays the sound with the given name if it isn't already playing
	public void playSound( string soundName, Vector3 location )
	{
		Sound sound;
		if( mySounds.TryGetValue( soundName, out sound ) )
		{
			if( ! sound.isPlaying() )
			{
				sound.play( location );
			}
		}		
		else
		{
			Utility.Print( LOG_TAG, "Unable to find sound" );
		}
	}

	public void playTrack( string trackName, bool loopTrack = true )
	{
		AudioSource track = myBackgroundTracks[trackName];

		if( track != null )
		{
			track.enabled = true;			
			stopPlayingAllTracks();
			// TODO: set volume?
			track.loop = loopTrack;
			track.Play();
		}
		else
		{
			Utility.Print( LOG_TAG, "Unable to find track" );
		}
	}

	public void stopPlayingTrack( string trackName )
	{
		AudioSource track = myBackgroundTracks[trackName];
		
		if( track != null )
		{
			track.Stop();
		}
	}

//--------------------------------------------------------------------------HELPERS:

	private void stopPlayingAllTracks()
	{
		foreach( KeyValuePair<string, AudioSource> pair in myBackgroundTracks )
		{
			pair.Value.Stop();
		}
	}
}
}