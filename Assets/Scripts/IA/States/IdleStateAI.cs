using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieScape.AI
{
    public class IdleStateAI : IAIState
    {
        private ISensor _sensor;

        private bool _canChange;

        public IdleStateAI(ISensor sensor)
        {
            _sensor = sensor;
        }
        public void StartState()
        {
            _canChange = false;
        }

        public void Tick(float deltaTime)
        {
            if (_sensor.IsTargetDetected() && _sensor.DistanceToTarget() > 3)
            {
                _canChange = true;

                return;
            }
        }
        public bool CanChangeState()
        {
            return _canChange;
        }

        public string ToStateNameString()
        {
            return "AI_State : Idle";
        }
    }
}
