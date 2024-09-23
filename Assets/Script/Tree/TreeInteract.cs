using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour
{
    public bool interactable;
    public GameObject instructions;
    private void Start()
    {
        instructions.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            instructions.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
        instructions.SetActive(false);
    }
}
