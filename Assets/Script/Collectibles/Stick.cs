using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : Collectible
{
    protected override void Collect()
    {
        base.Collect(); // to handle object destruction
        // CollectibleManager manager = FindObjectOfType<CollectibleManager>();
        GameController controller = GetComponent<GameController>();
        if (controller != null)
        {
            controller.AddStick(); // add to memory fragment count
        }
    }
}
