using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtility
{
    public class SpaceAdjuster : AxisSwapper
    {
//------------------------------------------------------------------------CONSTANTS:

        public enum TargetSpaces { World, Local, Root }

//---------------------------------------------------------------------------FIELDS:
	
        public TargetSpaces Space = TargetSpaces.World;
    
//--------------------------------------------------------------------------METHODS:

        public void SetSpaceAdjustedPosition( Vector3 position )
        {
            switch( Space )
            {
                case TargetSpaces.World:
                    transform.position = position;
                    break;
                case TargetSpaces.Root:
                    transform.position = transform.root.position +
                                         transform.root.rotation * position;
                    break;
                case TargetSpaces.Local:
                    transform.localPosition = position;
                    break;
            }
        }

        public void SetSpaceAdjustedRotation( Quaternion rotation )
        {
            switch( Space )
            {
                case TargetSpaces.World:
                    SetAttitude( rotation );
                    break;
                case TargetSpaces.Root:
                    rotation = transform.root.rotation * rotation;
                    SetAttitude( rotation );
                    break;
                case TargetSpaces.Local:
                    SetLocalAttitude( rotation );
                    break;
            }
        }

        public Quaternion ToLocalSpace( Quaternion rotation )
        {
            Quaternion localSpace = Quaternion.identity;

            switch( Space )
            {
                case TargetSpaces.World:
                    localSpace = transform.rotation.Inverse() * rotation;
                    return LocalLookRotation( localSpace );

                case TargetSpaces.Root:
                    var worldSpace = transform.root.rotation.Inverse() * rotation;
                    localSpace = transform.rotation.Inverse() * rotation;                    
                    return LocalLookRotation( localSpace );

                case TargetSpaces.Local:
                    localSpace = rotation;
                    return LocalLookRotation( localSpace );
            }
            return Quaternion.identity;
        }
    }
}