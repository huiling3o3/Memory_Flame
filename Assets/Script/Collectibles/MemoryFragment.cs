using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : Collectible
{
    protected override void Collect()
    {
        base.Collect(); // to handle object destruction

        GameController controller = Game.GetGameController();
        if (controller != null)
        {
            controller.AddMemoryFragment(); // add to memory fragment count
        }
    }
}
