using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public void StartLevel()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoCancelAction();
        }
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