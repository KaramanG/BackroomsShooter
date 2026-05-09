using UnityEngine;
using System;

namespace BackroomsShooter.Core
{
    public static class NoiseManager
    {
        public static event Action<Vector3, float> OnNoiseCreated;

        public static void MakeNoise(Vector3 position, float radius)
        {
            OnNoiseCreated?.Invoke(position, radius);
        }
    }
}