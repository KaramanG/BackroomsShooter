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
            ChosenVariant = PossibleVariants[UnityEngine.Random.Range(0, PossibleVariants.Count)];   // random.range for now
            PossibleVariants = new List<TileVariant> { ChosenVariant };
            IsCollapsed = true;
        }

        public int Constrain(int direction, List<TileVariant> neighborVariants)
        {
            int originalCount = PossibleVariants.Count;

            HashSet<string> validSockets = new HashSet<string>();
            foreach (var v in neighborVariants)
            {
                if (direction == 0) validSockets.Add(v.UP);
                if (direction == 1) validSockets.Add(v.DOWN);
                if (direction == 2) validSockets.Add(v.LEFT);
                if (direction == 3) validSockets.Add(v.RIGHT);
            }

            PossibleVariants.RemoveAll(v =>
            {
                if (direction == 0) return !validSockets.Contains(v.DOWN);
                if (direction == 1) return !validSockets.Contains(v.UP);
                if (direction == 2) return !validSockets.Contains(v.RIGHT);
                if (direction == 3) return !validSockets.Contains(v.LEFT);
                return false;
            });

            return originalCount - PossibleVariants.Count;
        }

    }
}