using UnityEngine;
using UnityEngine.AI;

namespace BackroomsShooter.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public enum AIState { Idle, Chase, Investigate };
        public AIState CurrentState = AIState.Idle;

        public float DetectionRange = 15f;

        private NavMeshAgent _agent;
        private Transform _player;
        private Vector3 _lastNoisePosition;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }

            Core.NoiseManager.OnNoiseCreated += HearNoise;
        }

        private void Update()
        {
            if (_player == null) return;
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            switch (CurrentState)
            {
                case AIState.Idle:
                    if (distanceToPlayer <= DetectionRange)
                    {
                        CurrentState = AIState.Chase;
                    }
                    break;

                case AIState.Investigate:
                    if (!_agent.isActiveAndEnabled) break;
                    _agent.SetDestination(_lastNoisePosition);
                    if (_agent.remainingDistance <= _agent.stoppingDistance)
                    {
                        CurrentState = AIState.Idle;
                    }

                    if (distanceToPlayer <= DetectionRange)
                    {
                        CurrentState = AIState.Chase;
                    }
                    break;
                
                case AIState.Chase:
                    if (!_agent.isActiveAndEnabled) break;
                    _agent.SetDestination(_player.position);
                    if (distanceToPlayer > DetectionRange * 1.5f)
                    {
                        CurrentState = AIState.Idle;
                        _agent.ResetPath();
                    }
                    break;
            }

        }

        private void HearNoise(Vector3 position, float radius)
        {
            if (CurrentState == AIState.Chase) return;

            float distanceToNoise = Vector3.Distance(transform.position, position);
            if (distanceToNoise <= radius)
            {
                _lastNoisePosition = position;
                CurrentState = AIState.Investigate;
            }
        }

        private void OnDestroy()
        {
            Core.NoiseManager.OnNoiseCreated -= HearNoise;
        }

    }
}