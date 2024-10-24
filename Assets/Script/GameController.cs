
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [Header("To be Assigned")]
    //references to assigned
    public PlayerController player;    
    InputHandler inputHandler;
    InteractHandler interactHandler;
    MenuSceneManager menuSceneManager;
    Scene_Manager currentSceneManager;
    public sceneName initialScene;

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
    public static event Action<bool> OnGameResumed;  // Event fired when the game is resumed

    private void Awake()
    {
        //Set the reference to Game
        Game.SetGameController(this);
        menuSceneManager = GetComponent<MenuSceneManager>();
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
        //Game.SetPlayer(player);
        //SetPause(false);
        //show start menu
        OpenStartMenu();       
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) return;
        //proceed game timers
        gameTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Game.GetWaveManager().NextWave();
        }

        branchCollectedChanged?.Invoke(branchCollected);
    }

    public void StartGame()
    {
        isPaused = true;

        if (isGameOver)
        {
            RestartGame();
        }
        else
        {
            //Start Lvl One
            OpenLevel("Level_1");
        }
        
        ///!!important must set player to reeceive the input for it to move
        SetPlayerInputReciever();
        SetPlayerShootInteractReciever();

        //reset game variables
        Game.GetPlayer().Reset();

        //resume Game
        ResumeGame();        
    }

    public void RestartGame()
    {
        //if (currentLvl == levelType.LEVEL_1)
        //{
        //    //close the scene first 
        //    menuSceneManager.CloseMenuScene("Level_1");
        //    //restart the scene
        //    OpenLevel("Level_1");
        //}

        isPaused = false;
        isGameOver = false;
        Debug.Log("Game Restart");
    }

    IEnumerator StartWave()
    {
        //wait for 2 seconds before starting the game
        yield return new WaitForSeconds(2f);

        //Open the UI panel
        //Game.GetHUDController().OpenWaveStatsPanel();

        Debug.Log("Game Controller: Calling Start Wave");

        //RESET timers
        gameTimer = 0;
        //reset the number of enemies killed
        numOfEnemiesKilled = 0;
        totalNumEnemiesKilled = 0;

        //call the wave manager to start the wave of enemies
        //Game.GetWaveManager().NextWave();

        //update the HUD manager to update the UI
        UpdateHUD(numOfEnemiesKilled);

        //resume Game
        ResumeGame();
    }

    public void GameOver()
    {
        isGameOver = true;
        isPaused = false;
        //TogglePause();
    }
    public bool CheckGameOver()
    {
        //check if game over
        return isGameOver;
    }
    public void EnemyKilled()
    {
        numOfEnemiesKilled++;
        totalNumEnemiesKilled++;
        //Check if all the current wave of enemies are killed if killed
        if (numOfEnemiesKilled == Game.GetWaveManager().GetEnemyCountInWave())
            {
                //call the wave manager to start the next wave of enemies
                //Game.GetWaveManager().NextWave();
                //reset the number of enemies killed
                numOfEnemiesKilled = 0;
            }

        //update the HUD manager to update the UI on the wave stats to get the number of enemies left 
        UpdateHUD(numOfEnemiesKilled);
    }
    public static void UpdateHUD(int numOfEnemiesKilled)
    {
        //Game.GetHUDController().UpdateWaveStats(Game.GetWaveManager().GetCurrentWave(), Game.GetWaveManager().GetEnemyCountInWave() - numOfEnemiesKilled);
    }
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

    public void LoadScene(sceneName aScene)
    {
        AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(aScene.ToString(), LoadSceneMode.Additive);
        loadSceneOp.completed += (result) =>
        {
            StartMenuScript menuScript = FindObjectOfType<StartMenuScript>();
            if (menuScript != null)
            {
                //set input receiver
                inputHandler.SetInputReceiver(menuScript);
            }

            Scene scene = SceneManager.GetSceneByName(aScene.ToString());
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootGameObjects)
            {
                currentSceneManager = rootObject.GetComponentInChildren<Scene_Manager>();
                if (currentSceneManager != null)
                {
                    // Initialize the scene controller
                    currentSceneManager.Initialize(this);
                    break;
                }                            
            }            
        };
    }

    public void RemoveScene(sceneName aScene)
    {
        Scene scene = SceneManager.GetSceneByName(aScene.ToString());
        SceneManager.UnloadSceneAsync(scene);
    }

    public void RestartLevel()
    {
        if (currentSceneManager != null) currentSceneManager.Initialize(this);
    }

    public void StartLevel(PlayerController playerScript)
    {
        player = playerScript;

        ///!!important must set player to reeceive the input for it to move
        SetPlayerInputReciever();
        SetPlayerShootInteractReciever();

        //set game ongoing
        //SetGameOver(false, false, 0, 0);
        //SetPause(false);
    }

    public void SetPause(bool aPause)
    {
        //set pause state
        isPaused = aPause;

        Debug.Log("Game Paused");

        // Fire the OnGamePaused event
        OnGamePaused?.Invoke(isPaused);
    }

    public void ResumeGame()
    {     
        isPaused = false;
        Debug.Log("Game Resumed");

        //set input handler to movement script
        inputHandler.SetInputReceiver(player.GetComponent<PlayerMovement>());
        interactHandler.SetInteractReceiver(player.GetComponent<PlayerShoot>());
        // Fire the OnGameResumed event
        OnGameResumed?.Invoke(isPaused);
    }

    public void PauseGame()
    {
        isPaused = true;
        Debug.Log("Game Paused");       

        PauseMenuScript recieverScript = FindObjectOfType<PauseMenuScript>();
        if (recieverScript != null)
        {
            //set input receiver
            inputHandler.SetInputReceiver(recieverScript);
            interactHandler.SetInteractReceiver(null);
        }

        // Fire the OnGamePaused event
        OnGamePaused?.Invoke(isPaused);
    }

    public void ClosePauseMenu()
    {
        menuSceneManager.CloseMenuScene("PauseMenuScene");
    }

    public void TogglePause()
    {
        //PauseGame();
        SetPause(!isPaused);
        //initialize menu after scene finishes loading
        PauseMenuScript menuScript = FindObjectOfType<PauseMenuScript>();
        menuScript.InitializeMenu(this);

        inputHandler.SetInputReceiver(menuScript);
    }

    public void OpenLevel(string lvl)
    {
        PauseGame();

        CloseStartMenu();

        menuSceneManager.OpenMenuScene(lvl, () =>
        {
            //initialize lvl 1 manager after scene finishes loading
            Level_1_Manager lvlManager = FindObjectOfType<Level_1_Manager>();
            lvlManager.Initialize(this);
        });
    }

    public void OpenStartMenu()
    {
        //PauseGame();

        //ClosePauseMenu();

        LoadScene(sceneName.StartMenuScene);
        //StartMenuScript menuScript = FindObjectOfType<StartMenuScript>();
        //set input receiver
        //inputHandler.SetInputReceiver(menuScript);

        //menuSceneManager.OpenMenuScene("StartMenuScene", () =>
        //{
        //    //initialize menu after scene finishes loading
        //    StartMenuScript menuScript = FindObjectOfType<StartMenuScript>();
        //    //menuScript.InitializeMenu(this);

        //    //set input receiver
        //    inputHandler.SetInputReceiver(menuScript);
        //    menuScript.ShowStartMenu();
        //});
    }

    public void CloseStartMenu()
    {
        menuSceneManager.CloseMenuScene("StartMenuScene");
    }

    #endregion
}
