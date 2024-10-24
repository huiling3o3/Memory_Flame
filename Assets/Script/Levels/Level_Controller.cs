using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Controller : Scene_Manager
{
    private List<EnemyController> enemyList;
    [SerializeField] private bool isStarted;
    private PlayerController player;
    public Transform startPosition;
    private CampFireController campFireController;

    public override void Initialize(GameController aController, InputHandler handler)
    {
        isStarted = false;

        base.Initialize(aController, handler);

        //initialize player 
        if (player == null) player = FindObjectOfType<PlayerController>();
        if (player != null) player.Init(this);

        //initialise the campfire
        if(campFireController == null) campFireController = FindObjectOfType<CampFireController>();
        if (campFireController != null) campFireController.Initialize(this);

        //initialize all enemies 
        if (enemyList == null)
        {
            enemyList = new List<EnemyController>();
            enemyList.AddRange(FindObjectsOfType<EnemyController>());
        }

        foreach (EnemyController enemy in enemyList)
        {
            enemy.Init(player.gameObject);
        }

        gameController.StartLevel(player);

        isStarted = true;
    }
    public void SetGameOver(bool aGameOver, bool isWin)
    {
        //gameController.SetGameOver(aGameOver, isWin, collectedCount, collectibleList.Count);
    }

    public bool CheckGameOver()
    {
        //check if game over
        return gameController.CheckGameOver();
    }

    public bool CheckIsStarted()
    {
        return isStarted;
    }

    public PlayerController GetPlayer()
    { 
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !gameController.CheckGameOver())
        {
            
        }
        else
        {
            //no player or game not active
        }
    }
}
