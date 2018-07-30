﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class SpaceAdjuster : AxisSwapper
{
//------------------------------------------------------------------------CONSTANTS:

    public enum TargetSpaces { World, Local, Root }

//---------------------------------------------------------------------------FIELDS:
	
    public TargetSpaces Space = TargetSpaces.World;

//--------------------------------------------------------------------------METHODS:

    protected void setPosition( Vector3 position )
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

    protected void setRotation( Quaternion rotation )
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

}