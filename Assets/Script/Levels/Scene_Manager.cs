using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Manager : MonoBehaviour
{
    protected GameController gameController;
    protected InputHandler inputHandler;
    public sceneType SceneName;
    
    public virtual void Initialize(GameController gameController, InputHandler handler)
    {
        this.gameController = gameController;    
        this.inputHandler = handler;
    }
}

public enum sceneType
{
    LEVEL_1, LEVEL_2, LEVEL_3, StartMenuScene, PauseMenuScene
}
