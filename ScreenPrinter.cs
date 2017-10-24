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

//---------------------------------------------------------------------------FIELDS:

        public Text[] Texts;

//---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            foreach( Text text in Texts )
            {
                text.enabled = false;
            }
        }
       
//--------------------------------------------------------------------------METHODS:

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

        public void Print( string text )
        {
            Print( 0, text );
        }

//--------------------------------------------------------------------------HELPERS:

    }
}