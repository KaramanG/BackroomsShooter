using UnityEngine;

namespace BackroomsShooter.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameManager gameManagerPrefab;

        private void Awake()
        {
            if (GameManager.Instance == null)
            {
                Instantiate(gameManagerPrefab);
            }

            GameManager.Instance.ChangeState(GameState.MainMenu);
        }
    }
}