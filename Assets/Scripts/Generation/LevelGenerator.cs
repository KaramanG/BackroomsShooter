using System.Collections.Generic;
using UnityEngine;

namespace BackroomsShooter.Generation
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Grid settings")]
        public int GridSizeX = 10;
        public int GridSizeY = 10;
        public float TileSize = 10f;

        [Header("Data")]
        public List<TileData> TilePool;

        private WFC_Cell[,] _grid;
        private List<TileVariant> _allVariants;

        private void Start()
        {
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            InitializeVariants();
            InitializeGrid();

            int safetyBreak = 0;
            int maxIterations = GridSizeX * GridSizeY * 5;

            while (HasUncollapsedCells() && safetyBreak < maxIterations)
            {
                safetyBreak++;
                if (!IterateWFC())
                {
                    Debug.LogWarning("Dead end! Trying again...");
                    GenerateLevel();
                    return;
                }
            }

            if (safetyBreak >= maxIterations)
            {
                Debug.LogError("Reached limit for generation amount!");
                return;
            }

            SpawnLevel();
        }

        private void InitializeVariants()
        {
            _allVariants = new List<TileVariant>();
            foreach (var data in TilePool)
            {
                for (int i = 0; i < 4; i++)
                {
                    _allVariants.Add(new TileVariant(data, i));
                }
            }
        }

        private void InitializeGrid()
        {
            _grid = new WFC_Cell[GridSizeX, GridSizeY];
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y <  GridSizeY; y++)
                {
                    _grid[x, y] = new WFC_Cell(_allVariants);
                }
            }
        }

        private bool HasUncollapsedCells()
        {
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (!_grid[x, y].IsCollapsed) return true;
                }
            }
            return false;
        }

        private bool IterateWFC()
        {
            var coords = FindLowestEntropyCell();
            if (coords == new Vector2Int(-1, -1)) return true;

            _grid[coords.x, coords.y].Collapse();

            return PropagateConstraints(coords.x, coords.y);
        }

        private Vector2Int FindLowestEntropyCell()
        {
            int minEntropy = int.MaxValue;
            Vector2Int bestCoord = new Vector2Int(-1, -1);

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (_grid[x, y].IsCollapsed) continue;

                    int entropy = _grid[x, y].PossibleVariants.Count;
                    if (entropy < minEntropy || (entropy == minEntropy && Random.value > 0.8f))
                    {
                        minEntropy = entropy;
                        bestCoord = new Vector2Int(x, y);
                    }
                }
            }

            return bestCoord;
        }

        private bool PropagateConstraints(int x, int y)
        {
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(new Vector2Int(x, y));

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Pop();

                Vector2Int[] neighbors = {
                    new Vector2Int(current.x, current.y + 1),   // UP
                    new Vector2Int(current.x, current.y - 1),   // DOWN
                    new Vector2Int(current.x - 1, current.y),   // LEFT
                    new Vector2Int(current.x + 1, current.y)    // RIGHT
                };

                for (int i = 0; i < 4; i++)
                {
                    Vector2Int next = neighbors[i];

                    if (next.x < 0 || next.x >= GridSizeX || next.y < 0 || next.y >= GridSizeY) continue;
                    if (_grid[next.x, next.y].IsCollapsed) continue;

                    int removedCount = _grid[next.x, next.y].Constrain(i, _grid[current.x, current.y].PossibleVariants);

                    if (removedCount > 0)
                    {
                        if (_grid[next.x, next.y].PossibleVariants.Count == 0) return false;
                        stack.Push(next);
                    }
                }
            }

            return true;
        }

        private void SpawnLevel()
        {
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    var cell = _grid[x, y];
                    if (!cell.IsCollapsed || cell.ChosenVariant.Data == null || cell.ChosenVariant.Data.Prefab == null) continue;

                    Vector3 pos = new Vector3(x * TileSize, 0, y * TileSize);
                    GameObject cellObject = Instantiate(cell.ChosenVariant.Data.Prefab, pos, Quaternion.identity, transform);

                    cellObject.transform.Rotate(0, cell.ChosenVariant.RotationIndex * 90, 0);
                }
            }
        }

    }
}