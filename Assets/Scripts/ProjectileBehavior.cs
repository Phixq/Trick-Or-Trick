using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 4.5f;
    public float lifetime = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject, lifetime);
    }
}
