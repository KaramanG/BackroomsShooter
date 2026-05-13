using BackroomsShooter.Player;
using NUnit.Framework;
using UnityEngine;

namespace BackroomsShooter.Core
{
    public enum LootType { Magazine, Weapon }

    public class Pickup : MonoBehaviour
    {
        public LootType Type;
        public WeaponData WeaponInfo;

        private void OnTriggerEnter(Collider foreign)
        {
            if (foreign.CompareTag("Player"))
            {
                var res = foreign.GetComponent<PlayerResources>();

                switch (Type)
                {
                    case LootType.Magazine:
                        res.AddMagazine();
                        break;

                    case LootType.Weapon:
                        if (res.CurrentWeaponData != null)
                        {
                            Vector3 dropPos = foreign.transform.position + foreign.transform.forward * 3f;
                            Instantiate(res.CurrentWeaponData.PickupModelPrefab, dropPos, Quaternion.Euler(new Vector3(0, 90, 90)));
                        }

                        res.CurrentWeaponData = WeaponInfo;
                        res.CurrentAmmo = 0;
                        res.NotifyUI();
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}