using BackroomsShooter.Core;
using System.Collections;
using UnityEngine;

namespace BackroomsShooter.Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public int Health = 30;

        [Header("Loot Settings")]
        public GameObject MagazinePickupPrefab;
        [Range(0, 100)]
        public float MagazineDropChance = 20f;

        public GameObject HealthKitPrefab;
        [Range(0, 100)]
        public float HealthKitDropChance = 20f;

        public GameObject[] WeaponPickupPrefabs;
        [Range(0, 100)]
        public float WeaponDropChance = 5f;

        private bool _isDead = false;

        public void TakeDamage(int amount)
        {
            if (_isDead) return;
            Health -= amount;
            if (Health <= 0) Die();
        }

        protected virtual void Die()
        {
            _isDead = true;
            SpawnLoot();
            StartCoroutine(FadeAndDestroy());
        }

        private void SpawnLoot()
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= WeaponDropChance)
            {
                var weapon = WeaponPickupPrefabs[Random.Range(0, WeaponPickupPrefabs.Length)];
                Instantiate(weapon, transform.position, Quaternion.Euler(new Vector3(0, 90, 90)));
            }
                
            else if (roll <= WeaponDropChance + MagazineDropChance)
                Instantiate(MagazinePickupPrefab, transform.position, Quaternion.Euler(new Vector3(0, 90, 90)));

            else if (roll <= WeaponDropChance + MagazineDropChance + HealthKitDropChance)
                Instantiate(HealthKitPrefab, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));

        }

        private IEnumerator FadeAndDestroy()
        { 
            GetComponent<Collider>().enabled = false;
            if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent)) agent.enabled = false;

            GetComponent<Animator>().SetTrigger("Death");
            yield return new WaitForSeconds(3f);

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