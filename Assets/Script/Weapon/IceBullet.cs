using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletBehaviour
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Check whether the gameobject(collision) is within the whatDestoryBullet layer mask
        //Make sure the object within hitting the bullet has colliders !important
        if ((whatDestoryBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            //spawn particles
            //GameObject Impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            //Destroy(Impact, 0.5f);

            //play sound FX

            //screen shake

            //Damage Player hypothemia
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Hit player");
                PlayerController player = collision.GetComponent<PlayerController>();
                player.TakeFreezeDamage(damagePower);
            }

            //Destory Bullet
            Destroy(gameObject);
        }
    }
}
