using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;

namespace BackroomsShooter.Generation
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Settings")]
        public int Seed = 0;
        public bool UseRandomSeed = false;

        [Header("Grid settings")]
        public int GridSizeX = 25;
        public int GridSizeY = 25;
        public float TileSize = 10f;

        [Header("Visualization (DEV ONLY)")]
        public bool VisualizeGeneration = true;
        public float StepDelay = 0.05f;

        [Header("Data")]
        public List<TileData> TilePool;
        public TileData EmptyFloorTile;

        private WFC_Cell[,] _grid;
        private List<TileVariant> _allVariants;
        private NavMeshSurface _navMesh;

        private int _centerX;
        private int _centerY;

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshSurface>();
        }

        private void Start()
        {
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }

            if (UseRandomSeed) Seed = Random.Range(0, int.MaxValue);
            Random.InitState(Seed);

            var settings = Core.LevelManager.Instance.GetCurrentLevel();
            GridSizeX = settings.GridSize;
            GridSizeY = settings.GridSize;
            TilePool = settings.TilePool;
            EmptyFloorTile = settings.EmptyTile;
            RenderSettings.ambientLight = settings.AmbientColor;

            _centerX = GridSizeX / 2;
            _centerY = GridSizeY / 2;

            InitializeVariants();
            InitializeGrid();

            ForceTileAt(new Vector2Int(_centerX, _centerY), EmptyFloorTile);

            if (VisualizeGeneration) StartCoroutine(GenerationCoroutine());
            else InstantGeneration();
        }

        private void InitializeVariants()
        {
            _allVariants = new List<TileVariant>();
            foreach (var data in TilePool)
            {
                for (int i = 0; i < 4; i++)
                    _allVariants.Add(new TileVariant(data, i));
            }
        }

        private void InitializeGrid()
        {
            _grid = new WFC_Cell[GridSizeX, GridSizeY];
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y <  GridSizeY; y++)
                    _grid[x, y] = new WFC_Cell(_allVariants);
            }
        }

        private void ForceTileAt(Vector2Int pos, TileData data)
        {
            TileVariant variant = new TileVariant(data, 0);
            _grid[pos.x, pos.y].PossibleVariants = new List<TileVariant> { variant };
            _grid[pos.x, pos.y].Collapse();

            SpawnTile(pos.x, pos.y);

            PropagateConstraints(pos.x, pos.y);
        }

        private IEnumerator GenerationCoroutine()
        {
            while (HasUncollapsedCells())
            {
                if (!IterateWFC())
                {
                    GenerateLevel();
                    yield break;
                }
                yield return new WaitForSeconds(StepDelay);
            }
            FinalizeLevel();
        }

        private void InstantGeneration()
        {
            while (HasUncollapsedCells())
            {
                if (!IterateWFC()) { GenerateLevel(); return; }
            }

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                    SpawnTile(x, y);
            }
            
            FinalizeLevel();
        }

        private void FinalizeLevel()
        {
            _navMesh.BuildNavMesh();
            NotifyActorsLevelReady();

            GameObject goal = new GameObject("ExitPoint");
            goal.transform.position = new Vector3((GridSizeX - _centerX) * TileSize, 0, (GridSizeY - _centerY) * TileSize);
            FindFirstObjectByType<UI.Compass>().Target = goal.transform;

            var settings = Core.LevelManager.Instance.GetCurrentLevel();
            Vector3 bossPos = new Vector3((GridSizeX - _centerX) * TileSize, 1, (GridSizeY - _centerY) * TileSize);
            Instantiate(settings.BossPrefab, bossPos, Quaternion.identity);

            Core.GameManager.Instance.ChangeState(Core.GameState.Gameplay);
        }

        private void NotifyActorsLevelReady()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(0, 1.5f, 0);
            }

            foreach (var ai in FindObjectsByType<Enemy.EnemyAI>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                ai.gameObject.SetActive(true);
                var agent = ai.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null) agent.enabled = true;
            }
        }

        private bool IterateWFC()
        {
            Vector2Int coords = FindLowestEntropyCell();
            if (coords == new Vector2Int(-1, -1)) return true;

            _grid[coords.x, coords.y].Collapse();

            if (VisualizeGeneration) SpawnTile(coords.x, coords.y);

            return PropagateConstraints(coords.x, coords.y);
        }

        private void SpawnTile(int x, int y)
        {
            var cell = _grid[x, y];
            if (!cell.IsCollapsed || cell.ChosenVariant.Data.Prefab == null) return;

            Vector3 pos = new Vector3((x - _centerX) * TileSize, 0, (y - _centerY) * TileSize);
            GameObject cellObject = Instantiate(cell.ChosenVariant.Data.Prefab, pos, Quaternion.identity, transform);
            cellObject.transform.Rotate(0, cell.ChosenVariant.RotationIndex * 90f, 0);
        }

        private bool HasUncollapsedCells()
        {
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                    if (!_grid[x, y].IsCollapsed) return true;
            }
            return false;
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

                    if (entropy < minEntropy)
                    {
                        minEntropy = entropy;
                        bestCoord = new Vector2Int(x, y);
                    }
                    else if (entropy == minEntropy && Random.value > 0.5f)
                        bestCoord = new Vector2Int(x, y);
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
                    new Vector2Int(current.x, current.y + 1),
                    new Vector2Int(current.x, current.y - 1),
                    new Vector2Int(current.x - 1, current.y),
                    new Vector2Int(current.x + 1, current.y)
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
    }
}