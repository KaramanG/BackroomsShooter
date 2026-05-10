using UnityEngine;

namespace BackroomsShooter.Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float SmoothSpeed = 0.125f;
        public Vector3 Offset = new Vector3(0, 15, 0);

        public GameObject GlobalVolume;

        private void Start()
        {
            if (GlobalVolume != null) GlobalVolume.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (Target == null) return;

            Vector3 desiredPosition = Target.position + Offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
            transform.position = smoothedPosition;
        }

    }
}