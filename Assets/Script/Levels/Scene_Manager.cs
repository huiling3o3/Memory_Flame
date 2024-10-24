using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Manager : MonoBehaviour
{
    protected GameController gameController;
    public sceneName SceneName;
    
    public virtual void Initialize(GameController gameController)
    {
        this.gameController = gameController;        
    }
}

public enum sceneName
{
    LEVEL_1, LEVEL_2, LEVEL_3, StartMenuScene
}
