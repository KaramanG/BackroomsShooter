using BackroomsShooter.Core;
using System.Collections;
using UnityEngine;

namespace BackroomsShooter.Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public int Health = 30;

        [Header("Loot Settings")]
        public GameObject MagazinePrefab;
        public GameObject WeaponPrefab;

        [Range(0, 100)] 
        public float MagazineDropChance = 15f;
        [Range(0, 100)] 
        public float WeaponDropChance = 5f;

        private bool _isDead = false;

        public void TakeDamage(int amount)
        {
            if (_isDead) return;
            Health -= amount;
            if (Health <= 0) Die();
        }

        private void Die()
        {
            _isDead = true;
            SpawnLoot();
            StartCoroutine(FadeAndDestroy());
        }

        private void SpawnLoot()
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= WeaponDropChance)
                Instantiate(WeaponPrefab, transform.position, Quaternion.identity);
            else if (roll <= MagazineDropChance + WeaponDropChance)
                Instantiate(MagazinePrefab, transform.position, Quaternion.identity);
        }

        private IEnumerator FadeAndDestroy()
        { 
            GetComponent<Collider>().enabled = false;
            if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent)) agent.enabled = false;

            float timer = 0;
            Vector3 startScale = transform.localScale;

            while (timer < 1f)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer);
                yield return null;
            }

            Destroy(gameObject);
        }

    }
}