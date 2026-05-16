using UnityEngine;

namespace BackroomsShooter.Core
{
    [System.Serializable]
    public class SaveData
    {
        public int PlayerHP;
        public int PlayerAmmo;
        public int PlayerMagazines;
        public string WeaponName;

        public float PlayerX, PlayerZ;

        public int LevelSeed;
        public int LevelIndex;

        public float BossX, BossZ;
    }
}
