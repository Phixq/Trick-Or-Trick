using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset; // Offset of the camera relative to the player

    public float minX, maxX; // The horizontal boundaries
    public float minY, maxY; // The vertical boundaries

    // LateUpdate is called after all other updates
    void LateUpdate()
    {
        if (player != null)
        {
            // Get the desired position of the camera
            Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);

            // Clamp the camera's X and Y position within the boundaries
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // Set the camera's position
            transform.position = new Vector3(clampedX, clampedY, desiredPosition.z);
        }
    }
}