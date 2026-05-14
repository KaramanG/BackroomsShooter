using BackroomsShooter.Core;
using UnityEngine;

namespace BackroomsShooter.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void OnStartClicked()
        {
            GameManager.Instance.ChangeState(GameState.GeneratingLevel);
        }

        public void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }
    }
}