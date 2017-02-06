using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoteMessages : byte
{
    Invalid = 0,

    Hello = 1,
    String = 2,
    Options = 3,
    SerializableObject = 4,

    Reserved = 255,
}
