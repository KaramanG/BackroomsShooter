using System.Collections.Generic;
using UnityEngine;

namespace BackroomsShooter.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance {  get; private set; }

        public List<LevelSettings> Levels;
        public int CurrentLevelIndex = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public LevelSettings GetCurrentLevel() => Levels[CurrentLevelIndex];

        public void GoToNextLevel()
        {
            CurrentLevelIndex++;
            if (CurrentLevelIndex < Levels.Count)
                FindFirstObjectByType<Generation.LevelGenerator>().GenerateLevel();
            else
                FindFirstObjectByType<UI.GameUIManager>().ShowWinScreen();
        }

    }
}