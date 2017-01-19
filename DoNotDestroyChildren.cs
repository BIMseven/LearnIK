using UnityEngine;
using System.Collections;

public class DoNotDestroyChildren : MonoBehaviour
{

    void Awake()
    {
        foreach( Transform child in transform )
        {
            DontDestroyOnLoad( child.gameObject );
        }
    }
    
	void OnDestroy()
	{
		foreach( Transform child in transform )
		{
			child.parent = null;
		}
	}
}
