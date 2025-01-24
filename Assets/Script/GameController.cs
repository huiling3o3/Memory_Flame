
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//controls the game flow and key variables to make and break the game
public class GameController : MonoBehaviour
{
    [Header("To be Assigned")]
    //references to assigned    
    [SerializeField] Animator transitionAnimtor;    
    [SerializeField] GameObject PauseMenu;

    InteractHandler interactHandler;
    HUDController hudController;

    [Header("Game Stats")]
    private static Dictionary<MemoryFragType, bool> memoryFragmentsList = new Dictionary<MemoryFragType, bool>(); //track sequences for achievements
    [SerializeField] int memoryFragmentsCollected = 0;
    [SerializeField] int branchCollected = 0;
    [SerializeField] bool firetorchCollected = false;

    public bool isPaused = false;
    public bool isGameOver = false;
    public bool reachedEnd = false;

    // Event that notifies subscribers when the current ammo changes
    public static event Action<int> branchCollectedChanged;
    public static event Action<MemoryFragType> memFragmentsCollected;
    public static event Action<bool> OnGamePaused;   // Event fired when the game is paused

    private void Awake()
    {
        //Set the reference to Game
        Game.SetGameController(this);
        interactHandler = GetComponent<InteractHandler>();
        hudController = GetComponent<HUDController>();

        //initialise the memory fragment list first
        memoryFragmentsList.Add(MemoryFragType.HEADBAND, false);
        memoryFragmentsList.Add(MemoryFragType.BROKENSWORD, false);
        memoryFragmentsList.Add(MemoryFragType.NECKLACE, false);
    }

    private void InitializeGame()
    {
        //reset
        memoryFragmentsList[MemoryFragType.HEADBAND] = false;
        memoryFragmentsList[MemoryFragType.BROKENSWORD] = false;
        memoryFragmentsList[MemoryFragType.NECKLACE] = false;

        isPaused = false;
        isGameOver = false;
        reachedEnd = false;

        memoryFragmentsCollected = 0;
        branchCollected = 0;
        firetorchCollected = false;

        hudController.Reset();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartLevel();
    }   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //Cheat Code
        {
            branchCollected += 10;
            branchCollectedChanged?.Invoke(branchCollected);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    #region Game Settings
    public void GameOver()
    {
        isGameOver = true;
        if (CheckFragmentCollectedAll() && reachedEnd)
        {
            hudController.DisplayGameOver("Game Complete");
            Debug.Log("Game Completed");        
        }
        else
        {
            hudController.DisplayGameOver("Game Lose");
            Debug.Log("Game lose");
        }
    }

    public bool CheckGameOver()
    {
        //check if game over
        return isGameOver;
    }

    public void StartLevel()
    {

        //do not allow the player to have weapon at the start
        interactHandler.SetInteractReceiver(null);
        firetorchCollected = false;

        //reset game variables
        InitializeGame();

        //set game ongoing
        SetPause(false, false);
        hudController.HideGameOver();
    }

    public void SetPause(bool aPause, bool showMenu)
    {
        //set pause state
        isPaused = aPause;

        // Fire the OnGamePaused event
        OnGamePaused?.Invoke(isPaused);

        //show pause screen
        PauseMenu.SetActive(showMenu);
    } 

    #endregion

    #region Game Variables function
    public void CollectFireTorch()
    {
        firetorchCollected = true;
        SetPlayerShootInteractReciever();
    }

    public bool HaveFireTorch()
    {
        return firetorchCollected;
    }

    public int GetSticks()
    {
        return branchCollected;
    }
    public void AddStick()
    {
        branchCollected++;
        // Trigger the event with the updated branch collected
        branchCollectedChanged?.Invoke(branchCollected);
        Debug.Log("Branch Amt: " + branchCollected);
    }

    public void RemoveStick(int branchAmt)
    {
        branchCollected = branchCollected - branchAmt;
        // Trigger the event with the updated branch collected
        branchCollectedChanged?.Invoke(branchCollected);
        Debug.Log("Branch Amt left: " + branchCollected);
    }

    public void AddMemoryFragment(MemoryFragType mf)
    {
        if (memoryFragmentsList.ContainsKey(mf))
        {
            //set the memory fragment to be found
            memoryFragmentsList[mf] = true;
            //Update the HUD to collect the memFrag
            memFragmentsCollected.Invoke(mf);
            //display the cut scene animation
            hudController.ShowCutScene(mf);
        }

        memoryFragmentsCollected++;
        Debug.Log("Memory Fragments: " + memoryFragmentsCollected);
    }

    public bool CheckFragmentCollectedAll()
    {
        return memoryFragmentsCollected == memoryFragmentsList.Count;
    }
    #endregion 

    public void SetTreeInteractReciever(CutTree tr)
    { 
        //set the input handler to the tree interacting with the player
        interactHandler.SetInteractReceiver(tr);
    }

    public void SetPlayerShootInteractReciever()
    {
        if (HaveFireTorch())
        {
            PlayerController player = Game.GetPlayer();
            //set the input handler to the player weapon 
            interactHandler.SetInteractReceiver(player.GetComponent<PlayerShoot>());
        }
        else
        {
            interactHandler.SetInteractReceiver(null);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void TogglePause()
    {
        SetPause(!isPaused, !isPaused);
    }

    public void OpenStartMenu()
    {
        SceneManager.LoadScene(0);
    }

}
