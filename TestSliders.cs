using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUtility
{
    public class TestSliders : Singleton<TestSliders>
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "GUIValues";
	    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	
        public Slider SliderA;
        public string NameA;
        public Vector2 RangeA;
        public Text TextA;
        public float ValueA { get; private set; }

        public Slider SliderB;
        public string NameB;
        public Vector2 RangeB;
        public Text TextB;
        public float ValueB { get; private set; }

        public Slider SliderC;
        public string NameC;
        public Vector2 RangeC;
        public Text TextC;
        public float ValueC { get; private set; }

//---------------------------------------------------------------------MONO METHODS:

        void Start() 
	    {

	    }
    

        void Update()
        {
            ValueA = updateSlider( SliderA, NameA, RangeA, TextA );
            ValueB = updateSlider( SliderB, NameB, RangeB, TextB );
            ValueC = updateSlider( SliderC, NameC, RangeC, TextC );
        }

//--------------------------------------------------------------------------METHODS:
    
//--------------------------------------------------------------------------HELPERS:
	
        private float updateSlider( Slider slider, 
                                   string name, 
                                   Vector2 range, 
                                   Text text )
        {
            float value = slider.value * ( range.y - range.x ) + range.x;
            text.text = name + "  " + value;
            return value;
        }
    }
}
