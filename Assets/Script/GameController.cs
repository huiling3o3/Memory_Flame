
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [Header("To be Assigned")]
    //references to assigned
    public GameObject playerObj;    
    InputHandler inputHandler;
    InteractHandler interactHandler;
    MenuSceneManager menuSceneManager;
    PlayerController pc;

    [Header("Game Stats")]
    //Game Controller Variables
    public int numOfEnemiesKilled = 0;
    public int totalNumEnemiesKilled = 0;
    private static Dictionary<MemoryFragType, bool> memoryFragmentsList = new Dictionary<MemoryFragType, bool>(); //track sequences for achievements
    [SerializeField] int memoryFragmentsCollected = 0;
    [SerializeField] int branchCollected = 0;
    private float gameTimer;

    public bool gameIsActive = false;
    public bool gameOver = false;

    [Header("Database")]
    [SerializeField] private List<string> fileNameList;

    // Event that notifies subscribers when the current ammo changes
    public static event Action<int> branchCollectedChanged;
    public static event Action<MemoryFragType> memFragmentsCollected;
    private void Awake()
    {
        //Set the reference to Game
        Game.SetGameController(this);

        pc = playerObj.GetComponent<PlayerController>();
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
        memoryFragmentsList.Add(MemoryFragType.SHIELD, false);
        memoryFragmentsList.Add(MemoryFragType.SHOE, false);
        memoryFragmentsList.Add(MemoryFragType.BAG, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //show start menu
        OpenStartMenu();

        //initialise the player
        pc.Init();                                
        Game.SetPlayer(pc);

    }

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
    // Update is called once per frame
    void Update()
    {
        if (!gameIsActive) return;
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
        gameIsActive = true;

        if (gameOver)
        {
            RestartGame();
        }

        CloseStartMenu();

        ///!!important must set player to reeceive the input for it to move
        SetPlayerInputReciever();
        SetPlayerShootInteractReciever();

        //reset game variables
        Game.GetPlayer().Reset();

        //resume Game
        ResumeGame();

        //Start wave
        //StartCoroutine(StartWave());

        //close the wave stats ui
        //Game.GetHUDController().CloseWaveStatsPanel();
    }

    public void RestartGame()
    {
        
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
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
        gameOver = true;
        gameIsActive = false;
        OpenStartMenu();
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

    #region input

    public void SetPlayerInputReciever()
    {
        //set input handler to movement script
        inputHandler.SetInputReceiver(playerObj.GetComponent<PlayerMovement>()); ;
    }

    public void SetTreeInteractReciever(Tree tr)
    { 
        //set the input handler to the tree interacting with the player
        interactHandler.SetInteractReceiver(tr);
    }

    public void SetPlayerShootInteractReciever()
    {
        //set the input handler to the player weapon 
        interactHandler.SetInteractReceiver(playerObj.GetComponent<PlayerShoot>());
    }
    #endregion

    #region Menus

    public void ResumeGame()
    {
        //unpause game
        Time.timeScale = 1f;
        gameIsActive = true;

        //set input handler to movement script
        inputHandler.SetInputReceiver(playerObj.GetComponent<PlayerMovement>());

        //close pause menu
        ClosePauseMenu();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsActive = false;
    }

    public void ClosePauseMenu()
    {
        menuSceneManager.CloseMenuScene("PauseMenuScene");
    }

    public void OpenPauseMenu()
    {
        PauseGame();

        menuSceneManager.OpenMenuScene("PauseMenuScene", () =>
        {
            //initialize menu after scene finishes loading
            PauseMenuScript menuScript = FindObjectOfType<PauseMenuScript>();
            menuScript.InitializeMenu(this);

            inputHandler.SetInputReceiver(menuScript);
        });
    }

    public void OpenStartMenu()
    {
        PauseGame();

        ClosePauseMenu();

        menuSceneManager.OpenMenuScene("StartMenuScene", () =>
        {
            //initialize menu after scene finishes loading
            StartMenuScript menuScript = FindObjectOfType<StartMenuScript>();
            menuScript.InitializeMenu(this);

            //set input receiver
            inputHandler.SetInputReceiver(menuScript);
            menuScript.ShowStartMenu(0, totalNumEnemiesKilled, gameTimer);
        });
    }

    public void CloseStartMenu()
    {
        menuSceneManager.CloseMenuScene("StartMenuScene");
    }

    #endregion
}
