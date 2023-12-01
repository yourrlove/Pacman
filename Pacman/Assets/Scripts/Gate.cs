using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform Teleport;

    // Teleport ghost to another Gate when thay move out of the map from a Gate
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 newPosition = Teleport.position;
        newPosition.z = collision.transform.position.z;
        collision.transform.position = newPosition;

    }
}
