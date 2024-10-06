using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform player;
    public float yOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 2, -5);
    }
}
