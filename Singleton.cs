using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
//---------------------------------------------------------------------------FIELDS:

        public static T Instance
        {
            get
            {
                if( applicationIsQuitting )  return null;
                
                if( instance == null )
                {
                    //var container = GameObject.CreatePrimitive( PrimitiveType.Cube );
                    //Destroy( container.GetComponent<Renderer>() );
                    //Destroy( container.GetComponent<Mesh>() );
                    //Destroy( container.GetComponent<BoxCollider>() );
                    var container = new GameObject();

                    container.name  = typeof( T ) + "Container";
                    instance = (T)container.AddComponent( typeof( T ) );
                }
                return instance;
            }
        }

        public static bool InstanceExists
        {
            get
            {
                if( applicationIsQuitting )   return false;

                return  instance != null;
            }
        }
        
        protected static T instance;
        private static bool applicationIsQuitting = false;

//---------------------------------------------------------------------MONO METHODS:

	    protected virtual void Awake ()
	    {
		    if( instance == null )
            {
                instance = this as T;                
		    }
		    else
		    {
			    Destroy ( gameObject );
		    }
	    }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}
