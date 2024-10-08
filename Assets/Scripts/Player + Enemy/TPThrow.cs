using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPThrow : MonoBehaviour
{
    public Rigidbody2D projectile;

    void Update()
    {
        // Ctrl was pressed, launch a projectile
        if (Input.GetMouseButtonDown(0))
        {
            // Instantiate the projectile at the position and rotation of this transform
            Rigidbody2D clone;
            clone = Instantiate(projectile, transform.position, transform.rotation);

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            clone.velocity = transform.TransformDirection(Vector3.forward * 10);
        }
    }
}