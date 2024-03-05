using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Transform player;
    public float baseScrollSpeed = 0.02f; // This is the base scroll speed that is always applied
    public float playerSpeedFactor = 0.02f; // This factor determines how much the player's speed affects the scroll speed
    private float previousPlayerPositionX;
    [SerializeField] Renderer backgroundRenderer;
    public bool scrollRight = true; // This boolean determines the direction of the scroll

    void Start()
    {
        previousPlayerPositionX = player.position.x;
    }

    void Update()
    {
        float playerSpeed = (player.position.x - previousPlayerPositionX) / Time.deltaTime;
        float totalScrollSpeed;

        if ((scrollRight && playerSpeed < 0) || (!scrollRight && playerSpeed > 0)) // player is moving opposite to scroll direction
        {
            totalScrollSpeed = baseScrollSpeed - Mathf.Abs(playerSpeed) * playerSpeedFactor;
        }
        else // player is moving in the same direction as scroll or not moving
        {
            totalScrollSpeed = baseScrollSpeed + Mathf.Abs(playerSpeed) * playerSpeedFactor;
        }

        if (scrollRight)
        {
            backgroundRenderer.material.mainTextureOffset += new Vector2(totalScrollSpeed * Time.deltaTime, 0);
        }
        else
        {
            backgroundRenderer.material.mainTextureOffset -= new Vector2(totalScrollSpeed * Time.deltaTime, 0);
        }
        
        previousPlayerPositionX = player.position.x;
    }
}