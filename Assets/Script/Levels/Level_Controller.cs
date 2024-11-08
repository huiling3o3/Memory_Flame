using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class Level_Controller : Scene_Manager
{    
    [SerializeField] private bool isStarted;
    [SerializeField] CampFireController tutorialCampfire;
    string initialInstructions;
    public Transform startPosition;
    private PlayerController player;
    private List<Collectible> collectiblesList;
    private List<EnemyController> enemyList;
    private List<CampFireController> campFireList;
    private List<MemoryFragment> fragmentsList;
    private List<CutTree> treeList;
    private fireTorch torch;
    private EndPoint endPoint;
    private TaskManager taskManager;
    private AudioSource audioSource;
    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        audioSource = GetComponent<AudioSource>();
        List<string> instructions = new List<string>()
        {
            "I'm feeling very COLD, I need something WARM",
            "Oh no the campfire is dying, let's keep it burnning",
            "I need more wood to sustain the FIRE, look for trees"
        };

        initialInstructions = "I'm feeling very COLD, I need something WARM";
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
            tr.Init();
        }

        //initialize all enemies 
        if (enemyList == null)
        {
            enemyList = new List<EnemyController>();
            enemyList.AddRange(FindObjectsOfType<EnemyController>());
        }

        foreach (EnemyController enemy in enemyList)
        {            
            //enemy.gameObject.SetActive(true);
            enemy.Init(player.gameObject);
        }

        //initialise the firetorch
        if (torch == null)
        {
            torch = FindObjectOfType<fireTorch>();           
        }

        torch.gameObject.SetActive(true);

        //initialise the end point
        if (endPoint == null)
        {
            endPoint = FindObjectOfType<EndPoint>();
        }
        endPoint.gameObject.SetActive(false);

        //initialise the collectables
        if (collectiblesList == null)
        {
            collectiblesList = new List<Collectible>();
            collectiblesList.AddRange(FindObjectsOfType<Collectible>());
        }

        foreach (Collectible col in collectiblesList)
        {
            col.Init();
            col.gameObject.SetActive(true);
        }

        //initialise the memory frag 
        if (fragmentsList == null)
        {
            fragmentsList = new List<MemoryFragment>();
            fragmentsList.AddRange(FindObjectsOfType<MemoryFragment>());
        }

        foreach (MemoryFragment mf in fragmentsList)
        {
            mf.Init();
            mf.gameObject.SetActive(true);
        }

        //initialise game variables
        gameController.StartLevel(player);

        // Display the first instruction
        Game.GetHUDController().ShowInstructions(initialInstructions);

        //play audio
        SoundManager.PlaySound(SoundType.LEVELAMBIENCE, audioSource,0.2f);

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
        if (!isStarted)
        {
            return;

        }

        if (!taskManager.AreAllTasksCompleted())
        {
            //Check all the task
            if (player.IsWarmed())
            {
                taskManager.SetTaskCompleted(TaskType.PLAYER_WARMED);
            }
            else if (tutorialCampfire.IsRefilledOnce())
            {
                taskManager.SetTaskCompleted(TaskType.FIRE_REFUELLED_ONCE);
            }
            else if (tutorialCampfire.IsRefilledAgain())
            {
                taskManager.SetTaskCompleted(TaskType.FIRE_REFUELLED_AGAIN);
            }
        }

        if (Game.GetGameController().CheckFragmentCollectedAll())
        {
            endPoint.gameObject.SetActive(true);
        }
    }
}


