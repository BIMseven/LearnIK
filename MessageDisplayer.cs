using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// This script is meant to be attached to a Canvas object.
namespace MyUtility
{

    public class MessageDisplayer : Singleton<MessageDisplayer>
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "MessageDisplayer";
        private const bool VERBOSE = true;

        private const float DEFAULT_DISPLAY_TIME = float.MaxValue;

//---------------------------------------------------------------------------FIELDS:

        private List<Message> queuedMessages;
        private Message currentlyDisplayedMessage;
        private float secondsRemaining;

        private Canvas myCanvas;

        public bool showMessages;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            queuedMessages = new List<Message>();
            secondsRemaining = 0;
            currentlyDisplayedMessage = null;
            myCanvas = GetComponent<Canvas>();
        }

        void Update()
        {
            if( !showMessages ) return;

            if( currentlyDisplayedMessage != null )
            {
                secondsRemaining -= Time.deltaTime;

                if( secondsRemaining <= 0 )
                {
                    hideDisplayedMessage();
                    // TODO: display next message in queue
                }
            }
            else if( queuedMessages.Count > 0 )
            {
                Message message = queuedMessages[0];
                queuedMessages.RemoveAt( 0 );
                displayMessage( message );
            }

        }

//--------------------------------------------------------------------------METHODS:

        public void hideDisplayedMessage()
        {
            Text[] textComponents = myCanvas.GetComponentsInChildren<Text>();

            for( int i = 0; i < textComponents.Length; i++ )
            {
                textComponents[i].enabled = false;
            }
            currentlyDisplayedMessage = null;
        }

        // Displays the given message in the center of the screen
        public void displayMessage( Message message )
        {
            currentlyDisplayedMessage = message;
            secondsRemaining = message.displayTime;

            Text text = getTextComponents( 1 )[0];
            text.text = message.message;
            text.enabled = true;

            makeLikeSample( text, message.textSample );

            fullscreenAndCenter( text );
        }

        public void queueMessage( Message message )
        {
            queuedMessages.Add( message );
        }

//--------------------------------------------------------------------------HELPERS:

        private void fullscreenAndCenter( Text text )
        {
            RectTransform screen = myCanvas.GetComponent<RectTransform>();

            RectTransform rectangle = text.GetComponent<RectTransform>();

            // Center text
            rectangle.anchoredPosition = new Vector2( 0.0f, 0.0f );
            rectangle.sizeDelta = new Vector2( screen.rect.width, screen.rect.height );

            text.alignment = TextAnchor.MiddleCenter;
        }

        private Text[] getTextComponents( int numberNeeded )
        {
            Text[] textComponents = myCanvas.GetComponentsInChildren<Text>();
            int textObjectsToCreate = numberNeeded - textComponents.Length;

            for( int i = 0; i < textObjectsToCreate; i++ )
            {
                GameObject go = new GameObject( "Line" + ( textComponents.Length + i ) );
                go.transform.SetParent( myCanvas.gameObject.transform );
                //			Text newComponent = go.AddComponent<Text>() as Text;
            }
            textComponents = myCanvas.GetComponentsInChildren<Text>();

            // TODO: Is there really no splice method for unity collections?
            Text[] neededTextComponents = new Text[numberNeeded];
            for( int i = 0; i < numberNeeded; i++ )
            {
                neededTextComponents[i] = textComponents[i];
            }
            return neededTextComponents;
        }

        private void makeLikeSample( Text text, Text sampleText )
        {
            text.alignment = sampleText.alignment;
            text.font = sampleText.font;
            text.fontSize = sampleText.fontSize;
            text.alignment = sampleText.alignment;
            text.color = sampleText.color;
        }

        private void makeLikeSample( Text[] texts, Text sampleText )
        {
            for( int i = 0; i < texts.Length; i++ )
            {
                makeLikeSample( texts[i], sampleText );
            }
        }


    }
}