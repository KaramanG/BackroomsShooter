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
                var res = foreign.GetComponent<Player.PlayerResources>();

                if (Type == LootType.Magazine) res.AddMagazine();

                else if (Type == LootType.Weapon)
                {
                    res.CurrentWeaponData = WeaponInfo;
                    res.CurrentAmmo = WeaponInfo.MaxAmmo;
                    res.NotifyUI();
                }

                Destroy(gameObject);
            }
        }
    }
}