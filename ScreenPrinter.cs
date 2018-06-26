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

        public int NumTexts
        {
            get
            {
                if( Texts == null )    return 0;

                return Texts.Length; 
            }
        }

        public Text[] Texts;

        private Image[] UIPanels;

        public bool UsePanels;

        public Color DefaultColor = Color.black;

        Dictionary<int, float> timesToDestroyTexts;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            timesToDestroyTexts = new Dictionary<int, float>();
            UIPanels = new Image[Texts.Length];
            for( int i = 0; i < Texts.Length; i++ )
            {
                Texts[i].enabled = false;
                UIPanels[i] = Texts[i].transform.parent.GetComponent<Image>();
                if( UIPanels[i] != null ) UIPanels[i].enabled = false;
            }
        }
               
        void Update()
        {
            removeExpiredMessages();
        }

//--------------------------------------------------------------------------METHODS:

        public void HideAllText()
        {
            for( int i = 0; i < Texts.Length; i++ )
            {
                enableText( i, false );
            }
        }

        /// <summary>
        /// Hides text
        /// </summary>
        /// <param name="textNumber"></param>
        public void HideText( int textNumber )
        {
            enableText( textNumber, false );
        }

        /// <summary>
        /// Prints given message on the textNumberth Text 
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        public void Print( int textNumber, string message )
        {
            Print( textNumber, message, DefaultColor );
        }

        /// <summary>
        /// Prints given message on the textNumberth Text 
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        public void Print( int textNumber, string message, Color color )
        {
            if( ! enableText( textNumber, true ) )
            {
                LOG_TAG.TPrint( "Pick textnumber between 0 and " + Texts.Length );
                return;
            }
            Texts[textNumber].color = color;
            Texts[textNumber].text = message;
        }

        /// <summary>
        /// Prints given message on the first Text object
        /// </summary>
        /// <param name="text"></param>
        public void Print( string text )
        {
            Print( 0, text, DefaultColor );
        }

        /// <summary>
        /// Prints given message on the first Text object
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void Print( string text, Color color )
        {
            Texts[0].color = color;
            Print( 0, text, color );
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
            Toast( textNumber, message, DefaultColor, duration );
        }

        /// <summary>
        /// Displays given message on given text object for duration seconds
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void Toast( int textNumber,
                           string message,
                           Color color,
                           float duration = DEFAULT_TOAST_TIME )
        {
            Print( textNumber, message, color );
            float timeToDestroy = Time.realtimeSinceStartup + duration;
            if( timesToDestroyTexts.ContainsKey( textNumber ) )
            {
                timesToDestroyTexts[textNumber] = timeToDestroy;
            }
            else
            {
                timesToDestroyTexts.Add( textNumber, timeToDestroy );
            }
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

        /// <summary>
        /// Displays given message on first text object for duration seconds
        /// </summary>
        /// <param name="textNumber"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void Toast( string message, 
                           Color color, 
                           float duration = DEFAULT_TOAST_TIME )
        {
            Toast( 0, message, color, duration );
        }

//--------------------------------------------------------------------------HELPERS:

        // returns true if successful
        private bool enableText( int textNum, bool enabled )
        {
            if( textNum < 0  || textNum >= Texts.Length )
            {
                return false;
            }

            if( UsePanels  &&  UIPanels[textNum] != null )
            {
                UIPanels[textNum].enabled = enabled;
            }
            Texts[textNum].enabled = enabled;
            return true;
        }

        private void removeExpiredMessages()
        {
            if( timesToDestroyTexts == null )   return;
            if( timesToDestroyTexts.Count == 0 )   return;

            float time = Time.realtimeSinceStartup;
            List<int> toRemove = new List<int>();

            foreach( KeyValuePair<int, float> pair in timesToDestroyTexts )
            {
                if( time > pair.Value )
                {
                    HideText( pair.Key );
                    toRemove.Add( pair.Key );
                }
            }
            foreach( int key in toRemove )
            {
                timesToDestroyTexts.Remove( key );
            }
        }
    }
}