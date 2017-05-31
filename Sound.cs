using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyUtility;

// This is a helper class for AudioManager. It is basically a wrapper for AudioClip
// that we used to keep track of which sounds are playing.

public class Sound
{
    
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Sound";

//---------------------------------------------------------------------------FIELDS:
	
    public bool IsPlaying
    {
        get
        {
            if( lastPlayed == 0 )   return false;
            return Time.time - lastPlayed < clip.length;
        }
    }

	private AudioClip clip;
    private AudioSource tempAudioSource;
	private float volume;
	private float lastPlayed;

//---------------------------------------------------------------------CONSTRUCTORS:

	public Sound( AudioClip clip, float volume )
	{
		this.clip = clip;
		this.volume = volume;
	}

//--------------------------------------------------------------------------METHODS:

	public void Play( Vector3 soundOrigin )
	{
        if( tempAudioSource != null )
        {

            GameObject.Destroy( tempAudioSource );

        }
        GameObject tempObject = new GameObject();
        tempObject.name = "Sound";
        tempAudioSource = tempObject.AddComponent<AudioSource>();
        tempAudioSource.volume = volume;
        tempAudioSource.loop = false;
        tempAudioSource.clip = clip;
        tempObject.transform.position = soundOrigin;
        tempAudioSource.Play();
        var destroyer = tempObject.AddComponent<GameObjectDestroyer>();
        destroyer.Destroy( clip.length );
        

        //AudioSource.PlayClipAtPoint( clip, soundOrigin, volume );

        lastPlayed = Time.time;
        
	}

    public void Stop()
    {
        if( tempAudioSource != null )
        {
            tempAudioSource.GetComponent<GameObjectDestroyer>().CancelInvoke();
            GameObject.Destroy( tempAudioSource );
        }
    }

//--------------------------------------------------------------------------HELPERS:
	
    void DestroyObject()
    {
        if( tempAudioSource != null )
        {
            GameObject.Destroy( tempAudioSource );
        }
    }
}