using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GroundSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] groundTiles;
    public LayerMask whatIsGround;
    public float detectionDistance = 1f;
    public Vector3Int gridSize = new Vector3Int(1, 1, 0);
    public float initialY;
    private bool isSpawning = false;

    // New variables
    private int platformLength = 0;
    public int platformSpacing = 3;
    public int minPlatformHeight = 0;
    public int maxPlatformHeight = 5;
    public float coinSpawnChance = 0.5f;
    public int coinHeightVariance = 2;

    public GameObject coinPrefab;
    public int maxCoinsPerPlatform = 3;

    private int lastIndex = 0;

    void Start()
    {
        initialY = transform.position.y;
        platformLength = groundTiles.Length;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);

        Vector3Int gridPosition = new Vector3Int(Mathf.RoundToInt(transform.position.x / gridSize.x), Mathf.RoundToInt(transform.position.y / gridSize.y), 0);
        List<Tile> currentTiles = GetTiles(gridPosition, platformLength);

        List<Tile> previousTiles = GetTiles(gridPosition - new Vector3Int(platformSpacing, 0, 0), platformSpacing);
        List<Tile> nextTiles = GetTiles(gridPosition + new Vector3Int(platformLength, 0, 0), platformSpacing);

        if (currentTiles.All(tile => !IsTileInGroundTiles(tile)) && previousTiles.All(tile => tile == null) && nextTiles.All(tile => tile == null) && !isSpawning)
        {
            int heightVariance = Random.Range(minPlatformHeight, maxPlatformHeight + 1);

            bool isSpaceOccupied = Enumerable.Range(0, platformLength).Any(i => tilemap.HasTile(gridPosition + new Vector3Int(i, 0, 0) + Vector3Int.up) || tilemap.HasTile(gridPosition + new Vector3Int(i, 0, 0) + Vector3Int.down));

            if (!isSpaceOccupied)
            {
                foreach (Vector3Int position in Enumerable.Range(0, platformLength).Select(i => gridPosition + new Vector3Int(i, 0, 0)))
                {
                    StartCoroutine(SpawnGround(position, heightVariance));
                }
            }
        }
    }

    List<Tile> GetTiles(Vector3Int gridPosition, int platformWidth)
    {
        List<Tile> tiles = new List<Tile>();

        for (int i = 0; i < platformWidth; i++)
        {
            Tile tile = tilemap.GetTile<Tile>(gridPosition + new Vector3Int(i, 0, 0));
            tiles.Add(tile);
        }

        return tiles;
    }

    IEnumerator SpawnGround(Vector3Int gridPosition, int heightVariance)
    {
        isSpawning = true;
        yield return new WaitForSeconds(0.1f);

        gridPosition.y += heightVariance;

        Tile selectedTile = groundTiles[lastIndex];
        tilemap.SetTile(gridPosition, selectedTile);

        lastIndex = (lastIndex + 1) % groundTiles.Length;

        if (Random.Range(0f, 1f) < coinSpawnChance)
        {
            Vector3Int aboveGridPosition = gridPosition + new Vector3Int(0, 1 + Random.Range(0, coinHeightVariance + 1), 0);
            
            // Check if there is already a tile at the position
            if (tilemap.GetTile(aboveGridPosition) == null)
            {
                Instantiate(coinPrefab, tilemap.GetCellCenterWorld(aboveGridPosition), Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(0.1f);
        isSpawning = false;
    }

    public bool IsTileInGroundTiles(Tile tile)
    {
        foreach (Tile groundTile in groundTiles)
        {
            if (groundTile == tile)
            {
                return true;
            }
        }
        return false;
    }
}