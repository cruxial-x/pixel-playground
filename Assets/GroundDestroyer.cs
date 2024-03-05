using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDestroyer : MonoBehaviour
{
    public GroundSpawner groundSpawner;
    private bool isDestroying = false;

    // Update is called once per frame
    void Update()
    {
        // Always set the Y position to the initial Y position
        transform.position = new Vector3(transform.position.x, groundSpawner.initialY, transform.position.z);

        Vector3Int gridPosition = new Vector3Int(Mathf.RoundToInt(transform.position.x / groundSpawner.gridSize.x), Mathf.RoundToInt(transform.position.y / groundSpawner.gridSize.y), 0);
        Tile currentTile = groundSpawner.tilemap.GetTile<Tile>(gridPosition);

        // Get the PlayerController script from the parent object
        PlayerController playerController = GetComponentInParent<PlayerController>();

        // Check if the player is facing left before destroying ground
        if (groundSpawner.IsTileInGroundTiles(currentTile) && !isDestroying)
        {
            StartCoroutine(DestroyGround(gridPosition, playerController.isFacingLeft));
        }
    }

    IEnumerator DestroyGround(Vector3Int gridPosition, bool destroyToLeft)
    {
        isDestroying = true;
        // Wait for a few frames
        yield return new WaitForSeconds(0.1f);

        // Erase the tiles starting from the grid position
        for (int i = 0; i < groundSpawner.groundTiles.Length; i++)
        {
            Vector3Int currentPos = gridPosition + new Vector3Int(destroyToLeft ? -i : i, 0, 0);
            Tile currentTile = groundSpawner.tilemap.GetTile<Tile>(currentPos);
            if (!groundSpawner.IsTileInGroundTiles(currentTile))
            {
                break;
            }
            groundSpawner.tilemap.SetTile(currentPos, null);
        }

        isDestroying = false;
    }
}