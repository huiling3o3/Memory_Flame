using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tree : DropBranchHandler, IInteractReciever
{
    TreeInteract ti;

    // Timer variables for cutting the tree
    public bool isCutting = false;
    public float holdTime = 0f;
    public float requiredHoldTime = 3f; // Time required to hold down mouse to cut the tree

    // UI Variables
    public Image fillCircle;
    private void Start()
    {
        ti = GetComponentInChildren<TreeInteract>();
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
        holdTime = 0f;
    }

    public void cutTree()
    {
        //Spawn the branches
        DropBranches();
        Destroy(gameObject);
    }

    //interact handling
    public void StartInteract()
    {
        Debug.Log("start interact");
        if (!EventSystem.current.IsPointerOverGameObject() && ti.interactable == true)
        {
            isCutting = true; // Player has started holding down the mouse button           
        }
    }
    public void StopInteract()
    {
        //Stop interacting if the player did not hold the left mouse input within the give time
        Debug.Log("Stop interact");
        ResetCutting();
    }

    public void DoShoot()
    { 
        //do nothing
    }
}
