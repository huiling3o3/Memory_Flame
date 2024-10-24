using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //reference to the bullet objects to spawn the bullets
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
     private float timer;
    bool canAttack = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            timer = 0;
            DoShoot();
        }
    }

    public void DoShoot()
    {
        if (!Game.GetGameController().isGameOver && canAttack)
        {
            //spawn bullet
            GameObject SpawnBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);

            Vector2 shootDirection = Game.GetPlayer().transform.position - SpawnBullet.transform.position;

            SpawnBullet.GetComponent<Rigidbody2D>().velocity = shootDirection;
            float rot = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            SpawnBullet.transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { 
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canAttack = false;
        }
    }
}
