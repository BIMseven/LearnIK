using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{

    /// <summary>
    /// This is a wrapper for the MKGlow asset.  To prepare an object, make sure 
    /// the material is using an MKGlow shader and that this script is attached
    /// to the object or one of its parents
    /// </summary>
    public class GlowingObject : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "GlowingObject";
        public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

        [Range( 0, 2.5f )]
        private float glowPower;
        public float GlowPower
        {
            get
            {
                return glowPower;
            }
            set
            {
                glowPower = value;
                updateMaterials();
            }
        }

        private float glowTextureStrength;
        public float GlowTextureStrength
        {
            get
            {
                return glowTextureStrength;
            }
            set
            {
                glowTextureStrength = value;
                updateMaterials();
            }
        }


        private Texture glowTexture;
        public Texture GlowTexture
        {
            get
            {
                return glowTexture;
            }
            set
            {
                glowTexture = value;
                if( glowTexture != null )
                {
                    foreach( Material material in glowingMaterials )
                    {
                        material.SetTexture( glowTextureHandle, value );
                    }
                }
            }
        }


        private Material[] glowingMaterials;
        private int glowTextureStrengthHandle;
        private int glowPowerHandle;
        private int glowTextureHandle;
        
 //---------------------------------------------------------------------MONO METHODS:

        void Start()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            glowingMaterials = new Material[renderers.Length];
            for( int i = 0; i < glowingMaterials.Length; i++ )
            {
                glowingMaterials[i] = renderers[i].material;
            }
            glowTextureStrengthHandle = Shader.PropertyToID( "_MKGlowTexStrength" );
            glowPowerHandle = Shader.PropertyToID( "_MKGlowPower" );
            glowTextureHandle = Shader.PropertyToID( "_MKGlowTex" );
        }
        
//--------------------------------------------------------------------------METHODS:

        public void GlowForSeconds( float seconds, 
                                    float power = 1, 
                                    float textureStrength = 1 )
        {
            glowPower = power;
            glowTextureStrength = textureStrength;
            StartCoroutine( glowForSecondsRoutine( seconds ) );
        }
        
//--------------------------------------------------------------------------HELPERS:

        private IEnumerator glowForSecondsRoutine( float secondsToGlow )
        {

            yield return new WaitForSeconds( secondsToGlow );
            secondsToGlow = 0;
            glowTextureStrength = 0;
            glowPower = 0;
        }

        private void updateMaterials()
        {
            foreach( Material material in glowingMaterials )
            {
                material.SetFloat( glowTextureStrengthHandle, glowTextureStrength );
                material.SetFloat( glowPowerHandle, glowPower );
            }
        }
    }
}