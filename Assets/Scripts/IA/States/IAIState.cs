using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieScape.AI
{
    public interface IAIState
    {
        void StartState();
        void Tick(float deltaTime);
        bool CanChangeState();
        string ToStateNameString();
    }
}
