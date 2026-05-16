using UnityEngine;

namespace BackroomsShooter.UI
{
    public class Compass : MonoBehaviour
    {
        public RectTransform Arrow;
        public float OrbitRadius = 100f;

        public Transform Target;

        private Transform _player;

        private void Start()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) _player = playerObject.transform;
        }

        private void Update()
        {
            if (_player == null || Target == null) return;

            Vector3 dir = Target.position - _player.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * OrbitRadius;
            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * OrbitRadius;

            Arrow.anchoredPosition = new Vector2(x, y);
            Arrow.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}