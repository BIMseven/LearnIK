﻿using System.Collections;
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

//--------------------------------------------------------------------------HELPERS:

    }
}