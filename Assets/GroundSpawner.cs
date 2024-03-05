using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GroundSpawner : MonoBehaviour
{
    public Tilemap tilemap; // The tilemap to paint on
    public Tile[] groundTiles; // The array of tiles to paint
    public LayerMask whatIsGround;
    public float detectionDistance = 1f; // Adjust this value as needed
    public Vector3Int gridSize = new Vector3Int(1, 1, 0); // Size of the grid cells
    public float initialY;
    private bool isSpawning = false;
    public int distanceBetweenPlatforms = 0;
    private int lastIndex = 0;

    void Start()
    {
        // Store the initial Y position
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Always set the Y position to the initial Y position
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);

        Vector3Int gridPosition = new Vector3Int(Mathf.RoundToInt(transform.position.x / gridSize.x), Mathf.RoundToInt(transform.position.y / gridSize.y), 0);
        List<Tile> currentTiles = GetTiles(gridPosition, groundTiles.Length);

        // Check if the tiles in the range of distanceBetweenPlatforms before the current platform are all empty
        List<Tile> previousTiles = GetTiles(gridPosition - new Vector3Int(distanceBetweenPlatforms, 0, 0), distanceBetweenPlatforms);

        // Check if the tiles in the range of distanceBetweenPlatforms after the current platform are all empty
        List<Tile> nextTiles = GetTiles(gridPosition + new Vector3Int(groundTiles.Length, 0, 0), distanceBetweenPlatforms);

        if (currentTiles.All(tile => !IsTileInGroundTiles(tile)) && previousTiles.All(tile => tile == null) && nextTiles.All(tile => tile == null) && !isSpawning)
        {
            foreach (Vector3Int position in Enumerable.Range(0, groundTiles.Length).Select(i => gridPosition + new Vector3Int(i, 0, 0)))
            {
                StartCoroutine(SpawnGround(position));
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

    IEnumerator SpawnGround(Vector3Int gridPosition)
    {
        isSpawning = true;
        // Wait for a few frames
        yield return new WaitForSeconds(0.1f);

        // Use the tile at the current index
        Tile selectedTile = groundTiles[lastIndex];
        // Paint the tile at the grid position
        tilemap.SetTile(gridPosition, selectedTile);

        // Increment the index, and reset it if it's out of bounds
        lastIndex = (lastIndex + 1) % groundTiles.Length;

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