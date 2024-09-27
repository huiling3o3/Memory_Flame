using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour
{
    public bool interactable;
    Tree tr;
    [SerializeField] GameObject instructions;
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
            
            //Set the interactable object to tree
            tr = GetComponentInParent<Tree>();
            Game.GetGameController().SetTreeInteractReciever(tr);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
        instructions.SetActive(false);
        //Set the interactable object to playershoot
        Game.GetGameController().SetPlayerShootInteractReciever();
    }
}
