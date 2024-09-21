using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireController : MonoBehaviour
{
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
            PlayerShoot ps = collision.GetComponent<PlayerShoot>();
            ps.RegenerateAmmo();
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.EnterSafeZone();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.ExitSafeZone();
        }
    }

}
