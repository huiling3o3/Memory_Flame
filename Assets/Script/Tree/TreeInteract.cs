using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour
{
    public bool interactable;
    CutTree tr;
    [SerializeField] GameObject instructions;
    private void Start()
    {
        instructions.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Game.GetGameController().isPaused)
        {
            interactable = true;
            instructions.SetActive(true);

            //Set the interactable object to tree
            tr = GetComponentInParent<CutTree>();
            Game.GetGameController().SetTreeInteractReciever(tr);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
        instructions.SetActive(false);
        if (!Game.GetGameController().isPaused)
        {
            //Set the interactable object to playershoot
            Game.GetGameController().SetPlayerShootInteractReciever();
        }
        
    }
}
