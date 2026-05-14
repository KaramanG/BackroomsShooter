using BackroomsShooter.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BackroomsShooter.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button LoadButton;

        private void Awake()
        {
            if (!SaveSystem.HasSave()) LoadButton.interactable = false;
            else LoadButton.interactable = true;
        }

        public void OnStartClicked()
        {
            SaveSystem.ClearSave();
            GameManager.Instance.ChangeState(GameState.GeneratingLevel);
        }

        public void OnContinueClicked()
        {
            SaveSystem.Load();
            GameManager.Instance.ChangeState(GameState.GeneratingLevel);
        }

        public void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }
    }
}