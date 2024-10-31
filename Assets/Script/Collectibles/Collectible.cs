using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    protected virtual void Collect()
    {
        // to be overridden by subclasses if necessary
        Pickup.instance.RemoveCollectible(this);
        Destroy(gameObject);  // destroy the collectible after collection
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }
}


