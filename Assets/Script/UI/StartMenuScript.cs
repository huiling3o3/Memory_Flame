using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour, IInputReceiver
{
    private GameController gameController;

    private int success;
    private int numOfEnemiesKilled;
    private float timer;

    public void InitializeMenu(GameController gameController)
    {
        this.gameController = gameController;
    }

    //set start menu display
    public void ShowStartMenu(int wave, int numOfEnemiesKilled, float timer)
    {
        this.success = wave;
        this.numOfEnemiesKilled = numOfEnemiesKilled;
        this.timer = timer;
        UpdateMenuText();
    }

    private void UpdateMenuText()
    {
        //format game over text display
        Text gameOverText = this.GetComponentInChildren<Text>();
        gameOverText.text = "Survival Game\n";

        if (gameController.gameOver)
        {
            gameOverText.text += "\nGame Over: Game Stats\n-------------------------------";
            gameOverText.text += "\nSurvived until Wave: " + success;
            gameOverText.text += "\nEnemies Killed: " + numOfEnemiesKilled;
            gameOverText.text += "\nTime survived: " + Mathf.FloorToInt(timer) + "s";

            gameOverText.text += "\n\nSpace - Play Again";
            gameOverText.text += "\nEsc - Exit";
        }
        else
        {
            gameOverText.text += "\n\nSpace - Play";
            gameOverText.text += "\nEsc - Exit";
        }
    }

    public void DoMoveDir(Vector2 aDir)
    {
        //do nothing
    }

    public void DoLeftAction()
    {

    }

    public void DoRightAction()
    {

    }

    public void DoDash()
    {
        //do nothing
    }

    public void DoSubmitAction()
    {
        //start game again
        gameController.StartGame();
    }

    public void DoCancelAction()
    {
#if UNITY_EDITOR
        //if in unity editor, stop playing
        UnityEditor.EditorApplication.isPlaying = false;
#else
            //if not in unity editor, quit application
            Application.Quit();
#endif
    }
}