using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GenerateRandomTerrain : MonoBehaviour
{
    [Header("Terrain Details")]
    [SerializeField] public int mapWidth = 500; // (-250, 250)
    [SerializeField] public int mapHeight = 24; // (-12, 12) -12 lowest point, 12 - highest
    [SerializeField] public int padding = 4;
    [SerializeField] public List<Vector2Int> terrain;
    [SerializeField] public int jumpHeight = 12;
    [SerializeField] public int maxHorizontalLength = 10;
    [Header("Terrain Tilemap + Tile")]
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public TileBase tile;
    [Header("For Enemy Spawner")]
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public List<GameObject> lootPrefabs;
    [SerializeField] public LayerMask groundLayer;      // layer of the ground colliders
    public int spawnEveryNCells = 10;  // 1 per 10 tiles
    public float groundClearance = 0.2f;
    [SerializeField] public Transform parent;           // parent for hierarchy... optional
    Vector2Int startPositionInGame;
    public bool enemyCreated;
    private List<int> surfaceHeights = new List<int>();
    private List<Vector2> enemyPositions = new List<Vector2>();
    private List<Vector2> lootPositions = new List<Vector2>();

    void Start()
    {
        RunRandomWalk();
        Vector2Int startPosition = new Vector2Int(0, 0);
        startPositionInGame = startPosition;


        // if (enemyPrefab)
        //     enemyCreated = SpawnEnemies(terrain);
    }

    [ContextMenu("Run Random Walk")]
    public void RunRandomWalk()
    {

        Vector2Int startPosition = new Vector2Int(0, padding);
        startPositionInGame = startPosition;
        terrain = RandomWalk(startPosition);
        DrawCyberGroundTilemap(terrain, tile);

        if (enemyPrefab)
            enemyCreated = SpawnEnemies(terrain);

        if (lootPrefabs.Count > 0)
            SpawnLoot();

    }

    public List<Vector2Int> RandomWalk(Vector2Int startPosition)
    {
        var path = new List<Vector2Int>();
        var currentPosition = startPosition;
        path.Add(currentPosition);

        int minHeight = padding;
        int maxHeight = mapHeight - padding;
        int minWidth = padding;
        int maxWidth = mapWidth - padding;

        int currHorCount = 0;
        int offsetY = 0;
        for (int i = 0; i < mapWidth - (padding * 2); i++)
        {
            if (Random.value < 0.4f && currHorCount <= 0)
            {
                currHorCount = Random.Range(3, 6);
            }

            if (currHorCount-- > 0)
            {
                offsetY = 0;
            }
            else
            {
                offsetY = Random.Range(-1, 2);
            }
            currentPosition += new Vector2Int(1, offsetY);
            currentPosition.x = Mathf.Min(currentPosition.x, maxWidth);
            currentPosition.y = Mathf.Clamp(currentPosition.y, minHeight, maxHeight);
            surfaceHeights.Add(currentPosition.y);
            // Debug.Log($"currentPosition.y: {currentPosition.y}");
            // Debug.Log($"{i} - {surfaceHeights[i]}");
            path.Add(currentPosition);
        }



        return path;
    }

    // public bool SpawnEnemies(List<Vector2Int> path)
    // {
    //     if (path == null || path.Count == 0) return false;
    //     var enemyLeftBoundX = surfaceHeights[startPositionInGame.x] + 10;
    //     // float enemyPatrolRange = GetComponent<Enemy>().patrolRange;
    //     if (enemyLeftBoundX >= surfaceHeights.Count) return false;
    //     for (int i = 0; i < surfaceHeights.Count - 21; i += 20)
    //     {
    //         int curr = i + 1;
    //         int enemySpawnPosX = curr;
    //         int enemySpawnPosY = surfaceHeights[enemySpawnPosX] + 3;
    //         var go = Instantiate(enemyPrefab, new Vector3(enemySpawnPosX, enemySpawnPosY, 0), Quaternion.identity);
    //     }
    //     // var go = Instantiate(enemyPrefab, new Vector3(enemyLeftBoundX, surfaceHeights[enemyLeftBoundX] + 2, 0), Quaternion.identity);
    //     // var enemy = go.GetComponent<Enemy>();
    //     // enemy.Init(enemyStartCell, tilemap, patrolRangeTiles: 6f);

    //     return true;

    //     // for (int i = start; i < path.Count; i d+= step)
    //     // {
    //     //     var cell = path[i];
    //     //     if (!used.Add(cell)) continue; // avoid duplicates if path revisits a cell

    //     //     // cell -> world (center of tile)

    //     //     // find the top of the ground at this X
    //     //     Vector3 rayStart = world + Vector3.up * 20f;
    //     //     RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, 50f, groundLayer);
    //     //     if (hit.collider != null)
    //     //         world.y = hit.point.y + groundClearance;

    //     //     Instantiate(enemyPrefab, world, Quaternion.identity, parent);
    //     // }
    // }

    public bool SpawnEnemies(List<Vector2Int> path)
    {
        if (path == null || path.Count == 0) return false;

        // Cache prefab collider bounds once
        var prefabCol = enemyPrefab.GetComponentInChildren<Collider2D>();
        if (!prefabCol) { Debug.LogError("Enemy prefab missing Collider2D"); return false; }
        // We can’t use prefabCol.bounds directly (it’s not in scene). Use size from the collider itself:
        Vector2 prefabSize = GetColliderApproxSize(prefabCol);
        Vector2 spawnCheckSize = prefabSize * 0.95f; // a hair smaller to avoid false positives
        float halfH = prefabSize.y * 0.5f;

        // choose chunks along X as I do now
        // for (int i = 10; i < 21; i += 20)
        for (int i = 10; i < surfaceHeights.Count - 21; i += 20)
        {
            int x = Random.Range(i + 1, i + 20);

            // first, I find the ground top at this X by raycasting DOWN from way above
            float castTopY = 999f; // or my  map’s maxY + margin
            var hit = Physics2D.Raycast(new Vector2(x, castTopY), Vector2.down, Mathf.Infinity, groundLayer);
            if (!hit) continue; // no ground under this x, skip

            // then I place enemy so its feet sit on the ground (hit.point is surface)
            Vector3 spawnPos = new Vector3(x, hit.point.y + halfH + 0.01f, 0f);
            if (enemyPositions.Contains(spawnPos) || lootPositions.Contains(spawnPos))
                continue;
            enemyPositions.Add(spawnPos);
            // ensure no overlap with ground at the target position (full body clearance)
            bool blocked = Physics2D.OverlapBox(spawnPos, spawnCheckSize, 0f, groundLayer);
            if (blocked) continue; // inside a wall/overhang -> skip

            // I ensure there’s ground support across the feet (avoid spawning on 1-pixel ledge)
            float halfW = prefabSize.x * 0.5f;
            var leftFoot = Physics2D.Raycast(new Vector2(x - halfW * 0.8f, hit.point.y + 0.05f), Vector2.down, 0.2f, groundLayer);
            var rightFoot = Physics2D.Raycast(new Vector2(x + halfW * 0.8f, hit.point.y + 0.05f), Vector2.down, 0.2f, groundLayer);
            if (!leftFoot || !rightFoot) continue; // not enough platform width

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

        return true;
    }


    public void SpawnLoot()
    {
        for (int i = 20; i < surfaceHeights.Count - 40; i += 40)
        {
            var lootPrefab = lootPrefabs[Random.Range(0, lootPrefabs.Count)];
            var posX = Random.Range(i, Mathf.Min(i + 40, surfaceHeights.Count));
            var spawnPos = new Vector2(posX, surfaceHeights[posX] + 2);
            if (lootPositions.Contains(spawnPos) || enemyPositions.Contains(spawnPos))
                continue;
            var lootGameobject = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
        }
    }

    static Vector2 GetColliderApproxSize(Collider2D col)
    {
        switch (col)
        {
            case BoxCollider2D b: return b.size;
            case CapsuleCollider2D c: return new Vector2(c.size.x, c.size.y);
            case CircleCollider2D ci: return new Vector2(ci.radius * 2f, ci.radius * 2f);
            case PolygonCollider2D p:
                {
                    var bounds = p.bounds; // safe even in prefab
                    return bounds.size;
                }
            default:
                return col.bounds.size;    // fallback
        }
    }

    public void DrawCyberGroundTilemap(List<Vector2Int> positions, TileBase tile)
    {
        if (!tilemap || !tile) return;
        tilemap.ClearAllTiles();
        foreach (Vector2Int pos in positions)
        {
            tilemap.SetTile((Vector3Int)pos, tile);
        }
    }
}



