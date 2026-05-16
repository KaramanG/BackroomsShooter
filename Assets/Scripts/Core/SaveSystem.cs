using System.IO;
using UnityEngine;

namespace BackroomsShooter.Core
{
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        public static SaveData CachedData { get; private set; }

        public static void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static SaveData Load()
        {
            if (!File.Exists(SavePath)) return null;

            string json = File.ReadAllText(SavePath);
            CachedData = JsonUtility.FromJson<SaveData>(json);
            return CachedData;
        }

        public static bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public static void ClearSave()
        {
            CachedData = null;
        }
    }
}
