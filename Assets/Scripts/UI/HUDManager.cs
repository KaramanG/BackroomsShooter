using BackroomsShooter.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BackroomsShooter.UI
{
    public class HUDManager : MonoBehaviour
    {
        [Header("UI links")]
        public Slider HealthSlider;
        public TextMeshProUGUI HealthText;
        
        public Slider StaminaSlider;

        public TextMeshProUGUI AmmoText;
        public TextMeshProUGUI MagazineText;
        public Image WeaponImage;

        private PlayerResources _playerResources;

        private void Start()
        {
            _playerResources = FindFirstObjectByType<PlayerResources>();
            PlayerResources.OnResourcesChanged += UpdateUI;
            UpdateUI();
        }

        private void OnDestroy()
        {
            PlayerResources.OnResourcesChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            if (_playerResources == null) return;

            HealthSlider.maxValue = _playerResources.MaxHealth;
            HealthSlider.value = _playerResources.CurrentHealth;
            HealthText.text = _playerResources.CurrentHealth.ToString() + " / " + _playerResources.MaxHealth.ToString();

            StaminaSlider.maxValue = _playerResources.MaxStamina;
            StaminaSlider.value = _playerResources.CurrentStamina;

            AmmoText.text = _playerResources.CurrentAmmo.ToString();
            MagazineText.text = _playerResources.CurrentMagazines.ToString();

            if (_playerResources.CurrentWeaponData != null)
            {
                WeaponImage.enabled = true;
                WeaponImage.sprite = _playerResources.CurrentWeaponData.WeaponIcon;
            }
            else WeaponImage.enabled = false;
                
        }

    }
}