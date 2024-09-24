using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuSceneManager : MonoBehaviour
{
    public void OpenMenuScene(string menuName, UnityAction actionOnLoad)
    {
        //add scene asynchronously
        SceneManager.LoadSceneAsync(menuName, LoadSceneMode.Additive).completed += (asyncOp) =>
        {
            //run actionOnLoad after scene finishes loading
            actionOnLoad?.Invoke();
        };
    }

    public void CloseMenuScene(string menuName)
    {
        //find scene by name
        Scene toClose = SceneManager.GetSceneByName(menuName);
        if (toClose.IsValid())
        {
            //unload if valid scene found
            SceneManager.UnloadSceneAsync(toClose);
        }
    }
}