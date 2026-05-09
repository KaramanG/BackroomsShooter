using BackroomsShooter.Core;
using System.Collections;
using UnityEngine;

namespace BackroomsShooter.Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public int Health = 30;
        private bool _isDead = false;

        public void TakeDamage(int amount)
        {
            if (_isDead) return;

            Health -= amount;
            Debug.Log($"Enemy took damage! HP: {Health}");

            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            Debug.Log("Enemy has died.");

            StartCoroutine(FadeAndDestroy());
        }

        private IEnumerator FadeAndDestroy()
        {
            GetComponent<Collider>().enabled = false;

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