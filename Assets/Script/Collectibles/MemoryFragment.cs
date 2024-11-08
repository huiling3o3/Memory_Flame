using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    [SerializeField] MemoryFragType mf;
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

    public void Collect()
    {
        GameController controller = Game.GetGameController();
        if (controller != null)
        {
            controller.AddMemoryFragment(mf); // add to memory fragment count        
        }
        
        //hide the gameobject
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }
}

public enum MemoryFragType
{
    HEADBAND,
    NECKLACE,
    BROKENSWORD
}