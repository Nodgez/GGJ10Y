using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var boid = other.GetComponent<Boid>();
        boid.Collect();
    }
}
