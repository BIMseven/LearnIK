using UnityEngine;
using System.Collections;

// This is a helper class for AudioManager. It is basically a wrapper for AudioClip
// that we used to keep track of which sounds are playing.

public class Sound
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Sound";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:
	
	private AudioClip clip;
	private float volume;
	private float lastPlayed;

//---------------------------------------------------------------------CONSTRUCTORS:

	public Sound( AudioClip clip, float volume )
	{
		this.clip = clip;
		this.volume = volume;
	}

//--------------------------------------------------------------------------METHODS:

	public bool isPlaying()
	{
		return Time.time - lastPlayed < clip.length;
	}

	public void play( Vector3 soundOrigin )
	{
		AudioSource.PlayClipAtPoint( clip, soundOrigin, volume );

		lastPlayed = Time.time;
	}
//--------------------------------------------------------------------------HELPERS:
	
}