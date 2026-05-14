using UnityEngine;
using BackroomsShooter.Core;
using System;
using System.Collections;
using System.Collections.Generic;

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

        [Header("All Weapons")]
        public List<WeaponData> AllAvailableWeapons;

        public static event Action OnResourcesChanged;
        public static event Action OnPlayerDeath;

        private Animator _animator;
        private PlayerSFX _playerSFX;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerSFX = GetComponent<PlayerSFX>();

            if (SaveSystem.CachedData != null)
            {
                CurrentHealth = SaveSystem.CachedData.PlayerHP;
                CurrentAmmo = SaveSystem.CachedData.PlayerAmmo;
                CurrentMagazines = SaveSystem.CachedData.PlayerMagazines;

                if (!string.IsNullOrEmpty(SaveSystem.CachedData.WeaponName))
                {
                    WeaponData foundWeapon = AllAvailableWeapons.Find(w => w.WeaponName == SaveSystem.CachedData.WeaponName);
                    if (foundWeapon != null) CurrentWeaponData = foundWeapon;
                }
            }
            else
            {
                CurrentHealth = MaxHealth;
                CurrentStamina = MaxStamina;
                CurrentMagazines = MaxMagazines;
                if (CurrentWeaponData != null) CurrentAmmo = CurrentWeaponData.MaxAmmo;
            }

            NotifyUI();
        }

        private void Update()
        {
            RegenStamina();
        }

        public void NotifyUI() => OnResourcesChanged?.Invoke();

        public void TakeDamage(int amount)  // from IDamageable
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            NotifyUI();
            _playerSFX.TakeDamage();

            if (CurrentHealth <= 0) OnPlayerDeath?.Invoke();
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
                _playerSFX.Shoot(CurrentWeaponData.ShootSound, CurrentWeaponData.ShootVolume);
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
                _playerSFX.Reload();
            }
        }

        private IEnumerator ReloadCoroutine()
        {
            IsReloading = true;
            _animator.SetTrigger("Reload");
            yield return new WaitForSeconds(CurrentWeaponData.ReloadTime);

            CurrentMagazines--;
            CurrentAmmo = CurrentWeaponData.MaxAmmo;
            NotifyUI();
            IsReloading = false;
        }

        public void RegenHealth(int amount)
        {
            if (GameManager.Instance.CurrentState == GameState.GameOver) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            NotifyUI();
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