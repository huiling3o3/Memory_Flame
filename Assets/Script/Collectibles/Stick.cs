using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : Collectible
{
    protected override void Collect()
    {
        base.Collect(); // to handle object destruction

        GameController controller = Game.GetGameController();
        if (controller != null)
        {
            controller.AddStick();
        }
    }
}
