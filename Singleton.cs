using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
//---------------------------------------------------------------------------FIELDS:

        protected static T instance;
        private static object _lock = new object();
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            // Getter method automatically called when this field is accessed
            get
            {
                if( applicationIsQuitting )  return null;
                
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

                        DontDestroyOnLoad( container );
                    }
                }
                return instance;
            }
        }

//---------------------------------------------------------------------MONO METHODS:

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        public void OnLevelWasLoaded( int level )
        {
            applicationIsQuitting = false;
        }
    }
}
