using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using ZombieScape.AI;

namespace ZombieScape
{
    public class Test : MonoBehaviour
    {
        public event Action<string, int> OnStateChange;

        [SerializeField] private Transform _targetTest;
        [SerializeField] private float _minDistance = 2.0f;

        private IAIState[] _aIStateArray;
        private IAIState _currenState;
        private int _stateIndex;

        private void Awake()
        {
            ISensor eyeSensor = GetComponent<ISensor>();
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();


            _aIStateArray = new IAIState[2];

            _aIStateArray[0] = new IdleStateAI(eyeSensor);
            _aIStateArray[1] = new ChaseStateAI(navMeshAgent, _targetTest, _minDistance);
        }

        private void Start()
        {
            _stateIndex = 0;
            _currenState = _aIStateArray[_stateIndex];
            _currenState.StartState();
        }

        private void LateUpdate()
        {
            _currenState.Tick(Time.deltaTime);

            if (_currenState.CanChangeState())
            {
                _stateIndex = _stateIndex.ToLoopArray(_aIStateArray.Length);
                _currenState = _aIStateArray[_stateIndex];

                _currenState.StartState();
            }
        }


    }
    public class TestStateMachine<T> where T : IAIState
    {
        private T _state;
        private Color _colorState;
        private string _stateName;

        public TestStateMachine(T state, Color colorState, string stateName)
        {
            _state = state;
            _colorState = colorState;
            _stateName = stateName;
        }
    }
}

