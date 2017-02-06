using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace MyUtility
{
    public static class IOUtility
    {
        /// <summary>
        /// Copies bytesInSource bytes from source to destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesInSource"></param>
        public static void CopyToStream( Stream source,
                                         Stream destination,
                                         byte[] buffer,
                                         int bytesInSource )
        {
            while( bytesInSource > 0 )
            {
                int bytesToCopy = Mathf.Min( buffer.Length, bytesInSource );
                int read = source.Read( buffer, 0, bytesToCopy );
                destination.Write( buffer, 0, read );
                bytesInSource -= read;
            }
        }
    }


    
}

