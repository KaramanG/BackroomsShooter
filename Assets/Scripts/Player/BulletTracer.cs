using UnityEngine;

namespace BackroomsShooter.Player
{
    public class BulletTracer : MonoBehaviour
    {
        public float Speed = 100f;
        public float LifeTime = 2f;

        private void Start()
        {
            Destroy(gameObject, LifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) Destroy(gameObject);
        }

    }
}

