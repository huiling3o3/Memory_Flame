using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Controller : Scene_Manager
{
    
    [SerializeField] private bool isStarted;    
    public Transform startPosition;
    private PlayerController player;
    private List<EnemyController> enemyList;
    private List<CampFireController> campFireList;
    private List<CutTree> treeList;


    private void Awake()
    {

    }
    public override void Initialize(GameController aController, InputHandler handler)
    {
        isStarted = false;

        base.Initialize(aController, handler);

        //initialize player 
        player = Game.GetPlayer();
        player.Init(this);

        //initialise the campfire
        if (campFireList == null)
        {
            campFireList = new List<CampFireController>();
            campFireList.AddRange(FindObjectsOfType<CampFireController>());
        }

        foreach (CampFireController campfire in campFireList)
        {
            campfire.Initialize(this);
        }

        //initialise the interactable tree
        if (treeList == null)
        {
            treeList = new List<CutTree>();
            treeList.AddRange(FindObjectsOfType<CutTree>());
        }

        foreach (CutTree tr in treeList)
        {
            tr.gameObject.SetActive(true);
            //treeList.Initialize(this);
        }

        //initialize all enemies 
        if (enemyList == null)
        {
            enemyList = new List<EnemyController>();
            enemyList.AddRange(FindObjectsOfType<EnemyController>());
        }

        foreach (EnemyController enemy in enemyList)
        {
            enemy.gameObject.SetActive(true);
            enemy.Init(player.gameObject);
        }

        gameController.StartLevel(player);

        isStarted = true;
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
       
    }
}
