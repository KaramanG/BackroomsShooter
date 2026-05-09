using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackroomsShooter.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentState = GameState.Bootstrapper;
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    SceneManager.LoadScene("MainMenu");
                    break;

                case GameState.GeneratingLevel:
                    break;

                case GameState.Gameplay:
                    break;

                case GameState.Paused:
                    break;

                case GameState.GameOver:
                    break;
            }
        }
    }
}