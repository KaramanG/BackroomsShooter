using UnityEngine;

namespace BackroomsShooter.Generation
{
    [CreateAssetMenu(fileName = "NewTileData", menuName = "BackroomsShooter/Generation/TileData")]
    public class TileData : ScriptableObject
    {
        public GameObject Prefab;

        [Header("Sockets")]
        public string SocketTop;
        public string SocketBottom;
        public string SocketLeft;
        public string SocketRight;

        [Header("Settings")]
        [Range(1, 100)]
        public int weight = 10;
    }
}