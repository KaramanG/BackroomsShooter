using UnityEngine;

namespace BackroomsShooter.Core
{
    public class LevelExit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelManager.Instance.GoToNextLevel();
                Destroy(gameObject);
            }
        }
    }
}