using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.25f;
    public Vector3 offset;
    private PlayerController player;
    public bool lockY = false;
    public bool flipX = false;
    private PixelPerfectCamera pixelPerfectCamera; // Reference to the PixelPerfectCamera component
    public GameObject touchMovement;


    private void Start()
    {
        player = target.GetComponent<PlayerController>();
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
    }
    void CheckAspectRatio()
    {
        if (Screen.height > Screen.width)
        {
            pixelPerfectCamera.cropFrameY = true;
            touchMovement.SetActive(true);
        }
        else
        {
            pixelPerfectCamera.cropFrameY = false;
            touchMovement.SetActive(false);
        }
        if (Screen.width > Screen.height * 2.5)
        {
            pixelPerfectCamera.cropFrameX = true;
        }
        else
        {
            pixelPerfectCamera.cropFrameX = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckAspectRatio();
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
        
        // Smoothly move the camera towards the desired position
        Vector3 velocity = Vector3.zero;
        float posX = Mathf.SmoothDamp(transform.position.x, desiredPosition.x, ref velocity.x, smoothSpeed);
        float posY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref velocity.y, smoothSpeed);

        // Pixel snapping
        Vector2 snappedPos = PixelSnapper.SnapToPixelGrid(new Vector2(posX, posY));

        // Set the camera's position
        transform.position = new Vector3(snappedPos.x, snappedPos.y, transform.position.z);
    }
}
