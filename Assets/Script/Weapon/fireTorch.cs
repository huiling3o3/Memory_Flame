using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTorch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Collect()
    {
        GameController controller = Game.GetGameController();
        if (controller != null)
        {
            controller.CollectFireTorch();       
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
