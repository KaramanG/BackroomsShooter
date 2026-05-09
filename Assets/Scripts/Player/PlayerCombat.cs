using BackroomsShooter.Core;
using UnityEngine;

namespace BackroomsShooter.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public int Damage = 10;
        public float FireRate = 0.2f;
        public Transform FirePoint;
        public float NoiseRadius = 20f;

        private float _nextFireTime;
        private PlayerResources _resources;

        private void Start()
        {
            _resources = GetComponent<PlayerResources>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + FireRate;
            }
        }

        private void Shoot()
        {
            if (!_resources.UseAmmo())
            {
                Debug.Log("No ammo!");
                return;
            }

            Debug.DrawRay(FirePoint.position, FirePoint.forward * 50f, Color.red, 0.1f);

            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, 50f, layerMask))
            {
                Debug.Log($"Hit: {hit.collider.name}");

                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(Damage);
                }
            }

            NoiseManager.MakeNoise(transform.position, NoiseRadius);
            Debug.Log($"Noise event. Radius: {NoiseRadius}");
        }

    }
}