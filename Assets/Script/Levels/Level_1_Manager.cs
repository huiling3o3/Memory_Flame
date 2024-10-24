using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_Manager : Level_Controller
{
    public override void Initialize(GameController gameController, InputHandler handler)
    {
        base.Initialize(gameController, handler);

        //Play the lvl music
        SoundManager.PlaySound(SoundType.LEVEL1, null, 0.6f);
    }
}
