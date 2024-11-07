using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : Scene_Manager, IInputReceiver
{
    [SerializeField] private AudioSource audioSource;

    public override void Initialize(GameController aController, InputHandler handler)
    {
        base.Initialize(aController,handler);
        inputHandler.SetInputReceiver(this);
        SoundManager.PlaySound(SoundType.MAIN_MENU, audioSource, 0.6f);
    }

    //set start menu display
    public void ShowStartMenu()
    {
        //TODO: play main menu bgm
        //SoundManager.PlaySound(SoundType.MAIN_MENU,audioSource,0.6f);
    }

    public void StartLevel(sceneType toSwitch)
    {
        gameController.LoadScene(toSwitch);
        gameController.RemoveScene(SceneName);
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

        //start game lvl 2
        StartCoroutine(gameController.LvlTransit(sceneType.LEVEL_2));
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