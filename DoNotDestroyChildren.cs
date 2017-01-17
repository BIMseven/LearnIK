using UnityEngine;
using System.Collections;

public class DoNotDestroyChildren : MonoBehaviour
{
	void OnDestroy()
	{
		foreach( Transform child in transform )
		{
			child.parent = null;//transform.parent;
		}
	}
}
