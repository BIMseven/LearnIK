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
        public float GlowPower;
        public float GlowTextureStrength;
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

        private Texture glowTexture;

        private Material[] glowingMaterials;
        private int glowTextureStrengthHandle;
        private int glowPowerHandle;
        private int glowTextureHandle;

        private float secondsToGlow;
        private float secondGlowBegan;

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

        void Update()
        {
            // Check if time to turn off glow (GlowForSeconds may have been called)
            checkIfTimeToStopGlowing();

            // Update shader variables to correspond with Glow Power and Texture Strength
            updateMaterials();
        }

//--------------------------------------------------------------------------METHODS:

        public void GlowForSeconds( float seconds, 
                                    float power = 1, 
                                    float textureStrength = 1 )
        {
            secondsToGlow = seconds;
            secondGlowBegan = Time.realtimeSinceStartup;
            GlowPower = power;
            GlowTextureStrength = textureStrength;
        }

//--------------------------------------------------------------------------HELPERS:

        private void checkIfTimeToStopGlowing()
        {
            if( secondsToGlow > 0 )
            {
                if( Time.realtimeSinceStartup - secondGlowBegan >= secondsToGlow )
                {
                    secondsToGlow = 0;
                    GlowTextureStrength = 0;
                    GlowPower = 0;
                }
            }
        }

        private void updateMaterials()
        {
            foreach( Material material in glowingMaterials )
            {
                material.SetFloat( glowTextureStrengthHandle, GlowTextureStrength );
                material.SetFloat( glowPowerHandle, GlowPower );
            }
        }
    }
}