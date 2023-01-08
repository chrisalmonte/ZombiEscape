using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieScape.AI;

namespace ZombieScape
{
    public class EyeSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float _eyeDistance = 3;
        [field: SerializeField] public Transform TargetTransform { get; private set; }

        private bool _inDistance;

        private void LateUpdate()
        {
            float distanceToTarget = Vector3.Distance(transform.position, TargetTransform.position);
            _inDistance = distanceToTarget <= _eyeDistance;
        }
        public bool IsTargetDetected()
        {
            return _inDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _eyeDistance);
        }

        public float DistanceToTarget()
        {
            return Vector3.Distance(transform.position, TargetTransform.position);
        }
    }
}
