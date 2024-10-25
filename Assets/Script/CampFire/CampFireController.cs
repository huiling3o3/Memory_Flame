using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireController : MonoBehaviour
{
    private Level_Controller levelController;

    // Fire System
    [Header("Fire System")]
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthDepletionRate = 0.5f;

    // List of fire sprites for different health levels
    [SerializeField] private List<Sprite> fireSprites; // List of fire sprites
    [SerializeField] SpriteRenderer fireSpriteRenderer;
    [SerializeField] GameObject instructions;

    // Event that notifies subscribers when the current ammo changes
    public static event Action<float> fireHealthChanged;

    void Awake()
    {
        //fireSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Initialize(Level_Controller aController)
    {
        levelController = aController;
        currentHealth = maxHealth; // Initialize health
        UpdateFireAppearance();    // Set initial fire appearance
        instructions.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.GetGameController().isPaused || levelController.CheckGameOver() || !levelController.CheckIsStarted())
        {
            return;
        }

        BurnFire(); // Deplete fire health over time
        UpdateFireAppearance(); // Update fire sprite based on current health
        AddBranchesToFire(); // F key to add the branches to the campfire
    }

    private void BurnFire()
    {
        // Deplete fire health over time
        currentHealth -= healthDepletionRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

        // Trigger the event with the updated ammo percentage
        fireHealthChanged?.Invoke(currentHealth);

        // If the fire health reaches 0, trigger game over
        if (!Game.GetGameController().isGameOver && currentHealth <= 0)
        {
            Debug.Log("Fire Burned Out!!!");
            // Trigger game over event
            Game.GetGameController().GameOver();
        }
    }
    private void BorrowFire(int amount)
    {
        // take away the fire health used to replenish the fire torch
        currentHealth -= maxHealth * amount / 100f;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

        // If the fire health reaches 0, trigger game over
        if (!levelController.CheckGameOver() && currentHealth <= 0)
        {
            Debug.Log("Fire Burned Out!!!");
            // Trigger game over event
            Game.GetGameController().GameOver();
        }
    }

    private void UpdateFireAppearance()
    {
        if (fireSprites.Count == 0) { return; }

        // Calculate the current fire health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Determine which sprite to display based on the health percentage
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (fireSprites.Count - 1));

        // Clamp the index to ensure it's within the bounds of the list
        spriteIndex = Mathf.Clamp(spriteIndex, 0, fireSprites.Count - 1);

        // Update the sprite renderer with the selected sprite
        fireSpriteRenderer.sprite = fireSprites[spriteIndex];
    }

    public void AddBranches(int branchAmount)
    {
        // Increase the fire health based on the amount of branch the player had collected
        // Each branch is worth 2 points
        float healthIncreaseAmount = branchAmount * 2;
        currentHealth += healthIncreaseAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to max value
        Game.GetGameController().RemoveStick(branchAmount);
    }


    void AddBranchesToFire()
    {
        //Check if player is within the fire place to add branches
        if (!levelController.GetPlayer().IsPlayerInSafeZone()) { return; }

        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to add branch 
        {
            //Check in the game controller whether the player have enough branch
            int sticks = Game.GetGameController().GetSticks();
            if (sticks > 0)
            {
                AddBranches(sticks);// if enough update the branches num in game controller & increase the branch health 
                Debug.Log($"{sticks} given to the fire");
            }
            else
            {
                Debug.Log($"not enough branches");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1)) // Press '1' to Increase fire health by 20%
        {
            //AddBranches(10);           
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2)) // Press '2' to Increase fire health by 35%
        {
            AddBranches(15); 
        }
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
