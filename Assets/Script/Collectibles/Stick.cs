using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : Collectible
{
    protected override void Collect()
    {
        base.Collect(); // to handle object destruction
        CollectibleManager manager = FindObjectOfType<CollectibleManager>();
        if (manager != null)
        {
            manager.AddStick(); // add to stick count
        }
    }
}
