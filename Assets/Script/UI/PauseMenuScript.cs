using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour, IInputReceiver
{
    private GameController gameController;
    [SerializeField] private TextMeshProUGUI titleText;
    public void InitializeMenu(GameController gameController)
    {
        this.gameController = gameController;
        UpdateText();
    }
    private void UpdateText()
    {
        //format game over text display
        titleText.text = "Game Paused";

        if (gameController.isGameOver)
        {
            titleText.text = "Game Over";
        }
        else
        {
            titleText.text = "Game Paused";
        }
    }

    public void Restart()
    { 
        //gameController.StartGame();
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