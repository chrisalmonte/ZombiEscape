using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicExtension
{
    public static int ToLoopArray(this int a, int arrayLength)
    {
        int nextPosition = a + 1 < arrayLength ? (a + 1) : 0;
        return nextPosition;

    }
}
