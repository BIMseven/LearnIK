using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MyUtility
{
    /// <summary>
    /// This class can provide RealtimeSinceStartp from inside a non-main thread.
    /// </summary>
    public class ThreadFriendlyTime
    {
//------------------------------------------------------------------------CONSTANTS:

	    private const string LOG_TAG = "ThreadFriendlyTime";

//---------------------------------------------------------------------------FIELDS:
	
        public static float RealtimeSinceStartup
        {
            get
            {
                return startTime + stopwatch.ElapsedMilliseconds * 0.001f;
            }
        }

        private static Stopwatch stopwatch;
        private static float startTime;
        
//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Refresh with current real time since startup.  Must be called from main
        /// thread to maintain accuracy.  This clock will lose about 5 seconds every
        /// 10 hours
        /// </summary>
        public static void Wind()
        {
            stopwatch = new Stopwatch();
            startTime = Time.realtimeSinceStartup;
            stopwatch.Start();
        }

//--------------------------------------------------------------------------HELPERS:	
    }
}