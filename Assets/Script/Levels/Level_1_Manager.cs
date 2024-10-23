using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_Manager : Level_Manager
{
    public override void InitializeLvl(GameController gameController)
    {
        base.InitializeLvl(gameController);

        //Play the lvl music
        SoundManager.PlaySound(SoundType.LEVEL1, null, 0.6f);
    }
}
