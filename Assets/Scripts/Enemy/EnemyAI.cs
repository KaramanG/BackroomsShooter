using BackroomsShooter.Core;
using UnityEngine;
using UnityEngine.AI;

namespace BackroomsShooter.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public enum AIState { Idle, Chase, Investigate, Attack };
        public AIState CurrentState = AIState.Idle;

        [Header("Detection Settings")]
        public float DetectionRange = 15f;

        [Header("Attack Settings")]
        public int AttackDamage = 10;
        public float AttackRange = 2f;
        public float AttackRate = 1.5f;

        private NavMeshAgent _agent;
        private Transform _player;
        private Vector3 _lastNoisePosition;
        private float _nextAttackTime;
        private IDamageable _playerDamageable;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
                _playerDamageable = playerObj.GetComponent<IDamageable>();
            }

            NoiseManager.OnNoiseCreated += HearNoise;

            _agent.stoppingDistance = AttackRange - 0.5f;
        }

        private void Awake()
        {
            float difficultyMultiplier = LevelManager.Instance.Levels[LevelManager.Instance.CurrentLevelIndex].DifficultyMultiplier;

            DetectionRange = DetectionRange * difficultyMultiplier;
            AttackDamage = (int)(AttackDamage * difficultyMultiplier);
            AttackRate = AttackRate / difficultyMultiplier;
        }

        private void Update()
        {
            if (_player == null) return;
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            switch (CurrentState)
            {
                case AIState.Idle:
                    if (distanceToPlayer <= DetectionRange)
                        CurrentState = AIState.Chase;
                    break;

                case AIState.Investigate:
                    if (!_agent.isActiveAndEnabled) break;
                    _agent.SetDestination(_lastNoisePosition);

                    if (_agent.remainingDistance <= _agent.stoppingDistance)
                        CurrentState = AIState.Idle;

                    if (distanceToPlayer <= DetectionRange)
                        CurrentState = AIState.Chase;
                    break;
                
                case AIState.Chase:
                    if (!_agent.isActiveAndEnabled) break;
                    _agent.SetDestination(_player.position);

                    if (distanceToPlayer <= AttackRange)
                        CurrentState = AIState.Attack;

                    if (distanceToPlayer > DetectionRange * 1.5f)
                    {
                        CurrentState = AIState.Idle;
                        _agent.ResetPath();
                    }
                    break;

                case AIState.Attack:
                    if (!_agent.isActiveAndEnabled) break;

                    Vector3 lookDir = _player.position - transform.position;
                    lookDir.y = 0;
                    if (lookDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookDir);

                    if (Time.time >= _nextAttackTime)
                    {
                        TryDealDamage();
                        _nextAttackTime = Time.time + AttackRate;
                    }

                    if (distanceToPlayer > AttackRange)
                        CurrentState = AIState.Chase;
                    break;
            }

        }

        private void HearNoise(Vector3 position, float radius)
        {
            if (CurrentState == AIState.Chase || CurrentState == AIState.Attack) return;

            float distanceToNoise = Vector3.Distance(transform.position, position);
            if (distanceToNoise <= radius)
            {
                _lastNoisePosition = position;
                CurrentState = AIState.Investigate;
            }
        }

        private void TryDealDamage()
        {
            if (_playerDamageable != null)
                _playerDamageable.TakeDamage(AttackDamage);
        }

        private void OnDestroy()
        {
            NoiseManager.OnNoiseCreated -= HearNoise;
        }

    }
}