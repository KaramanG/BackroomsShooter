using BackroomsShooter.Core;
using UnityEngine;

namespace BackroomsShooter.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public Transform FirePoint;
        public GameObject BulletPrefab;

        private float _nextFireTime;
        private PlayerResources _playerResources;

        private void Start()
        {
            _playerResources = GetComponent<PlayerResources>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && !_playerResources.IsReloading) _playerResources.Reload();

            if (Input.GetButton("Fire1") && Time.time >= _nextFireTime && !_playerResources.IsReloading) Shoot();
        }

        private void Shoot()
        {
            if (Time.timeScale == 0f) return;

            WeaponData data = _playerResources.CurrentWeaponData;
            if (data == null || _playerResources.IsReloading) return;

            if (_playerResources.CurrentAmmo <= 0)
            {
                _playerResources.Reload();
                return;
            }

            _playerResources.UseAmmo();
            _nextFireTime = Time.time + data.FireRate;

            if (BulletPrefab != null) 
                Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

            int layerMask = ~LayerMask.GetMask("Player");
            if (Physics.Raycast(FirePoint.position, FirePoint.forward, out RaycastHit hit, 50f, layerMask))
            {
                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null) target.TakeDamage(data.Damage);
            }

            NoiseManager.MakeNoise(transform.position, data.NoiseRadius);
        }

    }
}