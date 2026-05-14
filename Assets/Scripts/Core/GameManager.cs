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
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("MainMenu");
                    break;

                case GameState.GeneratingLevel:
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("GameScene");
                    break;

                case GameState.Gameplay:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;
            }
        }

        public void QuitGame() => Application.Quit();

    }
}