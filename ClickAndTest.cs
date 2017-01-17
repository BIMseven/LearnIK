using UnityEngine;
using System.Collections;
using MyUtility;

public class ClickAndTest : MonoBehaviour {

//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ClickAndTest";

//---------------------------------------------------------------------------FIELDS:

//--------------------------------------------------------------------------METHODS:

	void OnMouseDown()
	{
		Utility.Print ( LOG_TAG, "hello");

	}

//--------------------------------------------------------------------------HELPERS:


}
