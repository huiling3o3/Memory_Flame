using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //References
    protected Rigidbody2D rb;
    [SerializeField] protected float bulletSpeed= 15f;
    [SerializeField] protected float damagePower = 10f;
    [SerializeField] protected float destroyTime= 3f;
    [SerializeField] protected LayerMask whatDestoryBullet;
    [SerializeField] protected GameObject impactEffect;

    // Start is called before the first frame update
    private void Awake()
    {
        //set up the rigidbody
        rb = GetComponent<Rigidbody2D>();
        //play audio when bullet is spawned
        SoundManager.PlaySound(SoundType.ENEMY_SHOOT);
    }

    public virtual void InIt(Vector2 shootDirection)
    {
        
        //set the bullet to move in a straight velocity
        rb.velocity = transform.right * bulletSpeed;
        //destroy the bullet after a certain timing
        SetDestroyTime();
    }

    protected void SetDestroyTime()
    {
        //set the bullet to move in a straight velocity
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, destroyTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Check whether the gameobject(collision) is within the whatDestoryBullet layer mask
        //Make sure the object within hitting the bullet has colliders !important
        if ((whatDestoryBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            //spawn particles
            GameObject Impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(Impact, 0.3f);

            //play sound FX

            //screen shake

            //Damage Enemy
            if (collision.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                enemy.TakeDamage(damagePower);
            }

            //Destory Bullet
            Destroy(gameObject);
        }
    }
}
