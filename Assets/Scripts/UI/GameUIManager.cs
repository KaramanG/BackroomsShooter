using BackroomsShooter.Core;
using BackroomsShooter.Generation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BackroomsShooter.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private BGM bgm;

        [Header("Panels")]
        [SerializeField] private GameObject activeUI;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject deathPanel;

        [Header("Pause Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button exitButton;

        [Header("Death Buttons")]
        [SerializeField] private Button mainMenuButton;

        private void Start()
        {
            resumeButton.onClick.AddListener(ResumeGame);
            saveButton.onClick.AddListener(SaveGame);
            exitButton.onClick.AddListener(ToMainMenu);
            mainMenuButton.onClick.AddListener(ToMainMenu);

            Player.PlayerResources.OnPlayerDeath += ShowDeathScreen;
        }

        private void OnDestroy()
        {
            Player.PlayerResources.OnPlayerDeath -= ShowDeathScreen;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.CurrentState == GameState.Gameplay) PauseGame();
                else if (GameManager.Instance.CurrentState == GameState.Paused) ResumeGame();
            }
        }

        public void PauseGame()
        {
            pausePanel.SetActive(true);
            bgm.MuffleAudio();
            GameManager.Instance.ChangeState(GameState.Paused);
        }

        public void ResumeGame()
        {
            pausePanel.SetActive(false);
            bgm.RestoreAudio();
            GameManager.Instance.ChangeState(GameState.Gameplay);
        }

        public void SaveGame()
        {
            var playerRes = FindFirstObjectByType<Player.PlayerResources>();
            var levelGen = FindFirstObjectByType<LevelGenerator>();
            var boss = GameObject.FindGameObjectWithTag("Boss");

            SaveData data = new SaveData
            {
                PlayerHP = playerRes.CurrentHealth,
                PlayerAmmo = playerRes.CurrentAmmo,
                PlayerMagazines = playerRes.CurrentMagazines,
                WeaponName = playerRes.CurrentWeaponData.WeaponName,

                PlayerX = playerRes.transform.position.x,
                PlayerZ = playerRes.transform.position.z,

                LevelSeed = levelGen.Seed,
                LevelIndex = LevelManager.Instance.CurrentLevelIndex,

                BossX = boss.transform.position.x,
                BossZ = boss.transform.position.z
            };

            SaveSystem.Save(data);
        }

        public void ShowDeathScreen()
        {
            deathPanel.SetActive(true);
            bgm.PauseAudio();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }

        public void ToMainMenu()
        {
            Time.timeScale = 1f;
            GameManager.Instance.ChangeState(GameState.MainMenu);
        }
    }
}