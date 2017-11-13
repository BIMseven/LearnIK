using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public static class MeshUtil
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "MeshUtil";

//---------------------------------------------------------------------------FIELDS:

        public static Vector3 WorldPointFromUV( this GameObject obj, Vector2 uv )
        {
            MeshFilter meshFilter = obj.FindComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            Transform transform = meshFilter.transform;

            int[] triangles = mesh.triangles;
            Vector2[] uvs = mesh.uv;
            Vector3[] vertices = mesh.vertices;

            for( int i = 0; i < triangles.Length; i += 3 )
            {
                // Grab uvs of next triangle
                Vector2 uv1 = uvs[triangles[i]];
                Vector2 uv2 = uvs[triangles[i + 1]];
                Vector2 uv3 = uvs[triangles[i + 2]];

                // Calc triangle area, skip if zero
                float area = triArea( uv1, uv2, uv3 );
                if( area == 0 )   continue;

                // Calc barycentric coordinates
                float a1 = triArea( uv2, uv3, uv ) / area;
                float a2 = triArea( uv3, uv1, uv ) / area;
                float a3 = triArea( uv1, uv2, uv ) / area;
                if( a1 < 0   ||   a2 < 0   ||   a3 < 0 )   continue;

                // Find point inside the triangle
                Vector3 point = a1 * vertices[triangles[i]] +
                                a2 * vertices[triangles[i + 1]] +
                                a3 * vertices[triangles[i + 2]];

                // Return point in world space
                return transform.TransformPoint( point );
            }

            return Vector3.zero;
        }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:

        private static float triArea( Vector2 p1, Vector2 p2, Vector2 p3 )
        {
            Vector2 v1 = p1 - p3;
            Vector2 v2 = p2 - p3;
            return ( v1.x * v2.y - v1.y * v2.x ) / 2.0f;
        }

    }
}