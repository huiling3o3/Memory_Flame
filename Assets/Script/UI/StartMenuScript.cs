using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour, IInputReceiver
{
    private GameController gameController;
    [SerializeField] private AudioSource audioSource;

    public void InitializeMenu(GameController gameController)
    {
        this.gameController = gameController;
    }

    //set start menu display
    public void ShowStartMenu()
    {
        //TODO: play main menu bgm
        SoundManager.PlaySound(SoundType.MAIN_MENU,audioSource,0.6f);
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