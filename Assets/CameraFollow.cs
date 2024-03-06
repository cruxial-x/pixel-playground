using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.25f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private PlayerController player;
    public bool lockY = false;
    public bool flipX = false;

    private void Start()
    {
        player = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(flipX)
        {
            // Check if the player is facing left
            if (player.isFacingLeft)
            {
                // If the player is facing left, make the x value of offset negative
                offset.x = -Mathf.Abs(offset.x);
            }
            else
            {
                // If the player is not facing left, make the x value of offset positive
                offset.x = Mathf.Abs(offset.x);
            }
        }
        
        Vector3 desiredPosition = target.position + offset;
        
        if (lockY)
        {
            // Lock the y value of desiredPosition to the current camera position
            desiredPosition.y = transform.position.y;
        }
        
        transform.position = desiredPosition;
    }
}
