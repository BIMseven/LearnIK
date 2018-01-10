using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public static class MyCollectionsUtility
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "MyCollectionsUtility";

//---------------------------------------------------------------------------FIELDS:


//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Returns a stack build from given list.  The first element of the list
        /// will be on the top of the stack (first to be popped off)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Stack<T> ToStack<T>( this List<T> list )
        {
            Stack<T> stack = new Stack<T>();
            for( int i = list.Count - 1; i >= 0; i-- )
            {
                stack.Push( list[i] );
            }
            return stack;
        }

        public static List<Vector3> LastElement( this List<List<Vector3>> list )
        {
            return list[list.Count - 1];
        }

        public static Vector3 LastElement( this List<Vector3> list )
        {
            return list[list.Count - 1];
        }
        
        public static Vector2 LastElement( this List<Vector2> list )
        {
            return list[list.Count - 1];
        }
        
        public static float LastElement( this List<float> list )
        {
            return list[list.Count - 1];
        }

        public static T OneAtRandom<T>( this List<T> list )
        {
            return list[Random.Range( 0, list.Count )];
        }

        public static T OneAtRandom<T>( this HashSet<T> set )
        {
            return PullRandom<T>( set, 1 )[0];
        }
        
        public static T OneAtRandom<T>( this T[] array )
        {
            return array[Random.Range( 0, array.Length )];
        }

        /// <summary>
        /// Pulls numToPull random elements from given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] PullRandom<T>( this List<T> list, int numToPull )
        {
            T[] randomElements = new T[numToPull];
            int[] randomIndices = Utility.RandomUniqueIndices( list.Count, numToPull );
            for( int i = 0; i < numToPull; i++ )
            {
                int index = randomIndices[i];
                randomElements[i] = list[index];
            }
            return randomElements;
        }

        /// <summary>
        /// Pulls numToPull random elements from given set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] PullRandom<T>( this HashSet<T> set, int numToPull )
        {
            return PullRandom<T>( new List<T>( set ), numToPull );
        }
        
        /// <summary>
        /// Pulls numToPull random elements from given set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] PullRandom<T>( this T[] array, int numToPull )
        {
            return PullRandom<T>( new List<T>( array ), numToPull );
        }
        
        public static void SetActive( this GameObject[] objects, bool active )
        {
            foreach( GameObject obj in objects )
            {
                obj.SetActive( active );
            }
        }


        /// <summary>
        /// Shuffles this list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>( this IList<T> list )
        {
            int listLength = list.Count;

            for( int i = 0; i < listLength; i++ )
            {
                int indexToSwapWith = UnityEngine.Random.Range( 0, listLength );
                T value = list[indexToSwapWith];
                list[indexToSwapWith] = list[i];
                list[i] = value;
            }
        }

//--------------------------------------------------------------------------HELPERS:

    }
}