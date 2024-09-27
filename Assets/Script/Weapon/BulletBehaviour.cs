using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //References
    private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed= 15f;
    [SerializeField] private float damagePower = 10f;
    [SerializeField] private float destroyTime= 3f;
    [SerializeField] private LayerMask whatDestoryBullet;
    [SerializeField] private GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        //set up the rigidbody
        rb = GetComponent<Rigidbody2D>();
        //set the bullet to move in a straight velocity
        rb.velocity = transform.right * bulletSpeed;
        //destroy the bullet after a certain timing
        SetDestroyTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

            //Damage Enemy
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy");
                EnemyController enemy = collision.GetComponent<EnemyController>();
                enemy.TakeDamage(damagePower);
            }

            //Destory Bullet
            Destroy(gameObject);
        }
    }
}
