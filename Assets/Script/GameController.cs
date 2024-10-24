
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [Header("To be Assigned")]
    //references to assigned
    [SerializeField] PlayerController player;    
    InputHandler inputHandler;
    InteractHandler interactHandler;
    Scene_Manager currentSceneManager;

    [Header("Game Stats")]
    public int numOfEnemiesKilled = 0;
    public int totalNumEnemiesKilled = 0;
    private static Dictionary<MemoryFragType, bool> memoryFragmentsList = new Dictionary<MemoryFragType, bool>(); //track sequences for achievements
    [SerializeField] int memoryFragmentsCollected = 0;
    [SerializeField] int branchCollected = 0;
    private float gameTimer;

    public bool isPaused = false;
    public bool isGameOver = false;

    [Header("Database")]
    [SerializeField] private List<string> fileNameList;

    // Event that notifies subscribers when the current ammo changes
    public static event Action<int> branchCollectedChanged;
    public static event Action<MemoryFragType> memFragmentsCollected;
    public static event Action<bool> OnGamePaused;   // Event fired when the game is paused

    private void Awake()
    {
        //Set the reference to Game
        Game.SetGameController(this);
        inputHandler = GetComponent<InputHandler>();
        interactHandler = GetComponent<InteractHandler>();
        //load csv data from listed files
        DataManager.LoadCSVData(fileNameList);

        //set up initial state
        InitializeGame();
    }

    private void InitializeGame()
    {
        //reset
        memoryFragmentsList.Clear();

        //initialise the memory fragment list first
        memoryFragmentsList.Add(MemoryFragType.HEADBAND, false);
        memoryFragmentsList.Add(MemoryFragType.BROKENSWORD, false);
        memoryFragmentsList.Add(MemoryFragType.NECKLACE, false);        
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.SetPlayer(player);
        SetPause(false);
        //show start menu
        OpenStartMenu();       
    }   

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) return;
        //proceed game timers
        gameTimer += Time.deltaTime;

        branchCollectedChanged?.Invoke(branchCollected);
    }

    #region Game Settings
    public void GameOver()
    {
        isGameOver = true;
        isPaused = false;
    }

    public bool CheckGameOver()
    {
        //check if game over
        return isGameOver;
    }

    public void StartLevel(PlayerController playerScript)
    {
        player = playerScript;

        ///!!important must set player to reeceive the input for it to move
        SetPlayerInputReciever();
        SetPlayerShootInteractReciever();

        //reset game variables
        Game.GetPlayer().Reset();

        //set game ongoing
        //SetGameOver(false, false, 0, 0);
        SetPause(false);
    }

    public void SetPause(bool aPause)
    {
        //set pause state
        isPaused = aPause;

        if (isPaused)
        {
            Debug.Log("Game Paused");
        }
        else
        {
            Debug.Log("Game Resume");
        }

        // Fire the OnGamePaused event
        OnGamePaused?.Invoke(isPaused);
    }
    #endregion

    #region Game Variables function
    public int GetSticks()
    {
        int sticksToGive = 0;
        if (branchCollected != 0)
        {
            sticksToGive = branchCollected;
            //clear the sticks
            branchCollected = 0;
        }
        return sticksToGive;
    }
    public void AddStick()
    {
        branchCollected++;
        // Trigger the event with the updated branch collected
        branchCollectedChanged?.Invoke(branchCollected);
        Debug.Log("Branch Amt: " + branchCollected);
    }

    public void AddMemoryFragment(MemoryFragType mf)
    {
        //check if all the list have been collected
        if (CheckFragmentCollectedAll())
        { return; }

        if (memoryFragmentsList.ContainsKey(mf))
        {
            //set the memory fragment to be found
            memoryFragmentsList[mf] = true;
            memFragmentsCollected.Invoke(mf);
        }

        memoryFragmentsCollected++;
        Debug.Log("Memory Fragments: " + memoryFragmentsCollected);
    }

    public bool CheckFragmentCollectedAll()
    {
        bool collectedAll = false;

        foreach (KeyValuePair<MemoryFragType, bool> kvp in memoryFragmentsList)
        {
            Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
            if (kvp.Value == false)
            {
                collectedAll = false;
            }
            else
            {
                collectedAll = true;
            }
        }

        return collectedAll;
    }
    #endregion 

    #region input

    public void SetPlayerInputReciever()
    {
        //set input handler to movement script
        inputHandler.SetInputReceiver(player.GetComponent<PlayerMovement>()); ;
    }

    public void SetTreeInteractReciever(CutTree tr)
    { 
        //set the input handler to the tree interacting with the player
        interactHandler.SetInteractReceiver(tr);
    }

    public void SetPlayerShootInteractReciever()
    {
        //set the input handler to the player weapon 
        interactHandler.SetInteractReceiver(player.GetComponent<PlayerShoot>());
    }
    #endregion

    #region Scene Manager

    public void LoadScene(sceneType aScene)
    {
        AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(aScene.ToString(), LoadSceneMode.Additive);
        loadSceneOp.completed += (result) =>
        {
            Scene scene = SceneManager.GetSceneByName(aScene.ToString());
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootGameObjects)
            {
                currentSceneManager = rootObject.GetComponentInChildren<Scene_Manager>();
                if (currentSceneManager != null)
                {
                    // Initialize the scene controller
                    currentSceneManager.Initialize(this, inputHandler);
                    break;
                }                            
            }            
        };
    }

    public void RemoveScene(sceneType aScene)
    {
        Scene scene = SceneManager.GetSceneByName(aScene.ToString());
        SceneManager.UnloadSceneAsync(scene);
    }

    public void RestartLevel()
    {
        if (currentSceneManager != null) currentSceneManager.Initialize(this, inputHandler);
    }

    public void ClosePauseMenu()
    {
        RemoveScene(sceneType.PauseMenuScene);
        //resume game
        SetPause(false);
        ///!!important must set player to reeceive the input for it to move
        SetPlayerInputReciever();
        SetPlayerShootInteractReciever();
    }

    public void OpenPauseMenu()
    {
        SetPause(true);
        interactHandler.SetInteractReceiver(null);
        LoadScene(sceneType.PauseMenuScene);       
    }

    public void OpenStartMenu()
    {
        SetPause(false);
        if (currentSceneManager != null) RemoveScene(currentSceneManager.SceneName);
        LoadScene(sceneType.StartMenuScene);
    }

    #endregion
}
