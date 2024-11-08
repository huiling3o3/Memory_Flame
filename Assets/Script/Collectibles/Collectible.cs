using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] protected Vector2 initialPosition;

    void Awake()
    {
        //store their original position
        initialPosition = transform.position;
    }

    public void Init()
    {
        //reset original posiion
        transform.position = initialPosition;
    }

    protected virtual void Collect()
    {
        // to be overridden by subclasses if necessary
        Pickup.instance.RemoveCollectible(this);
        //Destroy(gameObject);  // destroy the collectible after collection
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }
}


