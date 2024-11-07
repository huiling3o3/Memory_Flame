using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CutSceneEnd()
    {
        Game.GetGameController().TogglePause();
        gameObject.SetActive(false);
    }
}
