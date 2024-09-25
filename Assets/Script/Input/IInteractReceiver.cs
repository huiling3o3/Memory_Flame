using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractReciever
{
    //void DoInteract(); //Interact controls
    void StartInteract(); // Begin interaction (press and hold)
    void StopInteract(); // Stop interaction (release hold)
    void DoShoot(); // Handle shooting action (single left-click)
}
