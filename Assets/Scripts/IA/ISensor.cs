using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieScape.AI
{
    public interface ISensor
    {
        Transform TargetTransform { get; }
        bool IsTargetDetected();
        float DistanceToTarget();
    }
}
