using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tree : MonoBehaviour
{
    public GameObject stickPrefab;
    //public Vector3 stickSpawnOffset;
    //public int health;
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
        if (isCutting && ti.interactable)
        {
            holdTime += Time.deltaTime; // Increment the hold time
            fillCircle.fillAmount = holdTime / requiredHoldTime;
            // If the hold time reaches the required time, cut the tree
            if (holdTime >= requiredHoldTime)
            {
                cutTree();
                ResetCutting(); // Reset the cutting process after cutting the tree
            }
        }

        // If the player releases the mouse button, reset the hold timer
        if (Input.GetMouseButtonUp(0))
        {
            ResetCutting();
        }
    }

    private void ResetCutting()
    {
        // Reset the cutting process
        isCutting = false;
        holdTime = 0f;
    }

    public void cutTree()
    {
        //Spawn the stick obj and set its value
        //Vector3 spawnPosition = transform.position + stickSpawnOffset;
        Instantiate(stickPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && ti.interactable == true)
        {
            isCutting = true; // Player has started holding down the mouse button
        }
    }
}
