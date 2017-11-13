using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public class MyShaderEffect : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "MyShaderEffect";
        public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

        public string ShaderName;

        public Material ShaderMaterial
        {
            get
            {
                if( shaderMaterial == null )
                {
                    Shader shader = Shader.Find( ShaderName );
                    if( shader == null )
                    {
                        Debug.LogError( "Unable to find shader " + ShaderName );
                    }
                    shaderMaterial = new Material( shader );
                }
                return shaderMaterial;
            }
        }

        private Material shaderMaterial;

        private Dictionary<string, int> propertyIDs = new Dictionary<string, int>();

//---------------------------------------------------------------------MONO METHODS:


//--------------------------------------------------------------------------METHODS:

        public void SetFloat( string propertyName, float value )
        {
            int id = getID( propertyName );
            ShaderMaterial.SetFloat( id, value );            
        }

        public void SetInt( string propertyName, int value )
        {
            int id = getID( propertyName );
            ShaderMaterial.SetInt( id, value );
        }

//--------------------------------------------------------------------------HELPERS:

        private int getID( string propertyName )
        {
            int id;
            if( ! propertyIDs.ContainsKey( propertyName ) )
            {
                id = Shader.PropertyToID( propertyName );
                propertyIDs.Add( propertyName, id );
                return id;
            }
            else
            {
                propertyIDs.TryGetValue( propertyName, out id );
            }
            return id;            
        }
    }
}
