using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private float pickupRadius = 1f, pickupSpeed = 0.5f;

    private CircleCollider2D circleCollider;

    public List<Collectible> collectibles = new List<Collectible>();
    public static Pickup instance;
    private void Awake()
    {
        if(instance == null) instance = this;
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        circleCollider.radius = pickupRadius;
    }
    private void Update()
    {
        foreach(Collectible col in collectibles)
        {
            if (col == null)
            {
                collectibles.Remove(col);
            }
            else
            {
                col.transform.position = Vector3.MoveTowards(col.transform.position, transform.position, pickupSpeed);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if(collectible != null)
        {
            collectibles.Add(collectible);
        }
    }
    public void RemoveCollectible(Collectible collectible)
    {
        collectibles.Remove(collectible);
    }
}
