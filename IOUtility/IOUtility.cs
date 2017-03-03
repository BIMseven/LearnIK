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

        /// <summary>
        /// Returns the FileInfo of all files in given directory with given extension
        /// </summary>
        /// <param name="localPath">Path of directory, such as 'Assets/Resources/Blarg'</param>
        /// <param name="fileExtension">Such as '.txt' or 'csv'</param>
        /// <returns></returns>
        public static FileInfo[] GetFilesOfType( string localPath, string fileExtension )
        {

            DirectoryInfo info = new DirectoryInfo( localPath );
            FileInfo[] filesInfo = info.GetFiles();
            List<FileInfo> culledFiles = new List<FileInfo>();

            // Go through each file, and Generate if it's a .csv file
            foreach( FileInfo file in filesInfo )
            {
                if( file.Name.EndsWith( fileExtension ) )
                {
                    culledFiles.Add( file );
                }
            }
            return culledFiles.ToArray();
        }

        /// <summary>
        /// Loads all of the given files (all of which must be located in the 
        /// given directory)
        /// </summary>
        /// <param name="pathFromResources"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static TextAsset[] LoadTextAssets( string pathFromResources,
                                                  FileInfo[] files )
        {
            List<TextAsset> assetList = new List<TextAsset>();
            foreach( FileInfo file in files )
            {
                // Cut off the extension to get the nameNoType
                string nameNoType = file.Name.Remove( file.Name.IndexOf( '.' ) );
                string filePath = pathFromResources + "/" + nameNoType;
                assetList.Add( Resources.Load( filePath ) as TextAsset );
            }
            return assetList.ToArray();
        }

        /// <summary>
        /// Loads all files in given directory (in Assets/Resources) with given file extension
        /// </summary>
        /// <param name="pathFromResources"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static TextAsset[] LoadTextAssets( string pathFromResources, 
                                                  string fileExtension )
        {
            string localPath = "Assets/Resources/" + pathFromResources;
            FileInfo[] filesInfo = GetFilesOfType( localPath, fileExtension );

            return LoadTextAssets( pathFromResources, filesInfo );
        }

        /// <summary>
        /// Returns the name of the given file with the file extension cut off. So
        /// if called on a file with name blarg.txt, this would return blarg
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string NameNoExtension( this FileInfo file )
        {
            return file.Name.Remove( file.Name.IndexOf( '.' ) );
        }
    }    
}

