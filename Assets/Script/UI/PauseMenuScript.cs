using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour, IInputReceiver
{
    private GameController gameController;

    public void InitializeMenu(GameController gameController)
    {
        this.gameController = gameController;
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
        //resume game
        gameController.ResumeGame();
    }

}