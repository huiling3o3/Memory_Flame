using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : Scene_Manager, IInputReceiver
{
    // Start is called before the first frame update
    public override void Initialize(GameController gameController, InputHandler handler)
    {
        base.Initialize(gameController, handler);
        inputHandler.SetInputReceiver(this);
    }

    public void DoMoveDir(Vector2 aDir)
    {
        //do nothing
    }
    public void DoLeftAction()
    {
        //do nothing
    }

    public void DoRightAction()
    {
        //do nothing
    }

    public void DoDash()
    {
        //do nothing
    }
    public void DoSubmitAction()
    {
        //Open start menu
        gameController.OpenStartMenu();
    }

    public void DoCancelAction()
    {

    }
}
