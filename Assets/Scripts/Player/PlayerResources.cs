using UnityEngine;
using BackroomsShooter.Core;
using System;
using System.Collections;

namespace BackroomsShooter.Player
{
    public class PlayerResources : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        public int MaxHealth = 100;
        public int CurrentHealth;

        [Header("Stamina")]
        public float MaxStamina = 100f;
        public float CurrentStamina;
        public float StaminaRegenRate = 15f;

        [Header("Ammo")]
        public int CurrentAmmo;
        public int CurrentMagazines = 3;
        public int MaxMagazines = 3;
        public bool IsReloading = false;
        public WeaponData CurrentWeaponData;

        public static event Action OnResourcesChanged;
        public static event Action OnPlayerDeath;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            if (CurrentWeaponData != null) CurrentAmmo = CurrentWeaponData.MaxAmmo;
        }

        private void Update()
        {
            RegenStamina();
        }

        public void NotifyUI() => OnResourcesChanged?.Invoke();

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            NotifyUI();

            if (CurrentHealth <= 0)
            {
                OnPlayerDeath?.Invoke();
            }
        }

        public bool ConsumeStamina(float amount)
        {
            if (CurrentStamina >= amount)
            {
                CurrentStamina -= amount;
                NotifyUI();
                return true;
            }
            return false;
        }
        
        public void UseAmmo()
        {
            if (CurrentAmmo > 0)
            {
                CurrentAmmo--;
                NotifyUI();
            }
        }

        public void AddMagazine()
        {
            if (CurrentMagazines < MaxMagazines)
            {
                CurrentMagazines++;
                NotifyUI();
            }
        }

        public void Reload()
        {
            if (CurrentMagazines > 0 && CurrentAmmo < CurrentWeaponData.MaxAmmo)
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

        private IEnumerator ReloadCoroutine()
        {
            IsReloading = true;
            yield return new WaitForSeconds(CurrentWeaponData.ReloadTime);

            CurrentMagazines--;
            CurrentAmmo = CurrentWeaponData.MaxAmmo;
            NotifyUI();
            IsReloading = false;
        }

        private void RegenStamina()
        {
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += StaminaRegenRate * Time.deltaTime;
                CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
                NotifyUI();
            }
        }

    }
}