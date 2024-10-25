using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamge : MonoBehaviour
{
    //Trigger used to deal damage to player 
    EnemyController ec;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerHealth = collision.GetComponent<PlayerController>();
            if (playerHealth != null)
            {
                //Set the interactable object to tree
                //ec = GetComponentInParent<EnemyController>();
                //playerHealth.TakeDamage(ec.atk);
            }
        }

    }
}
