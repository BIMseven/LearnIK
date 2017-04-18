using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
//---------------------------------------------------------------------------FIELDS:

        protected static T instance;

        public static T Instance
        {
            // Getter method automatically called when this field is accessed
            get
            {
                if( instance == null )
                {
                    //try to find it
                    instance = (T)FindObjectOfType( typeof( T ) );
                    //if we can't find it:
                    if( instance == null )
                    {
                        //create it
                        GameObject container = new GameObject();
                        //name it
                        container.name = typeof( T ) + "Container";
                        //add the appropriate script
                        instance = (T)container.AddComponent( typeof( T ) );
                    }
                }
                return instance;
            }
        }
    }
}
