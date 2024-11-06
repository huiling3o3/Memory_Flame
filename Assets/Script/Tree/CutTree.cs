using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CutTree: DropBranchHandler, IInteractReciever
{
    // Timer variables for cutting the tree
    [Header("Tree System")]
    public bool isCutting = false;
    public bool interactable = false;
    public float holdTime = 0f;
    public float requiredHoldTime = 3f; // Time required to hold down mouse to cut the tree

    // UI Variables
    [Header("To Assign")]
    public Image fillCircle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] GameObject instructions;
    [SerializeField] private List<Sprite> treeSprites; // List of fire sprites
    SpriteRenderer treeSpriteRenderer;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        treeSpriteRenderer = GetComponent<SpriteRenderer>();
        instructions.SetActive(false);
    }

    public void Init()
    {
        //reset the tree appearance
        ResetCutting();
        instructions.SetActive(false);
    }

    private void Update()
    {
        // If the player is holding the mouse button down and the tree is interactable
        //if (isCutting && interactable)
        //{
        //    holdTime += Time.deltaTime; // Increment the hold time
        //    //update the loading circle
        //    fillCircle.fillAmount = holdTime / requiredHoldTime;

        //    // Calculate the current tree health percentage
        //    float healthPercentage = holdTime / requiredHoldTime;

        //    //update the apperance
        //    // Determine which sprite to display based on the health percentage
        //    int spriteIndex = Mathf.FloorToInt(healthPercentage * (treeSprites.Count - 1));

        //    // Clamp the index to ensure it's within the bounds of the list
        //    spriteIndex = Mathf.Clamp(spriteIndex, 0, treeSprites.Count - 1);

        //    // Update the sprite renderer with the selected sprite
        //    treeSpriteRenderer.sprite = treeSprites[spriteIndex];

        //    // If the hold time reaches the required time, cut the tree
        //    if (holdTime >= requiredHoldTime)
        //    {
        //        cutTree();
        //        ResetCutting(); // Reset the cutting process after cutting the tree
        //    }
        //}
        //else
        //{
        //    //Set the fill to 0
        //    fillCircle.fillAmount = 0f;
        //}

        if (Input.GetMouseButtonUp(0))
        {
            StopInteract();
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
        //reset the tree appearance
        treeSpriteRenderer.sprite = treeSprites[0];
    }

    private void cutTree()
    {
        //Spawn the branches
        DropBranches();
        //stop audio
        audioSource.Pause();
        //destroy game object
        gameObject.SetActive(false);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            instructions.SetActive(true);

            //resume audio
            if (isCutting)
            {
                audioSource.Play();
            }

            Game.GetGameController().SetTreeInteractReciever(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = false;
            instructions.SetActive(false);
            isCutting = false;

            if (isCutting)
            {
                audioSource.Pause();
            }

            if (!Game.GetGameController().isPaused)
            {
                //Set the interactable object to playershoot
                Game.GetGameController().SetPlayerShootInteractReciever();
            }
        }        
    }

    //interact handling
    public void HoldInteract()
    {
        //Debug.Log("Hold interact");
        if ((interactable && isCutting) || !Game.GetGameController().isPaused || !Game.GetGameController().isGameOver)
        {
            
            holdTime += Time.deltaTime; // Increment the hold time
            //update the loading circle
            fillCircle.fillAmount = holdTime / requiredHoldTime;

            // Calculate the current tree health percentage
            float healthPercentage = holdTime / requiredHoldTime;

            //update the apperance
            // Determine which sprite to display based on the health percentage
            int spriteIndex = Mathf.FloorToInt(healthPercentage * (treeSprites.Count - 1));

            // Clamp the index to ensure it's within the bounds of the list
            spriteIndex = Mathf.Clamp(spriteIndex, 0, treeSprites.Count - 1);

            // Update the sprite renderer with the selected sprite
            treeSpriteRenderer.sprite = treeSprites[spriteIndex];

            // If the hold time reaches the required time, cut the tree
            if (holdTime >= requiredHoldTime)
            {
                cutTree();
                ResetCutting(); // Reset the cutting process after cutting the tree
            }
        }
        else
        {
            ResetCutting();
        }
    }
    public void StopInteract()
    {
        //Stop interacting if the player did not hold the left mouse input within the give time
        //Debug.Log("Stop interact");
        ResetCutting();
    }

    public void StartInteract()
    {
        if (interactable || !Game.GetGameController().isPaused || !Game.GetGameController().isGameOver)
        {
            if (!isCutting)
            {
                //SoundManager.PlaySound(SoundType.CUTTREE);
                SoundManager.PlaySound(SoundType.CUTTREE, audioSource, 1f);
                isCutting = true;
            }           
        }
        
    }
}
