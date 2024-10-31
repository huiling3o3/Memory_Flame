using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CutTree: DropBranchHandler, IInteractReciever
{
    // Timer variables for cutting the tree
    public bool isCutting = false;
    public bool interactable = false;
    public float holdTime = 0f;
    public float requiredHoldTime = 3f; // Time required to hold down mouse to cut the tree

    // UI Variables
    public Image fillCircle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] GameObject instructions;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        instructions.SetActive(false);
    }
    private void Update()
    {
        // If the player is holding the mouse button down and the tree is interactable
        if (isCutting)
        {
            holdTime += Time.deltaTime; // Increment the hold time
            //update the loading circle
            fillCircle.fillAmount = holdTime / requiredHoldTime;
            
            // If the hold time reaches the required time, cut the tree
            if (holdTime >= requiredHoldTime)
            {
                cutTree();
                ResetCutting(); // Reset the cutting process after cutting the tree
            }
        }
        else
        {
            //Set the fill to 0
            fillCircle.fillAmount = 0f;
        }
    }

    // If the player releases the mouse button, reset the hold timer
    private void ResetCutting()
    {
        // Reset the cutting process
        isCutting = false;
        interactable = false; 
        instructions.SetActive(false);
        holdTime = 0f;
        audioSource.Pause();
    }

    private void cutTree()
    {
        //Spawn the branches
        DropBranches();
        //destroy game object
        gameObject.SetActive(false);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            instructions.SetActive(true);

            Game.GetGameController().SetTreeInteractReciever(this);
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

    //interact handling
    public void HoldInteract()
    {
        Debug.Log("Hold interact");
        if (interactable || !Game.GetGameController().isPaused || !Game.GetGameController().isGameOver)
        {            
            isCutting = true;
        }
        
    }
    public void StopInteract()
    {
        //Stop interacting if the player did not hold the left mouse input within the give time
        Debug.Log("Stop interact");
        ResetCutting();
    }

    public void StartInteract()
    {
        if (!Game.GetGameController().isPaused || !Game.GetGameController().isGameOver)
        {
            SoundManager.PlaySound(SoundType.CUTTREE, audioSource, 1f);
        }
        
    }
}
