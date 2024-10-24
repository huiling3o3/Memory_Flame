using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_Manager : Scene_Manager
{
    public override void Initialize(GameController gameController)
    {
        base.Initialize(gameController);

        //Play the lvl music
        SoundManager.PlaySound(SoundType.LEVEL1, null, 0.6f);
    }
}
