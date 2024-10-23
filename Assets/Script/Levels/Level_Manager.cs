using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    private GameController gameController;
    [SerializeField]
    Transform startPoint;

    public virtual void InitializeLvl(GameController gameController)
    {
        this.gameController = gameController;
        //assign the player to the start point
        if (startPoint != null)
        {
            gameController.playerObj.transform.position = startPoint.position;
        }
        else
        {
            Debug.LogWarning("Player start point is not assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
