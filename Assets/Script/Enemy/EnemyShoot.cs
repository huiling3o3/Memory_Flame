using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //reference to the bullet objects to spawn the bullets
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    private float timer;
    [SerializeField] bool canAttack = false;
    [SerializeField] GameObject Player;
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

            Vector2 shootDirection = (Player.transform.position - SpawnBullet.transform.position).normalized;

            float rot = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            SpawnBullet.transform.rotation = Quaternion.Euler(0, 0, rot);
            // Set the bullet to fly
            SpawnBullet.GetComponent<IceBullet>().InIt(shootDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { 
            canAttack = true;
            Player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canAttack = false;
            Player = null;
        }
    }
}
