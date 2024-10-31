using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractReciever
{
    //void DoInteract(); //Interact controls
    void HoldInteract(); // Begin interaction (press and hold)
    void StopInteract(); // Stop interaction (release hold)
    void StartInteract(); // Handle shooting action (single left-click)
}
