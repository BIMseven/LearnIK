using UnityEngine;
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

        void Awake()
        {
            mySounds = new Dictionary<string, Sound>();

            if( Clips != null )
            {
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

        // Plays the sound with the given name if it isn't already playing
        public void PlaySound( string soundName, Vector3 location )
        {
            Sound sound;
            if( mySounds.TryGetValue( soundName, out sound ) )
            {
                if( !sound.isPlaying() )
                {
                    sound.play( location );
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