using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUtility
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Serializable]
        public class AMClip
        {
            public AudioClip Clip;
            public string Name;
            public float Volume;
        }

        [Serializable]
        public class BackgroundTrack
        {
            public AudioSource Track;
            public string Name;
        }

//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "AudioManager";
        public bool VERBOSE = false;

        private const float AMBIENT_SOUNDS_VOLUME = 0.5f;
        private const float DEFAULT_SOUNDS_VOLUME = 1.0f;

//---------------------------------------------------------------------------FIELDS:

        #region InspectorFields
        public AMClip[] Clips;
        #endregion

        // Background music
        public BackgroundTrack[] BackgroundTracks;

        private Dictionary<string, Sound> mySounds;
        private Dictionary<string, AudioSource> backgroundTracks;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            populateMySounds();
            populateBackgroundTracks();
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
                    sound.Play( location, soundName );
                }
            }
            else if( VERBOSE )
            { 
                Utility.Print( LOG_TAG, "Unable to find sound: " + soundName );
            }
        }

        public void PlayTrack( string trackName, bool loopTrack = true )
        {
            AudioSource track = backgroundTracks[trackName];

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

        public void RefreshSoundList()
        {
            populateMySounds();
        }
        /// <summary>
        /// Returns the duration of sound if it exists, -1 otherwise
        /// </summary>
        /// <param name="soundName"></param>
        /// <returns></returns>
        public float SoundDuration( string soundName )
        {
            Sound sound;
            if( mySounds != null &&
                mySounds.TryGetValue( soundName, out sound ) &&
                sound != null )
            {
                return sound.Duration;
            }
            return -1f;
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
            AudioSource track = backgroundTracks[trackName];

            if( track != null )
            {
                track.Stop();
            }
        }
        
//--------------------------------------------------------------------------HELPERS:

        private void populateBackgroundTracks()
        {
            backgroundTracks = new Dictionary<string, AudioSource>();
            if( BackgroundTracks != null )
            {
                for( int i = 0; i < BackgroundTracks.Length; i++ )
                {
                    backgroundTracks.Add( BackgroundTracks[i].Name,
                                            BackgroundTracks[i].Track );
                }

            }
        }

        private void populateMySounds()
        {
            mySounds = new Dictionary<string, Sound>();

            if( Clips != null )
            {
                foreach( AMClip clip in Clips )
                {
                    if( clip.Name == "" )
                    {
                        clip.Name = clip.Clip.name;
                    }
                    mySounds.Add( clip.Name, new Sound( clip.Clip, clip.Volume ) );
                }
            }
        }

        private void stopPlayingAllTracks()
        {
            foreach( KeyValuePair<string, AudioSource> pair in backgroundTracks )
            {
                pair.Value.Stop();
            }
        }
    }
}