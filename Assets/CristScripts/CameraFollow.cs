using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign player object here
    public Vector3 offset;   
    
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }  //sets it to le player position
    }
}

