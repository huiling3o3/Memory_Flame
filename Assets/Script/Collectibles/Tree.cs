using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tree : MonoBehaviour
{
    public bool interactable;
    public GameObject stickPrefab;
    public Vector3 stickSpawnOffset;
    public int health;

    // Timer variables for cutting the tree
    public bool isCutting = false;
    public float holdTime = 0f;
    public float requiredHoldTime = 3f; // Time required to hold down mouse to cut the tree
    void Awake()
    {
        interactable = false;
    }

    private void Update()
    {
        // If the player is holding the mouse button down and the tree is interactable
        if (isCutting && interactable)
        {
            holdTime += Time.deltaTime; // Increment the hold time

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
        Vector3 spawnPosition = transform.position + stickSpawnOffset;
        GameObject stickObj = Instantiate(stickPrefab, spawnPosition, transform.rotation);
        Destroy(gameObject);

        //health--;
        //if (health <= 0)
        //{
        //    //Spawn the stick obj and set its value
        //    Vector3 spawnPosition = transform.position + stickSpawnOffset;
        //    GameObject stickObj = Instantiate(stickPrefab, spawnPosition, transform.rotation);
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && interactable == true)
        {
            isCutting = true; // Player has started holding down the mouse button
        }
    }
}
