﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUtility
{
    public class AudioManager : Singleton<AudioManager>
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "AudioManager";
        public bool VERBOSE = false;

        private const float AMBIENT_SOUNDS_VOLUME = 0.5f;
        private const float DEFAULT_SOUNDS_VOLUME = 1.0f;

//---------------------------------------------------------------------------FIELDS:

        public AudioClip[] Clips;
        public string[] ClipNames;
        public float[] ClipVolumes;

        // Background music
        public AudioSource[] BackgroundTracks;
        public string[] BackgroundTrackNames;

        private Dictionary<string, Sound> mySounds;
        private Dictionary<string, AudioSource> myBackgroundTracks;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            mySounds = new Dictionary<string, Sound>();

            if( Clips != null )
            {
                if( ClipNames == null  ||  ClipNames.Length == 0 )
                {
                    ClipNames = new string[Clips.Length];
                    for( int i = 0; i < Clips.Length; i++ )
                    {
                        ClipNames[i] = Clips[i].name;
                    }
                }
                if( ClipVolumes == null  || ClipVolumes.Length == 0 )
                {
                    ClipVolumes = new float[Clips.Length];
                    for( int i = 0; i < Clips.Length; i++ )
                    {
                        ClipVolumes[i] = DEFAULT_SOUNDS_VOLUME;
                    }
                }
                for( int i = 0; i < Clips.Length; i++ )
                {
                    mySounds.Add( ClipNames[i],
                                  new Sound( Clips[i], ClipVolumes[i] ) );
                }
            }

            myBackgroundTracks = new Dictionary<string, AudioSource>();
            if( BackgroundTracks != null )
            {
                for( int i = 0; i < BackgroundTracks.Length; i++ )
                {
                    myBackgroundTracks.Add( BackgroundTrackNames[i], 
                                            BackgroundTracks[i] );
                }

            }
        }

//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Plays the sound with the given name at the main camera's location
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound( string soundName, 
                               bool playAgainIfAlreadyPlaying = false )
        {
            if( Camera.current != null )
            {
                PlaySound( soundName, 
                           Camera.current.transform.position,
                           playAgainIfAlreadyPlaying );
            }
            else
            {
                PlaySound( soundName, 
                           Vector3.zero, 
                           playAgainIfAlreadyPlaying );
            }
        }

        // Plays the sound with the given name if it isn't already playing
        public void PlaySound( string soundName, 
                               Vector3 location,
                               bool playAgainIfAlreadyPlaying = false )
        {
            Sound sound;
            if( mySounds != null  &&
                mySounds.TryGetValue( soundName, out sound ) &&
                sound != null )
            {
                if( ! sound.IsPlaying  ||  playAgainIfAlreadyPlaying )
                {
                    sound.Play( location );
                }
            }
            else if( VERBOSE )
            { 
                Utility.Print( LOG_TAG, "Unable to find sound: " + soundName );
            }
        }

        public void PlayTrack( string trackName, bool loopTrack = true )
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

        public void StopSound( string soundName )
        {
            Sound sound;
            if( mySounds != null  &&
                mySounds.TryGetValue( soundName, out sound ) )
            {
                if( sound != null && sound.IsPlaying )
                {
                    sound.Stop();
                }
            }
            else if( VERBOSE )
            {
                Utility.Print( LOG_TAG, "Unable to find sound: " + soundName );
            }
        }

        public void StopTrack( string trackName )
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