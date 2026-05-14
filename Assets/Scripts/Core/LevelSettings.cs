using BackroomsShooter.Generation;
using System.Collections.Generic;
using UnityEngine;

namespace BackroomsShooter.Core
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "BackroomsShooter/World/LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        public string LevelName;
        public int GridSize = 25;
        public List<TileData> TilePool;
        public TileData EmptyTile;
        public GameObject BossPrefab;
        public Color AmbientColor = Color.gray;
        public float DifficultyMultiplier = 1.0f;
    }
}