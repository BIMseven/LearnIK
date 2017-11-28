using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUtility
{
    public class ScreenPrinter : Singleton<ScreenPrinter> 
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "TextPrinter";
        public bool VERBOSE = false;

        private const float DEFAULT_TOAST_TIME = 2.0f;

//---------------------------------------------------------------------------FIELDS:

        public Text[] Texts;

        Dictionary<int, float> timesToDestroyTexts;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            timesToDestroyTexts = new Dictionary<int, float>();
        }

        void Start()
        {
            foreach( Text text in Texts )
            {
                text.enabled = false;
            }
        }
       
        void Update()
        {
            float time = Time.realtimeSinceStartup;
            foreach( KeyValuePair<int, float> pair in timesToDestroyTexts )
            {
                if( time > pair.Value )
                {
                    HideText( pair.Key );
                    timesToDestroyTexts.Remove( pair.Key );
                }
            }
        }

//--------------------------------------------------------------------------METHODS:

        public void HideAllText()
        {
            foreach( Text text in Texts )
            {
                text.enabled = false;
            }
        }

        /// <summary>
        /// Hides text
        /// </summary>
        /// <param name="textNumber"></param>
        public void HideText( int textNumber )
        {
            if( textNumber >= 0  &&  textNumber < Texts.Length )
            {
                Texts[textNumber].enabled = false;
            }
        }

        /// <summary>
        /// Prints given message on the textNumberth Text 
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        public void Print( int textNumber, string message )
        {
            if( textNumber >= Texts.Length )
            {
                LOG_TAG.TPrint( "Pick textnumber between 0 and " + Texts.Length );
                return;
            }
            Texts[textNumber].enabled = true;
            Texts[textNumber].text = message;
        }

        /// <summary>
        /// Prints given message on the first Text object
        /// </summary>
        /// <param name="text"></param>
        public void Print( string text )
        {
            Print( 0, text );
        }

        /// <summary>
        /// Displays given message on given text object for duration seconds
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void Toast( int textNumber, 
                           string message, 
                           float duration = DEFAULT_TOAST_TIME )
        {
            Print( textNumber, message );
            float timeToDestroy = Time.realtimeSinceStartup + duration;
            timesToDestroyTexts.Add( textNumber, timeToDestroy );                
        }

        /// <summary>
        /// Displays given message on first text object for duration seconds
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void Toast( string message, float duration = DEFAULT_TOAST_TIME )
        {
            Toast( 0, message, duration );
        }

//--------------------------------------------------------------------------HELPERS:

    }
}