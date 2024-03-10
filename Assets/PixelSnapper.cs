using UnityEngine;
public static class PixelSnapper
{
    private const float PixelsPerUnit = 16f; // Change this to match your game's PPU

    public static Vector2 SnapToPixelGrid(Vector2 position)
    {
        position.x = Mathf.Round(position.x * PixelsPerUnit) / PixelsPerUnit;
        position.y = Mathf.Round(position.y * PixelsPerUnit) / PixelsPerUnit;
        return position;
    }

    public static Vector3 SnapToPixelGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x * PixelsPerUnit) / PixelsPerUnit;
        position.y = Mathf.Round(position.y * PixelsPerUnit) / PixelsPerUnit;
        return position;
    }
}