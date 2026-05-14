using UnityEngine;

namespace BackroomsShooter.Enemy
{
    public class BossHealth : EnemyHealth
    {
        [Header("Boss Specific")]
        public GameObject ExitPrefab;

        protected override void Die()
        {
            Instantiate(ExitPrefab, transform.position, Quaternion.identity);
            base.Die();
        }
    }
}