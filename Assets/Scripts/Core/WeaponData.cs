using UnityEngine;

namespace BackroomsShooter.Core
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "BackroomsShooter/Combat/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Settings")]
        public string WeaponName;
        public Sprite WeaponIcon;

        public int Damage = 10;
        public float FireRate = 0.2f;
        public float NoiseRadius = 50f;

        [Header("Resources")]
        public int MaxAmmo = 30;
        public float ReloadTime = 1.5f;

        [Header("Sounds")]
        public AudioClip ShootSound;
        [Range(0f, 1f)]
        public float ShootVolume;

        [Header("Visuals")]
        public GameObject WeaponModelPrefab;
        public GameObject PickupModelPrefab;
    }
}