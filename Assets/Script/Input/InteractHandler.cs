using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    private IInteractReciever activeReceiver;
    private bool isInteracting = false; // Track whether player is holding left-click to interact
    public void SetInteractReceiver(IInteractReciever inputReceiver)
    {
        //set current input receiver (to control 1 thing at a time)
        activeReceiver = inputReceiver;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeReceiver == null) return;

        // Handle left-click (shooting or interacting)
        if (Input.GetMouseButtonDown(1))
        {
            if (isInteracting) return; // Don't shoot if interacting
            activeReceiver.DoInteract(); // Start interaction (press and hold)
            isInteracting = true;
        }

        // If the left-click is held for interaction
        if (Input.GetMouseButton(1) && isInteracting)
        {
            // Continue interaction (if necessary)
        }

        // When left-click is released, stop interaction
        if (Input.GetMouseButtonUp(1) && isInteracting)
        {
            //activeReceiver.StopInteract();
            isInteracting = false;
        }
    }
}