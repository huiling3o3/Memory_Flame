using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireController : MonoBehaviour
{
    PlayerShoot ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {            
            ps = collision.GetComponent<PlayerShoot>();
            ps.RegenerateAmmo();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ps = collision.GetComponent<PlayerShoot>();
            ps.ExitSafeZone();
        }
    }
}
