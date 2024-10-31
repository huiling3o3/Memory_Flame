using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour //IInputReceiver
{
    //private GameController gameController;
    [SerializeField] private TextMeshProUGUI titleText;
    //public override void Initialize(GameController gameController, InputHandler handler)
    //{
    //    base.Initialize(gameController, handler);
    //    inputHandler.SetInputReceiver(this);
    //    //UpdateText();
    //}

    public void Restart()
    {
        //Game.GetGameController().RestartLevel();
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
        //gameController.OpenStartMenu();
        //gameController.ClosePauseMenu();
    }

    public void DoCancelAction()
    {
        //resume game
       //gameController.ClosePauseMenu();
    }

}