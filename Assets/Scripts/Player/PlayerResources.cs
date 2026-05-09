using UnityEngine;
using BackroomsShooter.Core;
using System;

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
        public WeaponData CurrentWeaponData;

        public static event Action OnResourcesChanged;
        public static event Action OnPlayerDeath;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;

            if (CurrentWeaponData != null)
            {
                CurrentAmmo = CurrentWeaponData.MaxAmmo;
            }
        }

        private void Update()
        {
            RegenStamina();
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            OnResourcesChanged?.Invoke();

            if (CurrentHealth <= 0)
            {
                OnPlayerDeath?.Invoke();
            }
        }

        public bool TryConsumeStamina(float amount)
        {
            if (CurrentStamina >= amount)
            {
                CurrentStamina -= amount;
                OnResourcesChanged?.Invoke();
                return true;
            }
            return false;
        }

        private void RegenStamina()
        {
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += StaminaRegenRate * Time.deltaTime;
                CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
                OnResourcesChanged?.Invoke();
            }
        }

        public bool UseAmmo()
        {
            if (CurrentAmmo > 0)
            {
                CurrentAmmo--;
                OnResourcesChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void AddAmmo(int amount)
        {
            CurrentAmmo = Mathf.Min(CurrentAmmo + amount, CurrentWeaponData.MaxAmmo);
            OnResourcesChanged?.Invoke();
        }

    }
}