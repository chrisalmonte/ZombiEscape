using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ZombieScape.AI
{
    public class ChaseStateAI : IAIState
    {
        private NavMeshAgent _navAgent;
        private Transform _target;

        private float _minDistance;

        private bool _canChange;

        public ChaseStateAI(NavMeshAgent navAgent, Transform target, float minDistance)
        {
            _navAgent = navAgent;
            _target = target;
            _minDistance = minDistance;
        }
        public void StartState()
        {
            _canChange = false;
        }

        public void Tick(float deltaTime)
        {
            float distanceToTarget = Vector3.Distance(_navAgent.transform.position, _target.position);

            if (distanceToTarget <= _minDistance)
            {

                _navAgent.SetDestination(_navAgent.transform.position);
                _canChange = true;
        
                return;
            }

            _navAgent.SetDestination(_target.position);
        }
        public bool CanChangeState()
        {
            return _canChange;
        }
        public string ToStateNameString()
        {
            return "AI_State : Chase";
        }
    }
}
