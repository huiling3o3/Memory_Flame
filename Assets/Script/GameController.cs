using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    //[Header("To be Assigned")]
    //references to assigned
    //public GameObject playerObj;    
    //public InputHandler inputHandler;    
    //public DialogueUIController dialogueUIController;
    //public NPCManager npcManager;
    //MenuSceneManager menuSceneManager;
    //Database dm;
    //PlayerController pc;

    ////initial character & weapon
    //public string initCharacter;
    //public string initWeapon;
    //[Header("Game Controller")]
    ////Game Controller Variables
    //public int numOfEnemiesKilled = 0;
    //public int totalNumEnemiesKilled = 0;
    //private float gameTimer;

    //private bool gameIsActive = false;
    //public bool gameOver = false;

    //private void Awake()
    //{
    //    //Set the reference to Game
    //    Game.SetGameController(this);

    //    //set up the database
    //    dm = GetComponent<Database>();
    //    dm.SetDatabase();

    //    //Set reference for the player
    //    pc = playerObj.GetComponent<PlayerController>();
    //    //Set reference to the menu
    //    menuSceneManager = GetComponent<MenuSceneManager>();

    //    //get the character initial character
    //    Character character = Game.GetCharacterByRefID(initCharacter);     
    //}

    
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //show start menu
    //    OpenStartMenu();

    //    //initialise the player
    //    pc.Init();
    //    Game.SetPlayer(pc);
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    if (!gameIsActive) return;
    //    //proceed game timers
    //    gameTimer += Time.deltaTime;

    //    //Cheat code to skip dialogue
    //    if (Input.GetKeyDown(KeyCode.Keypad0))
    //    {
    //        SetCharacter(initCharacter);
    //    }
    //}  

    //public void StartGame()
    //{
    //    if (gameOver)
    //    {
    //        RestartGame();
    //    }
        
    //    CloseStartMenu();
    //    //reset wave
    //    Game.GetWaveManager().WaveReset();
    //    //reset player
    //    Game.GetPlayer().Reset();
    //    //resume Game
    //    ResumeGame();
    //    //close the wave stats ui
    //    Game.GetHUDController().CloseWaveStatsPanel();
    //    npcManager.StartDialogue();
    //}

    //public void RestartGame()
    //{
    //    // Get the current active scene
    //    Scene currentScene = SceneManager.GetActiveScene();

    //    // Reload the current scene
    //    SceneManager.LoadScene(currentScene.name);
    //}

    //IEnumerator StartWave()
    //{
    //    //wait for 2 seconds before starting the game
    //    yield return new WaitForSeconds(2f);

    //    //Open the UI panel
    //    Game.GetHUDController().OpenWaveStatsPanel();

    //    Debug.Log("Game Controller: Calling Start Wave");
    //    //set player initial weapon
    //    SetWeapon(initWeapon);
    //    //SetWeapon("w102");
    //    //SetWeapon("w103");

    //    //RESET timers
    //    gameTimer = 0;
    //    //reset the number of enemies killed
    //    numOfEnemiesKilled = 0;
    //    totalNumEnemiesKilled = 0;        

    //    //call the wave manager to start the wave of enemies
    //    Game.GetWaveManager().NextWave();

    //    //update the HUD manager to update the UI
    //    UpdateHUD(numOfEnemiesKilled);

    //    //resume Game
    //    ResumeGame();
    //}

    //public void SetCharacter(string characterId)
    //{
    //    //update the player stats with the character selected info
    //    Game.GetPlayer().ChangeCharacter(characterId);

    //    //update the player UI sprite on the HP Bar
    //    Game.GetHUDController().ChangeCharacterSprite(characterId);

    //    //close menu
    //    Game.GetHUDController().CloseCharacterSelectMenu();

    //    //Start wave
    //    StartCoroutine(StartWave());
    //}

    //public void SetWeapon(string weaponId)
    //{
    //    string weaponName = Game.GetWeaponByRefID(weaponId).name;
    //    Game.GetPlayer().AddWeapon(weaponId, Game.GetWeaponManager().GetWeaponPrefab(weaponName));
    //}

    //public bool CheckInitialCharacter(string id) 
    //{
    //    if (initCharacter == id)
    //        return true;
    //    else return false;
    //}

    //public void DialogueCompleted()
    //{
    //    //Open character select menu
    //    Game.GetHUDController().OpenCharacterSelectMenu();        
    //}
    //public void GameOver()
    //{
    //    gameOver = true;
    //    OpenStartMenu();
    //    dm.WriteAnalyticsTracking(Game.GetWaveManager().GetCurrentWaveID(),totalNumEnemiesKilled,gameTimer);
    //}

    //public void EnemyKilled()
    //{
    //    numOfEnemiesKilled++;       
    //    totalNumEnemiesKilled++;
    //    //Check if all the current wave of enemies are killed if killed 
    //    if (numOfEnemiesKilled == Game.GetWaveManager().GetEnemyCountInWave())
    //    {
    //        //call the wave manager to start the next wave of enemies
    //        Game.GetWaveManager().NextWave();
    //        //reset the number of enemies killed
    //        numOfEnemiesKilled = 0;
    //    }

    //    //update the HUD manager to update the UI on the wave stats to get the number of enemies left 
    //    UpdateHUD(numOfEnemiesKilled);
    //}
    //public static void UpdateHUD(int numOfEnemiesKilled)
    //{
    //    Game.GetHUDController().UpdateWaveStats(Game.GetWaveManager().GetCurrentWave(), Game.GetWaveManager().GetEnemyCountInWave() - numOfEnemiesKilled);
    //}

    //#region input
    //public void SetDialogueReciever()
    //{
    //    //set input receiver
    //    inputHandler.SetInputReceiver(dialogueUIController);
    //}

    //public void SetPlayerInputReciever()
    //{
    //    //set input handler to movement script
    //    inputHandler.SetInputReceiver(playerObj.GetComponent<PlayerMovement>()); ;
    //}
    //#endregion

    //#region Menus

    //public void ResumeGame()
    //{
    //    //unpause game
    //    Time.timeScale = 1f;
    //    gameIsActive = true;

    //    //set input handler to movement script
    //    inputHandler.SetInputReceiver(playerObj.GetComponent<PlayerMovement>());

    //    //close pause menu
    //    ClosePauseMenu();
    //}

    //public void PauseGame()
    //{
    //    Time.timeScale = 0;
    //    gameIsActive = false;
    //}

    //public void ClosePauseMenu()
    //{
    //    menuSceneManager.CloseMenuScene("PauseMenuScene");
    //}

    //public void OpenPauseMenu()
    //{
    //    PauseGame();

    //    menuSceneManager.OpenMenuScene("PauseMenuScene", () =>
    //    {
    //        //initialize menu after scene finishes loading
    //        PauseMenuScript menuScript = FindObjectOfType<PauseMenuScript>();
    //        menuScript.InitializeMenu(this);

    //        inputHandler.SetInputReceiver(menuScript);
    //    });
    //}

    //public void OpenStartMenu()
    //{
    //    PauseGame();

    //    ClosePauseMenu();

    //    menuSceneManager.OpenMenuScene("StartMenuScene", () =>
    //    {
    //        //initialize menu after scene finishes loading
    //        StartMenuScript menuScript = FindObjectOfType<StartMenuScript>();
    //        menuScript.InitializeMenu(this);

    //        //set input receiver
    //        inputHandler.SetInputReceiver(menuScript);
    //        menuScript.ShowStartMenu(Game.GetWaveManager().GetCurrentWave(),totalNumEnemiesKilled,gameTimer);
    //    });
    //}

    //public void CloseStartMenu()
    //{
    //    menuSceneManager.CloseMenuScene("StartMenuScene");
    //}

    //#endregion
}
