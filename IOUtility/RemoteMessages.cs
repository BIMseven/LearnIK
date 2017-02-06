using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoteMessages : byte
{
    Invalid = 0,

    Hello = 1,
    Options = 2,
    SerializableObject = 3,

    Reserved = 255,
}
