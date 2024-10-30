using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampFireController : MonoBehaviour
{
    // Fire System
    [Header("Fire System")]
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthDepletionRate = 0.5f;
    [SerializeField] private int branchHealAmount = 2;
    [SerializeField] private int currentBranches = 0;
    [SerializeField] private int amountToReviveFire = 10;
    [SerializeField] private float reviveFireStartingHealth = 50f;
    [SerializeField] private FireState InitialState = FireState.Extinguished;
    [SerializeField] private Level_Controller lvlController;
    // List of fire sprites for different health levels
    [SerializeField] private List<Sprite> fireSprites; // List of fire sprites
    [SerializeField] SpriteRenderer fireSpriteRenderer;
    [SerializeField] GameObject instructions;
    [SerializeField] TextMeshProUGUI branchTxt;

    [SerializeField]
    private Slider fireHealthBar;
    private enum FireState
    {
        Burning, 
        Extinguished
    }

    [SerializeField]
    private FireState currentFireState = FireState.Burning;

    void Awake()
    {
        //fireSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize();
    }

    public void Initialize(Level_Controller level_Controller)
    {
        // Set initial fire appearance
        UpdateFireAppearance();    
        //set the interaction apperance to false
        instructions.SetActive(false);
        //Add the lvl controller ref
        lvlController = level_Controller;
        // Initialize health
        switch (InitialState)
        {
            case FireState.Burning:
                currentHealth = maxHealth;
                currentBranches = 0;
                currentFireState = FireState.Burning;
                break;
            case FireState.Extinguished:
                currentHealth = 0;
                currentBranches = 0;
                currentFireState = FireState.Extinguished;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lvlController == null || Game.GetGameController().isPaused || Game.GetGameController().isGameOver)
        {
            return;
        }

        BurnFire(); // Deplete fire health over time
        UpdateFireAppearance(); // Update fire sprite based on current health
        AddBranchesToFire(); // F key to add the branches to the campfire
    }

    private void BurnFire()
    {
        switch (currentFireState)
        {
            case FireState.Burning:
                // Deplete fire health over time
                currentHealth -= healthDepletionRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

                if (currentHealth <= 0)
                {
                    KillFire();
                }

                break;
            case FireState.Extinguished:
                //do nothing
                break;
        }
       
    }
    private void BorrowFire(int amount)
    {
        // take away the fire health used to replenish the fire torch
        currentHealth -= maxHealth * amount / 100f;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

        // If the fire health reaches 0, trigger game over
        if (!lvlController.CheckGameOver() && currentHealth <= 0)
        {
            KillFire();            
        }
    }

    private void UpdateFireAppearance()
    {
        if (fireSprites.Count == 0) { return; }


        // Calculate the current fire health percentage
        float healthPercentage = currentHealth / maxHealth;

        fireHealthBar.value = healthPercentage;

        //display the UI
        switch (currentFireState)
        {
            case FireState.Burning:               
                //diplay the heath bar
                fireHealthBar.gameObject.SetActive(true);

                //hide the branch txt to revive
                branchTxt.gameObject.SetActive(false);

                // Determine which sprite to display based on the health percentage
                int spriteIndex = Mathf.FloorToInt(healthPercentage * (fireSprites.Count - 1));

                //Debug.Log("Fire Apperance" + spriteIndex);

                // Clamp the index to ensure it's within the bounds of the list
                spriteIndex = Mathf.Clamp(spriteIndex, 0, fireSprites.Count - 1);

                // Update the sprite renderer with the selected sprite
                fireSpriteRenderer.sprite = fireSprites[spriteIndex];
                break;
            case FireState.Extinguished:
                //hide the heath bar
                fireHealthBar.gameObject.SetActive(false);

                //show the branch txt to revive
                branchTxt.gameObject.SetActive(true);

                //update the txt of the branch count
                branchTxt.text = $"{currentBranches} / {amountToReviveFire}";
                break;

        }        
    }

    public void AddBranches(int branchAmount)
    {
        // Increase the fire health based on the amount of branch the player had collected
        // Each branch is worth 2 points
        float healthIncreaseAmount = branchAmount * branchHealAmount;
        currentHealth += healthIncreaseAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to max value
        Game.GetGameController().RemoveStick(branchAmount);
    }


    void AddBranchesToFire()
    {
        //Check if player is within the fire place to add branches
        if (!lvlController.GetPlayer().IsPlayerInSafeZone()) { return; }

        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to add branch 
        {
            //Check in the game controller whether the player have enough branch
            int sticks = Game.GetGameController().GetSticks();

            switch (currentFireState)
            {
                case FireState.Burning:
                    
                    if (sticks > 0)
                    {
                        int sticksUsed = (int)((maxHealth - currentHealth) / branchHealAmount) + 1;
                        if (sticks >= sticksUsed)
                        {
                            AddBranches(sticksUsed);// if enough update the branches num in game controller & increase the branch health 
                        }
                        else
                        {
                            AddBranches(sticks);
                        }
                        Debug.Log($"{sticks} given to the fire");
                    }
                    else
                    {
                        Debug.Log($"not enough branches");
                    }
                    break;

                case FireState.Extinguished:
                    if (currentBranches != amountToReviveFire)
                    {
                        int currentStickRequired = amountToReviveFire - currentBranches;
                        //check if player have enough sticks
                        if (sticks > 0 && sticks < currentStickRequired) //not enough stick
                        {
                            //take all the player have and store it into the current branches
                            currentBranches += sticks;
                            //remove player current stick amt
                            Game.GetGameController().RemoveStick(sticks);
                        }
                        else if (sticks >= currentStickRequired) //enough stick
                        {
                            //remove the amt of stick from player
                            Game.GetGameController().RemoveStick(currentStickRequired);
                            //revive the fire
                            currentFireState = FireState.Burning;
                            currentHealth = maxHealth;
                            //update fire UI
                            UpdateFireAppearance();
                            //reset the current branches to revive
                            currentBranches = 0;
                        }
                    }
                    
                    //Check in the game controller whether the player have enough branch
                    //if (Game.GetGameController().GetSticks() >= amountToReviveFire)
                    //{
                    //    Game.GetGameController().RemoveStick(amountToReviveFire);
                    //    currentFireState = FireState.Burning;
                    //    currentHealth = reviveFireStartingHealth;
                    //}

                    break;
            }
        }

    }
    private void KillFire()
    {
        currentFireState = FireState.Extinguished;
        //Debug.Log("Fire Burned Out!!!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {                    
            //Player enter into a safe zone, so the ammo does not start to drop
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.EnterSafeZone();
            
            instructions.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //When player exit the safe zone the fire torch will start to deplete over time
            PlayerController pc = collision.GetComponent<PlayerController>();
            //Everytime the fire torch is regenerated, 2% of the fire is taken away
            BorrowFire(2);
            pc.ExitSafeZone();
            instructions.SetActive(false);
        }
    }

}
