using System.Collections.Generic;

namespace BackroomsShooter.Generation
{
    public class WFC_Cell
    {
        public bool IsCollapsed;
        public TileVariant ChosenVariant;
        public List<TileVariant> PossibleVariants;

        public WFC_Cell(IEnumerable<TileVariant> allVariants)
        {
            PossibleVariants = new List<TileVariant>(allVariants);
        }

        public void Collapse()
        {
            if (PossibleVariants.Count == 0) return;

            float totalWeight = 0;
            foreach (var variant in PossibleVariants)
            {
                totalWeight += variant.Data.Weight;
            }

            float randomNumber = UnityEngine.Random.Range(0, totalWeight);

            float currentWeightSum = 0;
            foreach (var variant in PossibleVariants)
            {
                currentWeightSum += variant.Data.Weight;
                if (randomNumber <= currentWeightSum)
                {
                    ChosenVariant = variant;
                    break;
                }
            }

            if (ChosenVariant.Data == null) ChosenVariant = PossibleVariants[0];

            PossibleVariants = new List<TileVariant> { ChosenVariant };
            IsCollapsed = true;
        }

        public int Constrain(int direction, List<TileVariant> neighborVariants)
        {
            int originalCount = PossibleVariants.Count;
            HashSet<string> validSockets = new HashSet<string>();

            foreach (var v in neighborVariants)
            {
                if (direction == 0) validSockets.Add(v.Sockets[0]);
                if (direction == 1) validSockets.Add(v.Sockets[2]);
                if (direction == 2) validSockets.Add(v.Sockets[3]);
                if (direction == 3) validSockets.Add(v.Sockets[1]);
            }

            PossibleVariants.RemoveAll(v =>
            {
                if (direction == 0) return !validSockets.Contains(v.Sockets[2]);
                if (direction == 1) return !validSockets.Contains(v.Sockets[0]);
                if (direction == 2) return !validSockets.Contains(v.Sockets[1]);
                if (direction == 3) return !validSockets.Contains(v.Sockets[3]);
                return false;
            });

            return originalCount - PossibleVariants.Count;
        }

    }
}